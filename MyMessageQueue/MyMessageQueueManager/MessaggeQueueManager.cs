using My.MessageQueue.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.MessageQueue
{
    public class MessaggeQueueManager : MyManagerCSharp.ManagerDB
    {

        protected LogManager _log;

        public MessaggeQueueManager(string connectionName)
            : base(connectionName)
        {
            _log = new LogManager(_connection);
        }

        public MessaggeQueueManager(System.Data.Common.DbConnection connection)
            : base(connection)
        {
            _log = new LogManager(_connection);
        }

        public bool insert(Email email)
        {
            //_log = new MyManagerCSharp.Log.LogManager(_connection);

            long newId;
            newId = _insert(email);


            if (email.SendType == MessageBase.EnumSendType.Async)
            {
                //Ci sarà un task che si occupa dell'invio delle mail....
                return true;
            }


            //invio l'email....
            My.MessageQueue.MailManager mailManager = new MailManager(_connection);
            mailManager.send(email);

            return true;
        }


        public bool delete(long messageId)
        {
            _strSQL = "DELETE FROM mmq.Attachment WHERE message_id = " + messageId;
            _executeNoQuery(_strSQL);

            _strSQL = "DELETE FROM mmq.Email WHERE id = " + messageId;
            _executeNoQuery(_strSQL);

            _strSQL = "DELETE FROM mmq.log WHERE message_id = " + messageId;
            _executeNoQuery(_strSQL);

            _strSQL = "DELETE FROM mmq.Message WHERE id = " + messageId;
            return _executeNoQuery(_strSQL) == 1;
        }


        public bool deleteMember(long distributionListId, long memberId)
        {
            DistributionListManager m = new DistributionListManager(_connection);
            return m.deleteMember(distributionListId, memberId);
        }

        public bool insertMember(long distributionListId, string nome, string email)
        {
            DistributionListManager m = new DistributionListManager(_connection);
            return m.insertMember(distributionListId, nome, email);
        }

        public long insertDistributionList(string nome)
        {
            DistributionListManager m = new DistributionListManager(_connection);

            DistributionList list = new DistributionList();
            list.name = nome;

            return m.createDistributionList(list);
        }



        private long _insert(Email email)
        {
            System.Data.Common.DbCommand command;
            command = _connection.CreateCommand();

            _transactionBegin();

            _log._transactionBegin(ref _transaction);
            long newId = -1;

            try
            {
                _strSQL = "INSERT INTO mmq.Message ( date_added , send_type, message_type , [distribution_list_id] ) VALUES ( GETDATE() , @SENDTYPE , " + (int)MessageBase.EnumMessageType.Email + " , @DISTRIBUTION_LIST_ID )";
                _addParameter(command, "@SENDTYPE", (int)email.SendType);
                _addParameter(command, "@DISTRIBUTION_LIST_ID", email.distributionListId);

                command.CommandText = _strSQL;

                _executeNoQuery(command);

                newId = _getIdentity();


                _strSQL = "INSERT INTO mmq.Email ( id , body, [subject] , [to] , cc , bcc ) VALUES (  " + newId + " , @BODY ,  @SUBJECT , @TO , @CC , @BCC  )";
                command.CommandText = _strSQL;
                command.Parameters.Clear();

                _addParameter(command, "@BODY", email.Body);
                _addParameter(command, "@SUBJECT", email.Subject);
                _addParameter(command, "@TO", email.To);
                _addParameter(command, "@CC", email.Cc);
                _addParameter(command, "@BCC", email.Bcc);

                _executeNoQuery(command);

                _log.info(newId, String.Format("Messaggio creato"));

                //ATTACHMENTS
                if (email.Attachments != null && email.Attachments.Count > 0)
                {
                    foreach (Attachment att in email.Attachments)
                    {
                        _strSQL = "INSERT INTO mmq.Attachment ( message_id , date_added, name, stream ) VALUES (  " + newId + " , GetDate() ,  @NAME ,  @STREAM )";
                        command.CommandText = _strSQL;
                        command.Parameters.Clear();

                        _addParameter(command, "@NAME", att.name);
                        //   _addParameter(command, "@STREAM", att.fileStream);

                        (command as System.Data.SqlClient.SqlCommand).Parameters.Add("@STREAM", SqlDbType.Image, att.fileStream.Length).Value = att.fileStream;
                        _executeNoQuery(command);


                        _log.info(newId, String.Format("Allegato: {0}", att.name));
                    }

                    _log.info(newId, String.Format("Inseriti {0:N0} allegati", email.Attachments.Count));
                }


                _transactionCommit();
            }
            catch (Exception ex)
            {
                _transactionRollback();
                Debug.WriteLine("Exception: " + ex.Message);
                throw ex;
            }

            return newId;
        }


        public bool updateStatus(long messageId, MessageBase.EnumMessageStatus status, string errorMessage)
        {
            _strSQL = "UPDATE mmq.Message SET message_status =  " + (int)status +
                ", DATE_TRANSMISSION = GetDate() " +
                ", ERROR_MESSAGE = @MESSAGE " +
                " WHERE ID=" + messageId;

            System.Data.Common.DbCommand command;
            command = _connection.CreateCommand();
            command.CommandText = _strSQL;

            if (String.IsNullOrEmpty(errorMessage))
            {
                _addParameter(command, "@MESSAGE", DBNull.Value);
            }
            else
            {
                _addParameter(command, "@MESSAGE", errorMessage.Trim());
            }

            _executeNoQuery(command);
            return true;
        }


        #region *** Attachment ***

        public Attachment getAttachment(long id)
        {
            _strSQL = "SELECT * FROM mmq.Attachment WHERE id = " + id;
            _dt = _fillDataTable(_strSQL);

            if (_dt.Rows.Count == 0)
            {
                return null;
            }

            if (_dt.Rows.Count > 1)
            {
                throw new MyManagerCSharp.MyException(MyManagerCSharp.MyException.ErrorNumber.Record_duplicato);
            }

            Attachment attachment;
            attachment = new Attachment(_dt.Rows[0]);

            return attachment;
        }

        public void saveAttachment(Attachment attachment, string path)
        {
            System.IO.File.WriteAllBytes(path, attachment.fileStream);
        }

        #endregion



        public void getList(Models.SearchMessages model)
        {

            List<Models.MessageBase> risultato;
            risultato = new List<Models.MessageBase>();

            _strSQL = "SELECT t1.* FROM mmq.message as t1";

            System.Data.Common.DbCommand command;
            command = _connection.CreateCommand();



            string strWHERE = "";
            //if (model.filter != null)
            //{
            //    if (!String.IsNullOrEmpty(model.filter.nome))
            //    {
            //        strWHERE += " AND UPPER(nome) like  @NOME";
            //        _addParameter(command, "@NOME", "%" + model.filter.nome.ToUpper().Trim() + "%");
            //    }

            //    if (!String.IsNullOrEmpty(model.filter.cognome))
            //    {
            //        strWHERE += " AND UPPER(cognome) like  @COGNOME";
            //        _addParameter(command, "@COGNOME", "%" + model.filter.cognome.ToUpper().Trim() + "%");
            //    }

            //    if (!String.IsNullOrEmpty(model.filter.email))
            //    {
            //        strWHERE += " AND UPPER(email) like  @EMAIL";
            //        _addParameter(command, "@EMAIL", "%" + model.filter.email.ToUpper().Trim() + "%");
            //    }

            //    if (!String.IsNullOrEmpty(model.filter.login))
            //    {
            //        strWHERE += " AND UPPER(my_login) like  @MY_LOGIN";
            //        _addParameter(command, "@MY_LOGIN", "%" + model.filter.login.ToUpper().Trim() + "%");
            //    }
            //}


            if (!String.IsNullOrEmpty(strWHERE))
            {
                _strSQL += " WHERE (1=1) " + strWHERE;
            }

            string temp;
            int totalRecords;

            //paginazione
            if (model.PageSize > 0 && (_connection is System.Data.SqlClient.SqlConnection))
            {
                temp = "SELECT COUNT(*) " + _strSQL;
                command.CommandText = temp;

                totalRecords = int.Parse(_executeScalar(command));
                model.TotalRows = totalRecords;
            }



            temp = _strSQL + " ORDER BY " + model.Sort + " " + model.SortDir;


            if (model.PageSize > 0 && (_connection is System.Data.SqlClient.SqlConnection))
            {
                temp += " OFFSET " + ((model.PageNumber - 1) * model.PageSize) + " ROWS FETCH NEXT " + model.PageSize + " ROWS ONLY";
            }


            command.CommandText = temp;
            command.Connection = _connection;
            _dt = _fillDataTable(command);

            if (model.PageSize > 0 && !(_connection is System.Data.SqlClient.SqlConnection))
            {
                model.TotalRows = _dt.Rows.Count;

                // apply paging
                IEnumerable<DataRow> rows = _dt.AsEnumerable().Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize);
                foreach (DataRow row in rows)
                {
                    risultato.Add(new Models.MessageBase(row));
                }
            }
            else
            {
                foreach (DataRow row in _dt.Rows)
                {
                    risultato.Add(new Models.MessageBase(row));
                }
            }

            model.Messages = risultato;
        }


        public MessageBase getMessage(long id)
        {
            _strSQL = "SELECT * FROM mmq.message WHERE id = " + id;
            _dt = _fillDataTable(_strSQL);

            if (_dt.Rows.Count == 0)
            {
                return null;
            }

            if (_dt.Rows.Count > 1)
            {
                throw new MyManagerCSharp.MyException("id > 0");
            }

            MessageBase message;
            message = new MessageBase(_dt.Rows[0]);

            // LOGS
            message.Logs = getLogs(id);

            //Distribution List
            if (message.distributionListId != null)
            {
                message.distributionList = getDistributionList((long)message.distributionListId);
            }

            return message;
        }


        public Email getEmail(long id)
        {
            My.MessageQueue.MailManager manager = new MailManager(_connection);
            return manager.getEmail(id);
        }


        public void setEmailByMessage(Email mail, MessageBase message)
        {
            mail.comunicationId = message.comunicationId;
            mail.dateTransmission = message.dateTransmission;
            mail.dateAdded = message.dateAdded;
            mail.SendType = message.SendType;
            mail.MessageType = message.MessageType;
            mail.MessageStatus = message.MessageStatus;
            mail.errorMessage = message.errorMessage;

            mail.Logs = message.Logs;

            mail.distributionList = message.distributionList;
            mail.distributionListId = message.distributionListId;
        }


        public List<DistributionList> getListeDiDistribuzione()
        {
            My.MessageQueue.DistributionListManager manager = new DistributionListManager(_connection);
            return manager.getList();
        }


        public DistributionList getDistributionList(long id)
        {
            My.MessageQueue.DistributionListManager manager = new DistributionListManager(_connection);
            return manager.getDistributionList(id);
        }

        public bool send(Email email)
        {
            My.MessageQueue.MailManager manager = new MailManager(_connection);
            return manager.send(email);
        }


        public List<Models.Log> getLogs(long messageId)
        {
            return _log.getLogs(messageId);
        }
    }
}

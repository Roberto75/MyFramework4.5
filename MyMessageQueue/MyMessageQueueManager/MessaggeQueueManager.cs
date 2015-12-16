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
            _log = new LogManager(mConnection);
        }

        public MessaggeQueueManager(System.Data.Common.DbConnection connection)
            : base(connection)
        {
            _log = new LogManager(mConnection);
        }

        public bool insert(Email email)
        {
            //_log = new MyManagerCSharp.Log.LogManager(mConnection);

            long newId;
            newId = _insert(email);


            if (email.SendType == MessageBase.EnumSendType.Async)
            {
                //Ci sarà un task che si occupa dell'invio delle mail....
                return true;
            }


            //invio l'email....
            My.MessageQueue.MailManager mailManager = new MailManager(mConnection);
            mailManager.send(email);

            return true;
        }


        public bool delete(long messageId)
        {
            mStrSQL = "DELETE FROM mmq.Attachment WHERE message_id = " + messageId;
            mExecuteNoQuery(mStrSQL);

            mStrSQL = "DELETE FROM mmq.Email WHERE id = " + messageId;
            mExecuteNoQuery(mStrSQL);

            mStrSQL = "DELETE FROM mmq.log WHERE message_id = " + messageId;
            mExecuteNoQuery(mStrSQL);

            mStrSQL = "DELETE FROM mmq.Message WHERE id = " + messageId;
            return mExecuteNoQuery(mStrSQL) == 1;
        }


        public bool deleteMember(long distributionListId, long memberId)
        {
            DistributionListManager m = new DistributionListManager(mConnection);
            return m.deleteMember(distributionListId, memberId);
        }

        public bool insertMember(long distributionListId, string nome, string email)
        {
            DistributionListManager m = new DistributionListManager(mConnection);
            return m.insertMember(distributionListId, nome, email);
        }

        public long insertDistributionList(string nome)
        {
            DistributionListManager m = new DistributionListManager(mConnection);

            DistributionList list = new DistributionList();
            list.name = nome;

            return m.createDistributionList(list);
        }



        private long _insert(Email email)
        {
            System.Data.Common.DbCommand command;
            command = mConnection.CreateCommand();

            mTransactionBegin();

            _log.mTransactionBegin(ref mTransaction);
            long newId = -1;

            try
            {
                mStrSQL = "INSERT INTO mmq.Message ( date_added , send_type, message_type , [distribution_list_id] ) VALUES ( GETDATE() , @SENDTYPE , " + (int)MessageBase.EnumMessageType.Email + " , @DISTRIBUTION_LIST_ID )";
                mAddParameter(command, "@SENDTYPE", (int)email.SendType);
                mAddParameter(command, "@DISTRIBUTION_LIST_ID", email.distributionListId);

                command.CommandText = mStrSQL;

                mExecuteNoQuery(command);

                newId = mGetIdentity();


                mStrSQL = "INSERT INTO mmq.Email ( id , body, [subject] , [to] , cc , bcc ) VALUES (  " + newId + " , @BODY ,  @SUBJECT , @TO , @CC , @BCC  )";
                command.CommandText = mStrSQL;
                command.Parameters.Clear();

                mAddParameter(command, "@BODY", email.Body);
                mAddParameter(command, "@SUBJECT", email.Subject);
                mAddParameter(command, "@TO", email.To);
                mAddParameter(command, "@CC", email.Cc);
                mAddParameter(command, "@BCC", email.Bcc);

                mExecuteNoQuery(command);

                _log.info(newId, String.Format("Messaggio creato"));

                //ATTACHMENTS
                if (email.Attachments != null && email.Attachments.Count > 0)
                {
                    foreach (Attachment att in email.Attachments)
                    {
                        mStrSQL = "INSERT INTO mmq.Attachment ( message_id , date_added, name, stream ) VALUES (  " + newId + " , GetDate() ,  @NAME ,  @STREAM )";
                        command.CommandText = mStrSQL;
                        command.Parameters.Clear();

                        mAddParameter(command, "@NAME", att.name);
                        //   mAddParameter(command, "@STREAM", att.fileStream);

                        (command as System.Data.SqlClient.SqlCommand).Parameters.Add("@STREAM", SqlDbType.Image, att.fileStream.Length).Value = att.fileStream;
                        mExecuteNoQuery(command);


                        _log.info(newId, String.Format("Allegato: {0}", att.name));
                    }

                    _log.info(newId, String.Format("Inseriti {0:N0} allegati", email.Attachments.Count));
                }


                 mTransactionCommit();
            }
            catch (Exception ex)
            {
                mTransactionRollback();
                Debug.WriteLine("Exception: " + ex.Message);
                throw ex;
            }

            return newId;
        }


        public bool updateStatus(long messageId, MessageBase.EnumMessageStatus status, string errorMessage)
        {
            mStrSQL = "UPDATE mmq.Message SET message_status =  " + (int)status +
                ", DATE_TRANSMISSION = GetDate() " +
                ", ERROR_MESSAGE = @MESSAGE " +
                " WHERE ID=" + messageId;

            System.Data.Common.DbCommand command;
            command = mConnection.CreateCommand();
            command.CommandText = mStrSQL;

            if (String.IsNullOrEmpty(errorMessage))
            {
                mAddParameter(command, "@MESSAGE", DBNull.Value);
            }
            else
            {
                mAddParameter(command, "@MESSAGE", errorMessage.Trim());
            }

            mExecuteNoQuery(command);
            return true;
        }


        #region *** Attachment ***

        public Attachment getAttachment(long id)
        {
            mStrSQL = "SELECT * FROM mmq.Attachment WHERE id = " + id;
            mDt = mFillDataTable(mStrSQL);

            if (mDt.Rows.Count == 0)
            {
                return null;
            }

            if (mDt.Rows.Count > 1)
            {
                throw new MyManagerCSharp.MyException(MyManagerCSharp.MyException.ErrorNumber.Record_duplicato);
            }

            Attachment attachment;
            attachment = new Attachment(mDt.Rows[0]);

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

            mStrSQL = "SELECT t1.* FROM mmq.message as t1";

            System.Data.Common.DbCommand command;
            command = mConnection.CreateCommand();



            string strWHERE = "";
            //if (model.filter != null)
            //{
            //    if (!String.IsNullOrEmpty(model.filter.nome))
            //    {
            //        strWHERE += " AND UPPER(nome) like  @NOME";
            //        mAddParameter(command, "@NOME", "%" + model.filter.nome.ToUpper().Trim() + "%");
            //    }

            //    if (!String.IsNullOrEmpty(model.filter.cognome))
            //    {
            //        strWHERE += " AND UPPER(cognome) like  @COGNOME";
            //        mAddParameter(command, "@COGNOME", "%" + model.filter.cognome.ToUpper().Trim() + "%");
            //    }

            //    if (!String.IsNullOrEmpty(model.filter.email))
            //    {
            //        strWHERE += " AND UPPER(email) like  @EMAIL";
            //        mAddParameter(command, "@EMAIL", "%" + model.filter.email.ToUpper().Trim() + "%");
            //    }

            //    if (!String.IsNullOrEmpty(model.filter.login))
            //    {
            //        strWHERE += " AND UPPER(my_login) like  @MY_LOGIN";
            //        mAddParameter(command, "@MY_LOGIN", "%" + model.filter.login.ToUpper().Trim() + "%");
            //    }
            //}


            if (!String.IsNullOrEmpty(strWHERE))
            {
                mStrSQL += " WHERE (1=1) " + strWHERE;
            }

            string temp;
            int totalRecords;

            //paginazione
            if (model.PageSize > 0 && (mConnection is System.Data.SqlClient.SqlConnection))
            {
                temp = "SELECT COUNT(*) " + mStrSQL;
                command.CommandText = temp;

                totalRecords = int.Parse(mExecuteScalar(command));
                model.TotalRows = totalRecords;
            }



            temp = mStrSQL + " ORDER BY " + model.Sort + " " + model.SortDir;


            if (model.PageSize > 0 && (mConnection is System.Data.SqlClient.SqlConnection))
            {
                temp += " OFFSET " + ((model.PageNumber - 1) * model.PageSize) + " ROWS FETCH NEXT " + model.PageSize + " ROWS ONLY";
            }


            command.CommandText = temp;
            command.Connection = mConnection;
            mDt = mFillDataTable(command);

            if (model.PageSize > 0 && !(mConnection is System.Data.SqlClient.SqlConnection))
            {
                model.TotalRows = mDt.Rows.Count;

                // apply paging
                IEnumerable<DataRow> rows = mDt.AsEnumerable().Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize);
                foreach (DataRow row in rows)
                {
                    risultato.Add(new Models.MessageBase(row));
                }
            }
            else
            {
                foreach (DataRow row in mDt.Rows)
                {
                    risultato.Add(new Models.MessageBase(row));
                }
            }

            model.Messages = risultato;
        }


        public MessageBase getMessage(long id)
        {
            mStrSQL = "SELECT * FROM mmq.message WHERE id = " + id;
            mDt = mFillDataTable(mStrSQL);

            if (mDt.Rows.Count == 0)
            {
                return null;
            }

            if (mDt.Rows.Count > 1)
            {
                throw new MyManagerCSharp.MyException("id > 0");
            }

            MessageBase message;
            message = new MessageBase(mDt.Rows[0]);

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
            My.MessageQueue.MailManager manager = new MailManager(mConnection);
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
            My.MessageQueue.DistributionListManager manager = new DistributionListManager(mConnection);
            return manager.getList();
        }


        public DistributionList getDistributionList(long id)
        {
            My.MessageQueue.DistributionListManager manager = new DistributionListManager(mConnection);
            return manager.getDistributionList(id);
        }

        public bool send(Email email)
        {
            My.MessageQueue.MailManager manager = new MailManager(mConnection);
            return manager.send(email);
        }


        public List<Models.Log> getLogs(long messageId)
        {
            return _log.getLogs(messageId);
        }
    }
}

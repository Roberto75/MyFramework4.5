using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;


namespace MyManagerCSharp.Ticket
{
    public class TicketManager : ManagerDB
    {

        public enum TicketStatus
        {
            Undefined,
            Aperto,
            Assegnato,
            In_lavorazione,
            In_attesa,
            Chiuso
        }

        private string SQL_SELECT_UTENTI_SMALL = " u.my_login, u.nome , u.cognome, u.email, u.user_id ";

        public TicketManager(string connectionName)
            : base(connectionName)
        {

        }

        public TicketManager(System.Data.Common.DbConnection connection)
            : base(connection)
        {

        }


        public long getTicketTypeId(long ticketId)
        {
            _strSQL = "SELECT TICKET_TYPE_ID FROM TICKET where ID = " + ticketId;
            return long.Parse(_executeScalar(_strSQL));
        }


        public List<Models.MyTicket> getMyTicketList(long userId, TicketManager.TicketStatus? stato)
        {
            List<Models.MyTicket> risultato;
            risultato = new List<Models.MyTicket>();

            _strSQL = "select t.* , " + SQL_SELECT_UTENTI_SMALL +
                " from Ticket as t " +
                " join Utente as u on t.OWNER_ID = u.user_id ";

            _strSQL += " WHERE t.OWNER_ID = " + userId;
            if (stato != null && stato != TicketManager.TicketStatus.Undefined)

            {
                _strSQL += " AND t.TICKET_STATUS_ID = '" + stato.ToString() + "'";
            }

            _strSQL += " ORDER BY t.DATE_ADDED";

            _dt = _fillDataTable(_strSQL);

            Models.MyTicket t;
            foreach (System.Data.DataRow row in _dt.Rows)
            {
                t = new Models.MyTicket(row);

                t.Owner = new MyManagerCSharp.Models.MyUserSmall(row);

                risultato.Add(t);
            }

            return risultato;
        }

        public List<Models.MyTicket> getTicketList()
        {
            return getTicketList("", null);
        }

        public List<Models.MyTicket> getTicketList(string targetId, TicketManager.TicketStatus? stato)
        {
            List<Models.MyTicket> risultato;
            risultato = new List<Models.MyTicket>();

            System.Data.Common.DbCommand command;
            command = _connection.CreateCommand();

            _strSQL = "select t.* , " + SQL_SELECT_UTENTI_SMALL +
                " from Ticket as t " +
                " join Utente as u on t.OWNER_ID = u.user_id ";

            if (!String.IsNullOrEmpty(targetId))
            {
                _strSQL += " where t.target_id = @TARGET_ID";
                //Rel. 1.0.1.6 visibilià sui dati per gli utenti dello stesso gruppo di redazione
                _addParameter(command, "@TARGET_ID", targetId);
            }
       
            if (stato != null && stato != TicketManager.TicketStatus.Undefined)
            {
                _strSQL += " AND  t.TICKET_STATUS_ID = '" + stato.ToString() + "'";
            }
            _strSQL += " ORDER BY t.DATE_ADDED";

            command.CommandText = _strSQL;
            _dt = _fillDataTable(command );

            Models.MyTicket t;
            foreach (System.Data.DataRow row in _dt.Rows)
            {
                t = new Models.MyTicket(row);
                t.Owner = new MyManagerCSharp.Models.MyUserSmall(row);

                risultato.Add(t);
            }

            return risultato;
        }

        //Public Function existTicketOpen(ByVal userIdCliente As String, ByVal ticketType As Int16) As Long
        //    'verifico che per questo CLIENTE non ci sia già un ticket APERTO di questo TIPO
        //    'se non esiste vine restituito 0 
        //    Dim strSQL As String
        //    strSQL = "select ticket_id from Ticket where ticket_status_id = 'OPEN' and user_id_cliente = @user_id_cliente and ticket_type_id= @ticket_type_id"

        //    Dim command As System.Data.Common.DbCommand
        //    command = _connection.CreateCommand()
        //    command.CommandText = strSQL
        //    command.Connection = Me._connection

        //    Me._addParameter(command, "@user_id_cliente", userIdCliente)
        //    Me._addParameter(command, "@ticket_type_id", ticketType)

        //    Dim risultato As String
        //    risultato = Me._executeScalar(command)
        //    If String.IsNullOrEmpty(risultato) Then
        //        Return 0
        //    Else
        //        Return Long.Parse(risultato)
        //    End If

        //End Function


        public long insertNewTicket(long userId, string targetId, string note, Models.MyTicket ticket)
        {
            _strSQL = "INSERT INTO TICKET ( OWNER_ID, TARGET_ID, TITOLO,  TICKET_STATUS_ID , DATE_ADDED ,  DATE_LAST_MODIFIED ";

            string strSQLParametri = "";
            strSQLParametri = " VALUES ( @OWNER_ID , @TARGET_ID ,  @TITOLO , '" + TicketStatus.Aperto + "', GETDATE() , GETDATE() ";

            System.Data.Common.DbCommand command;
            command = _connection.CreateCommand();
            command.CommandText = _strSQL;
            try
            {
                _transactionBegin();

                _addParameter(command, "@OWNER_ID", userId);
                _addParameter(command, "@TARGET_ID", targetId);
                _addParameter(command, "@TITOLO", ticket.titolo);

                if (!String.IsNullOrEmpty(ticket.referenceTypeId))
                {
                    _strSQL += ",REFERENCE_TYPE_ID ";
                    strSQLParametri += ", @REFERENCE_TYPE_ID ";
                    _addParameter(command, "@REFERENCE_TYPE_ID", ticket.referenceTypeId);
                }

                if (ticket.referenceId != -1)
                {
                    _strSQL += ",REFERENCE_ID ";
                    strSQLParametri += ", @REFERENCE_ID ";
                    _addParameter(command, "@REFERENCE_ID", ticket.referenceId);
                }

                if (ticket.referenceSourceId != -1)
                {
                    _strSQL += ",REFERENCE_SOURCE_ID ";
                    strSQLParametri += ", @REFERENCE_SOURCE_ID ";
                    _addParameter(command, "@REFERENCE_SOURCE_ID", ticket.referenceSourceId);
                }


                if (!String.IsNullOrEmpty(ticket.referenceSource))
                {
                    _strSQL += ",REFERENCE_SOURCE ";
                    strSQLParametri += ", @REFERENCE_SOURCE ";
                    _addParameter(command, "@REFERENCE_SOURCE", ticket.referenceSource);
                }

                command.CommandText = _strSQL + " ) " + strSQLParametri + " )";

                long newTicketId;
                _executeNoQuery(command);
                newTicketId = _getIdentity();


                //aggiungo il testo ...
                _strSQL = "INSERT INTO TICKET_POST (DATE_ADDED , isFirstPost, USER_ID, TICKET_ID, NOTE )" +
                           " VALUES ( GETDATE(), 1, @USER_ID , @TICKET_ID , @NOTE )";

                command.Parameters.Clear();
                command.CommandText = _strSQL;
                _addParameter(command, "@USER_ID", userId);
                _addParameter(command, "@TICKET_ID", newTicketId);
                _addParameter(command, "@NOTE", note);

                _executeNoQuery(command);
                _transactionCommit();


                return newTicketId;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex.Message);
                _transactionRollback();
                throw ex;
            }
        }


        public bool deleteAttachment(long attachId, long ticketId, string absoluteServerPath)
        {
            string fileName;
            _strSQL = "SELECT FILE_NAME FROM TICKET_ATTACHMENT WHERE ID = " + attachId;

            fileName = _executeScalar(_strSQL);

            if (!String.IsNullOrEmpty(fileName) && !String.IsNullOrEmpty(absoluteServerPath))
            {
                string path;
                path = String.Format("{0}{1}\\{2}", absoluteServerPath, ticketId, fileName);

                System.IO.FileInfo f = new System.IO.FileInfo(path);
                if (f.Exists)
                {
                    f.Delete();
                }
            }

            _strSQL = "DELETE TICKET_ATTACHMENT  where ID = " + attachId;
            return _executeNoQuery(_strSQL) == 1;
        }


        public bool delete(long ticketId, string absoluteServerPath)
        {
            try
            {
                _transactionBegin();

                _strSQL = "DELETE TICKET_POST  where TICKET_ID = " + ticketId;
                _executeNoQuery(_strSQL);

                _strSQL = "DELETE TICKET_ATTACHMENT  where TICKET_ID = " + ticketId;
                _executeNoQuery(_strSQL);

                _strSQL = "DELETE TICKET  where ID = " + ticketId;
                int conta;
                conta = _executeNoQuery(_strSQL);

                _transactionCommit();


                if (!String.IsNullOrEmpty(absoluteServerPath))
                {
                    string path;
                    path = String.Format("{0}{1}\\", absoluteServerPath, ticketId);

                    System.IO.DirectoryInfo f = new System.IO.DirectoryInfo(path);
                    if (f.Exists)
                    {
                        f.Delete(true);
                    }
                }



                return conta == 1;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex.Message);
                _transactionRollback();
                return false;
            }

        }


        public long reply(long ticketId, long userId, string testo)
        {
            long newId;
            try
            {
                _transactionBegin();

                //operatore risponde ad un ticket già esistente ...
                _strSQL = "INSERT INTO TICKET_POST (DATE_ADDED ,  isFirstPost, USER_ID, TICKET_ID, NOTE )  VALUES ( GetDate() , 0, @USER_ID, @TICKET_ID, @NOTE ) ";

                System.Data.Common.DbCommand command;
                command = _connection.CreateCommand();
                command.CommandText = _strSQL;

                _addParameter(command, "@USER_ID", userId);
                _addParameter(command, "@TICKET_ID", ticketId);
                _addParameter(command, "@NOTE", testo);

                _executeNoQuery(command);

                newId = _getIdentity();

                _strSQL = "UPDATE TICKET SET  DATE_LAST_MODIFIED = GETDATE() WHERE id =  " + ticketId;
                _executeNoQuery(_strSQL);

                _transactionCommit();
            }
            catch (Exception ex)
            {
                _transactionRollback();
                Debug.WriteLine("Exception: " + ex.Message);
                return -1;
            }

            return newId;
        }



        public long close(long ticketId, long userId, string testo)
        {
            long newId;
            try
            {
                _transactionBegin();

                //operatore risponde ad un ticket già esistente ...
                _strSQL = "INSERT INTO TICKET_POST (DATE_ADDED ,  isFirstPost, USER_ID, TICKET_ID, NOTE )  VALUES ( GetDate() , 0, @USER_ID, @TICKET_ID, @NOTE ) ";

                System.Data.Common.DbCommand command;
                command = _connection.CreateCommand();
                command.CommandText = _strSQL;

                _addParameter(command, "@USER_ID", userId);
                _addParameter(command, "@TICKET_ID", ticketId);
                _addParameter(command, "@NOTE", testo);

                _executeNoQuery(command);

                newId = _getIdentity();

                _strSQL = "UPDATE TICKET SET TICKET_STATUS_ID = '" + TicketStatus.Chiuso.ToString() + "', DATE_LAST_MODIFIED = GETDATE() WHERE id =  " + ticketId;
                _executeNoQuery(_strSQL);

                _transactionCommit();
            }
            catch (Exception ex)
            {
                _transactionRollback();
                Debug.WriteLine("Exception: " + ex.Message);
                return -1;
            }

            return newId;
        }



        public Models.MyTicket getTicket(long ticketId)
        {
            //_strSQL = "SELECT * FROM TICKET where ID = " + ticketId;

            _strSQL = "select t.* ,  " + SQL_SELECT_UTENTI_SMALL +
               " from Ticket as t " +
               " join Utente as u on t.OWNER_ID = u.user_id " +
               " where ID = " + ticketId;


            _dt = _fillDataTable(_strSQL);

            if (_dt.Rows.Count == 0)
            {
                return null;
            }

            if (_dt.Rows.Count > 1)
            {
                throw new MyManagerCSharp.MyException("id > 0");
            }

            Models.MyTicket t;
            t = new Models.MyTicket(_dt.Rows[0]);

            t.Owner = new MyManagerCSharp.Models.MyUserSmall(_dt.Rows[0]);


            return t;
        }


        public List<Models.MyTicketPost> getPosts(long ticketId)
        {
            //_strSQL = "SELECT * FROM TICKET_POST WHERE TICKET_ID = " + ticketId + " ORDER BY DATE_ADDED";

            _strSQL = "SELECT t.* ,   " + SQL_SELECT_UTENTI_SMALL +
                        " FROM TICKET_POST as t " +
                        " join Utente as u on t.user_id = u.user_id " +
                        "WHERE TICKET_ID = " + ticketId + " ORDER BY DATE_ADDED";
            _dt = _fillDataTable(_strSQL);

            List<Models.MyTicketPost> risultato = new List<Models.MyTicketPost>();

            Models.MyTicketPost post;
            foreach (System.Data.DataRow row in _dt.Rows)
            {
                post = new Models.MyTicketPost(row);
                post.Owner = new MyManagerCSharp.Models.MyUserSmall(row);

                risultato.Add(post);
            }

            return risultato;
        }



        //Public Function getLastPost(ByVal ticketId As Long) As System.Data.DataTable
        //    Dim strSQL As String
        //    strSQL = "select ticket_post_id , max (date_added) " & _
        //                    " from ticket_post " & _
        //                    " where ticket_id = @TICKET_ID " & _
        //                    " group by ticket_post_id "

        //    Dim command As System.Data.Common.DbCommand
        //    command = _connection.CreateCommand()
        //    command.CommandText = strSQL
        //    command.Connection = Me._connection

        //    Me._addParameter(command, "@TICKET_ID", ticketId)

        //    Return Me._fillDataSet(command).Tables(0)
        //End Function


        //Public Function closeTicket(ByVal ticketId As Long) As Boolean

        //    Dim strSQL As String = "UPDATE TICKET SET DATE_CLOSED = GETDATE(), DATE_LAST_MODIFIED = GETDATE(), TICKET_STATUS_ID = 'CLOSED' WHERE TICKET_ID = @TICKET_ID "

        //    Dim command As System.Data.Common.DbCommand
        //    command = _connection.CreateCommand()
        //    command.CommandText = strSQL
        //    command.Connection = Me._connection


        //    Me._addParameter(command, "@TICKET_ID", ticketId)

        //    Me._executeNoQuery(command)

        //    Return True

        //End Function


        public long insertAttachment(string fileName, string description, long ticketId, long userId)
        {

            _strSQL = "INSERT INTO Ticket_Attachment ( DATE_ADDED, FILE_NAME, NOTE, USER_ID, TICKET_ID) " +
                       " VALUES ( GETDATE() , @FILENAME , @NOTE , @FK_USER_ID ,  @FK_EXTERNAL_ID   ) ";

            System.Data.Common.DbCommand command;
            command = _connection.CreateCommand();

            _addParameter(command, "@FILENAME", fileName);
            _addParameter(command, "@NOTE", description);
            _addParameter(command, "@FK_USER_ID", userId);
            _addParameter(command, "@FK_EXTERNAL_ID", ticketId);

            command.CommandText = _strSQL;

            _executeNoQuery(command);

            return _getIdentity();
        }


        public List<Models.MyTicketAttachment> getAttachments(long ticketId)
        {
            // _strSQL = "SELECT * FROM TICKET_ATTACHMENT WHERE TICKET_ID = " + ticketId + " ORDER BY FILE_NAME";
            _strSQL = "SELECT t.* ,  " + SQL_SELECT_UTENTI_SMALL +
                   " FROM TICKET_ATTACHMENT as t " +
                   " join Utente as u on t.user_id = u.user_id " +
                   "WHERE TICKET_ID = " + ticketId + " ORDER BY FILE_NAME";

            _dt = _fillDataTable(_strSQL);

            List<Models.MyTicketAttachment> risultato = new List<Models.MyTicketAttachment>();

            Models.MyTicketAttachment attach;
            foreach (System.Data.DataRow row in _dt.Rows)
            {
                attach = new Models.MyTicketAttachment(row);
                attach.Owner = new MyManagerCSharp.Models.MyUserSmall(row);

                risultato.Add(attach);
            }

            return risultato;
        }


        public Models.MyTicketAttachment getAttachment(long attachId)
        {
            _strSQL = "SELECT * FROM TICKET_ATTACHMENT WHERE ID = " + attachId;
            _dt = _fillDataTable(_strSQL);

            if (_dt.Rows.Count == 0)
            {
                return null;
            }

            if (_dt.Rows.Count > 1)
            {
                throw new MyManagerCSharp.MyException("id > 0");
            }

            Models.MyTicketAttachment attach;
            attach = new Models.MyTicketAttachment(_dt.Rows[0]);
            return attach;
        }


        public List<MyManagerCSharp.Models.MyUserSmall> getUtentiInTicket(long ticketId, long utenteCorrente)
        {
            _strSQL = "SELECT DISTINCT " + SQL_SELECT_UTENTI_SMALL +
                       " FROM TICKET_POST as t " +
                       " join Utente as u on t.user_id = u.user_id " +
                       " WHERE TICKET_ID = " + ticketId +
                       " AND u.user_id <> " + utenteCorrente +
                       " ORDER BY  U.MY_LOGIN";

            _dt = _fillDataTable(_strSQL);

            List<MyManagerCSharp.Models.MyUserSmall> risultato = new List<MyManagerCSharp.Models.MyUserSmall>();

            MyManagerCSharp.Models.MyUserSmall userSmall;
            foreach (System.Data.DataRow row in _dt.Rows)
            {
                userSmall = new MyManagerCSharp.Models.MyUserSmall(row);

                risultato.Add(userSmall);
            }

            return risultato;

        }

    }
}

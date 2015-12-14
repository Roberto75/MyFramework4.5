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
            mStrSQL = "SELECT TICKET_TYPE_ID FROM TICKET where ID = " + ticketId;
            return long.Parse(mExecuteScalar(mStrSQL));
        }


        public List<Models.MyTicket> getMyTicketList(long userId, TicketManager.TicketStatus? stato)
        {
            List<Models.MyTicket> risultato;
            risultato = new List<Models.MyTicket>();

            mStrSQL = "select t.* , " + SQL_SELECT_UTENTI_SMALL +
                " from Ticket as t " +
                " join Utente as u on t.OWNER_ID = u.user_id ";

            mStrSQL += " WHERE t.OWNER_ID = " + userId;
            if (stato != null && stato != TicketManager.TicketStatus.Undefined)

            {
                mStrSQL += " AND t.TICKET_STATUS_ID = '" + stato.ToString() + "'";
            }

            mStrSQL += " ORDER BY t.DATE_ADDED";

            mDt = mFillDataTable(mStrSQL);

            Models.MyTicket t;
            foreach (System.Data.DataRow row in mDt.Rows)
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
            command =  mConnection.CreateCommand();

            mStrSQL = "select t.* , " + SQL_SELECT_UTENTI_SMALL +
                " from Ticket as t " +
                " join Utente as u on t.OWNER_ID = u.user_id ";

            if (!String.IsNullOrEmpty(targetId))
            {
                mStrSQL += " where t.target_id = @TARGET_ID";
                //Rel. 1.0.1.6 visibilià sui dati per gli utenti dello stesso gruppo di redazione
                mAddParameter(command, "@TARGET_ID", targetId);
            }
       
            if (stato != null && stato != TicketManager.TicketStatus.Undefined)
            {
                mStrSQL += " AND  t.TICKET_STATUS_ID = '" + stato.ToString() + "'";
            }
            mStrSQL += " ORDER BY t.DATE_ADDED";

            command.CommandText = mStrSQL;
            mDt = mFillDataTable(command );

            Models.MyTicket t;
            foreach (System.Data.DataRow row in mDt.Rows)
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
        //    command = mConnection.CreateCommand()
        //    command.CommandText = strSQL
        //    command.Connection = Me.mConnection

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
            mStrSQL = "INSERT INTO TICKET ( OWNER_ID, TARGET_ID, TITOLO,  TICKET_STATUS_ID , DATE_ADDED ,  DATE_LAST_MODIFIED ";

            string strSQLParametri = "";
            strSQLParametri = " VALUES ( @OWNER_ID , @TARGET_ID ,  @TITOLO , '" + TicketStatus.Aperto + "', GETDATE() , GETDATE() ";

            System.Data.Common.DbCommand command;
            command =  mConnection.CreateCommand();
            command.CommandText = mStrSQL;
            try
            {
                mTransactionBegin();

                mAddParameter(command, "@OWNER_ID", userId);
                mAddParameter(command, "@TARGET_ID", targetId);
                mAddParameter(command, "@TITOLO", ticket.titolo);

                if (!String.IsNullOrEmpty(ticket.referenceTypeId))
                {
                    mStrSQL += ",REFERENCE_TYPE_ID ";
                    strSQLParametri += ", @REFERENCE_TYPE_ID ";
                    mAddParameter(command, "@REFERENCE_TYPE_ID", ticket.referenceTypeId);
                }

                if (ticket.referenceId != -1)
                {
                    mStrSQL += ",REFERENCE_ID ";
                    strSQLParametri += ", @REFERENCE_ID ";
                    mAddParameter(command, "@REFERENCE_ID", ticket.referenceId);
                }

                if (ticket.referenceSourceId != -1)
                {
                    mStrSQL += ",REFERENCE_SOURCE_ID ";
                    strSQLParametri += ", @REFERENCE_SOURCE_ID ";
                    mAddParameter(command, "@REFERENCE_SOURCE_ID", ticket.referenceSourceId);
                }


                if (!String.IsNullOrEmpty(ticket.referenceSource))
                {
                    mStrSQL += ",REFERENCE_SOURCE ";
                    strSQLParametri += ", @REFERENCE_SOURCE ";
                    mAddParameter(command, "@REFERENCE_SOURCE", ticket.referenceSource);
                }

                command.CommandText = mStrSQL + " ) " + strSQLParametri + " )";

                long newTicketId;
                mExecuteNoQuery(command);
                newTicketId = mGetIdentity();


                //aggiungo il testo ...
                mStrSQL = "INSERT INTO TICKET_POST (DATE_ADDED , isFirstPost, USER_ID, TICKET_ID, NOTE )" +
                           " VALUES ( GETDATE(), 1, @USER_ID , @TICKET_ID , @NOTE )";

                command.Parameters.Clear();
                command.CommandText = mStrSQL;
                mAddParameter(command, "@USER_ID", userId);
                mAddParameter(command, "@TICKET_ID", newTicketId);
                mAddParameter(command, "@NOTE", note);

                mExecuteNoQuery(command);
                mTransactionCommit();


                return newTicketId;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex.Message);
                mTransactionRollback();
                throw ex;
            }
        }


        public bool deleteAttachment(long attachId, long ticketId, string absoluteServerPath)
        {
            string fileName;
            mStrSQL = "SELECT FILE_NAME FROM TICKET_ATTACHMENT WHERE ID = " + attachId;

            fileName = mExecuteScalar(mStrSQL);

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

            mStrSQL = "DELETE TICKET_ATTACHMENT  where ID = " + attachId;
            return mExecuteNoQuery(mStrSQL) == 1;
        }


        public bool delete(long ticketId, string absoluteServerPath)
        {
            try
            {
                mTransactionBegin();

                mStrSQL = "DELETE TICKET_POST  where TICKET_ID = " + ticketId;
                mExecuteNoQuery(mStrSQL);

                mStrSQL = "DELETE TICKET_ATTACHMENT  where TICKET_ID = " + ticketId;
                mExecuteNoQuery(mStrSQL);

                mStrSQL = "DELETE TICKET  where ID = " + ticketId;
                int conta;
                conta = mExecuteNoQuery(mStrSQL);

                 mTransactionCommit();


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
                mTransactionRollback();
                return false;
            }

        }


        public long reply(long ticketId, long userId, string testo)
        {
            long newId;
            try
            {
                 mTransactionBegin();

                //operatore risponde ad un ticket già esistente ...
                mStrSQL = "INSERT INTO TICKET_POST (DATE_ADDED ,  isFirstPost, USER_ID, TICKET_ID, NOTE )  VALUES ( GetDate() , 0, @USER_ID, @TICKET_ID, @NOTE ) ";

                System.Data.Common.DbCommand command;
                command =  mConnection.CreateCommand();
                command.CommandText = mStrSQL;

                mAddParameter(command, "@USER_ID", userId);
                mAddParameter(command, "@TICKET_ID", ticketId);
                mAddParameter(command, "@NOTE", testo);

                mExecuteNoQuery(command);

                newId = mGetIdentity();

                mStrSQL = "UPDATE TICKET SET  DATE_LAST_MODIFIED = GETDATE() WHERE id =  " + ticketId;
                mExecuteNoQuery(mStrSQL);

                 mTransactionCommit();
            }
            catch (Exception ex)
            {
                mTransactionRollback();
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
                 mTransactionBegin();

                //operatore risponde ad un ticket già esistente ...
                mStrSQL = "INSERT INTO TICKET_POST (DATE_ADDED ,  isFirstPost, USER_ID, TICKET_ID, NOTE )  VALUES ( GetDate() , 0, @USER_ID, @TICKET_ID, @NOTE ) ";

                System.Data.Common.DbCommand command;
                command =  mConnection.CreateCommand();
                command.CommandText = mStrSQL;

                mAddParameter(command, "@USER_ID", userId);
                mAddParameter(command, "@TICKET_ID", ticketId);
                mAddParameter(command, "@NOTE", testo);

                mExecuteNoQuery(command);

                newId = mGetIdentity();

                mStrSQL = "UPDATE TICKET SET TICKET_STATUS_ID = '" + TicketStatus.Chiuso.ToString() + "', DATE_LAST_MODIFIED = GETDATE() WHERE id =  " + ticketId;
                mExecuteNoQuery(mStrSQL);

                 mTransactionCommit();
            }
            catch (Exception ex)
            {
                mTransactionRollback();
                Debug.WriteLine("Exception: " + ex.Message);
                return -1;
            }

            return newId;
        }



        public Models.MyTicket getTicket(long ticketId)
        {
            // mStrSQL = "SELECT * FROM TICKET where ID = " + ticketId;

            mStrSQL = "select t.* ,  " + SQL_SELECT_UTENTI_SMALL +
               " from Ticket as t " +
               " join Utente as u on t.OWNER_ID = u.user_id " +
               " where ID = " + ticketId;


            mDt = mFillDataTable(mStrSQL);

            if (mDt.Rows.Count == 0)
            {
                return null;
            }

            if (mDt.Rows.Count > 1)
            {
                throw new MyManagerCSharp.MyException("id > 0");
            }

            Models.MyTicket t;
            t = new Models.MyTicket(mDt.Rows[0]);

            t.Owner = new MyManagerCSharp.Models.MyUserSmall(mDt.Rows[0]);


            return t;
        }


        public List<Models.MyTicketPost> getPosts(long ticketId)
        {
            // mStrSQL = "SELECT * FROM TICKET_POST WHERE TICKET_ID = " + ticketId + " ORDER BY DATE_ADDED";

            mStrSQL = "SELECT t.* ,   " + SQL_SELECT_UTENTI_SMALL +
                        " FROM TICKET_POST as t " +
                        " join Utente as u on t.user_id = u.user_id " +
                        "WHERE TICKET_ID = " + ticketId + " ORDER BY DATE_ADDED";
            mDt = mFillDataTable(mStrSQL);

            List<Models.MyTicketPost> risultato = new List<Models.MyTicketPost>();

            Models.MyTicketPost post;
            foreach (System.Data.DataRow row in mDt.Rows)
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
        //    command = mConnection.CreateCommand()
        //    command.CommandText = strSQL
        //    command.Connection = Me.mConnection

        //    Me._addParameter(command, "@TICKET_ID", ticketId)

        //    Return Me._fillDataSet(command).Tables(0)
        //End Function


        //Public Function closeTicket(ByVal ticketId As Long) As Boolean

        //    Dim strSQL As String = "UPDATE TICKET SET DATE_CLOSED = GETDATE(), DATE_LAST_MODIFIED = GETDATE(), TICKET_STATUS_ID = 'CLOSED' WHERE TICKET_ID = @TICKET_ID "

        //    Dim command As System.Data.Common.DbCommand
        //    command = mConnection.CreateCommand()
        //    command.CommandText = strSQL
        //    command.Connection = Me.mConnection


        //    Me._addParameter(command, "@TICKET_ID", ticketId)

        //    Me._executeNoQuery(command)

        //    Return True

        //End Function


        public long insertAttachment(string fileName, string description, long ticketId, long userId)
        {

            mStrSQL = "INSERT INTO Ticket_Attachment ( DATE_ADDED, FILE_NAME, NOTE, USER_ID, TICKET_ID) " +
                       " VALUES ( GETDATE() , @FILENAME , @NOTE , @FK_USER_ID ,  @FK_EXTERNAL_ID   ) ";

            System.Data.Common.DbCommand command;
            command =  mConnection.CreateCommand();

            mAddParameter(command, "@FILENAME", fileName);
            mAddParameter(command, "@NOTE", description);
            mAddParameter(command, "@FK_USER_ID", userId);
            mAddParameter(command, "@FK_EXTERNAL_ID", ticketId);

            command.CommandText = mStrSQL;

            mExecuteNoQuery(command);

            return mGetIdentity();
        }


        public List<Models.MyTicketAttachment> getAttachments(long ticketId)
        {
            //  mStrSQL = "SELECT * FROM TICKET_ATTACHMENT WHERE TICKET_ID = " + ticketId + " ORDER BY FILE_NAME";
            mStrSQL = "SELECT t.* ,  " + SQL_SELECT_UTENTI_SMALL +
                   " FROM TICKET_ATTACHMENT as t " +
                   " join Utente as u on t.user_id = u.user_id " +
                   "WHERE TICKET_ID = " + ticketId + " ORDER BY FILE_NAME";

            mDt = mFillDataTable(mStrSQL);

            List<Models.MyTicketAttachment> risultato = new List<Models.MyTicketAttachment>();

            Models.MyTicketAttachment attach;
            foreach (System.Data.DataRow row in mDt.Rows)
            {
                attach = new Models.MyTicketAttachment(row);
                attach.Owner = new MyManagerCSharp.Models.MyUserSmall(row);

                risultato.Add(attach);
            }

            return risultato;
        }


        public Models.MyTicketAttachment getAttachment(long attachId)
        {
            mStrSQL = "SELECT * FROM TICKET_ATTACHMENT WHERE ID = " + attachId;
            mDt = mFillDataTable(mStrSQL);

            if (mDt.Rows.Count == 0)
            {
                return null;
            }

            if (mDt.Rows.Count > 1)
            {
                throw new MyManagerCSharp.MyException("id > 0");
            }

            Models.MyTicketAttachment attach;
            attach = new Models.MyTicketAttachment(mDt.Rows[0]);
            return attach;
        }


        public List<MyManagerCSharp.Models.MyUserSmall> getUtentiInTicket(long ticketId, long utenteCorrente)
        {
            mStrSQL = "SELECT DISTINCT " + SQL_SELECT_UTENTI_SMALL +
                       " FROM TICKET_POST as t " +
                       " join Utente as u on t.user_id = u.user_id " +
                       " WHERE TICKET_ID = " + ticketId +
                       " AND u.user_id <> " + utenteCorrente +
                       " ORDER BY  U.MY_LOGIN";

            mDt = mFillDataTable(mStrSQL);

            List<MyManagerCSharp.Models.MyUserSmall> risultato = new List<MyManagerCSharp.Models.MyUserSmall>();

            MyManagerCSharp.Models.MyUserSmall userSmall;
            foreach (System.Data.DataRow row in mDt.Rows)
            {
                userSmall = new MyManagerCSharp.Models.MyUserSmall(row);

                risultato.Add(userSmall);
            }

            return risultato;

        }

    }
}

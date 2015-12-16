//Direttive di compilazione per le librerire esterne
#define Oracle

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace My.Shared.db
{

    public class OracleManager
    {

#if Oracle
        protected Oracle.DataAccess.Client.OracleConnection mConnection;
        protected Oracle.DataAccess.Client.OracleTransaction mTransaction;

        protected string mConnectionName;

        protected string mStrSQL;

        protected DataTable mDataTable;

        #region ***Connessione**

        public OracleManager(string connectionName)
        {
            mConnectionName = connectionName;

            mConnection = new Oracle.DataAccess.Client.OracleConnection();
            mConnection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
        }

        public OracleManager(Oracle.DataAccess.Client.OracleConnection connection)
        {

            mConnection = connection;
        }

        public Oracle.DataAccess.Client.OracleConnection getConnection()
        {
            return mConnection;
        }

        public void openConnection()
        {
            if (mConnection == null)
            {
                throw new ApplicationException("Connection non inizializzata");
            }


            if (mConnection.State != System.Data.ConnectionState.Open)
            {
                mConnection.Open();
            }
        }

        public void closeConnection()
        {
            if (mConnection == null)
            {
                throw new ApplicationException("Connection non inizializzata");
            }


            mConnection.Close();
            mConnection.Dispose();
        }


        #endregion


        #region ***Transazione**

        public void transactionBegin(ref Oracle.DataAccess.Client.OracleTransaction transaction)
        {
            if (transaction == null)
            {
                throw new ApplicationException("Transazione NON inizializzata");
            }
            mTransaction = transaction;
        }

        public void transactionBegin()
        {
            if (mTransaction != null)
            {
                throw new ApplicationException("Transazione già aperta");
            }
            mTransaction = mConnection.BeginTransaction();
        }

        public void transactionCommit()
        {
            if (mTransaction == null)
            {
                throw new ApplicationException("Transazione NON inizializzata");
            }
            mTransaction.Commit();
            mTransaction.Dispose();
            mTransaction = null;
        }

        public void transactionRollback()
        {
            if (mTransaction == null)
            {
                throw new ApplicationException("Transazione NON inizializzata");

            }
            mTransaction.Rollback();
            mTransaction.Dispose();
            mTransaction = null;
        }


        public Oracle.DataAccess.Client.OracleTransaction getTransaction()
        {
            //'26/01/2012 commento
            //' If _transaction Is Nothing Then
            //' Throw New ManagerException("Transazione NON inizializzata")
            //'xit Function
            //'  End If
            return mTransaction;
        }

        #endregion



        protected long getSequence(string sequenceName)
        {
            string strSQL = "select " + sequenceName + ".nextval from dual";
            Oracle.DataAccess.Client.OracleCommand command = new Oracle.DataAccess.Client.OracleCommand(strSQL, mConnection);

            return Convert.ToInt64(command.ExecuteScalar());
        }



        protected int mExecuteNonQuery(string strSQL)
        {
            Oracle.DataAccess.Client.OracleCommand command = new Oracle.DataAccess.Client.OracleCommand(strSQL, mConnection);
            return mExecuteNonQuery(command);
        }

        protected int mExecuteNonQuery(Oracle.DataAccess.Client.OracleCommand command)
        {
            //if (_transaction != null)
            //{
            //    command.Transaction = _transaction;
            //}


            return command.ExecuteNonQuery();
        }

#endif

    }







}

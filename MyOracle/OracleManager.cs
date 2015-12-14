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
        protected Oracle.DataAccess.Client.OracleConnection _connection;
        protected Oracle.DataAccess.Client.OracleTransaction _transaction;

        protected string _connectionName;

        protected string _strSQL;

        protected DataTable _dt;

        #region ***Connessione**

        public OracleManager(string connectionName)
        {
            _connectionName = connectionName;

            _connection = new Oracle.DataAccess.Client.OracleConnection();
            _connection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
        }

        public OracleManager(Oracle.DataAccess.Client.OracleConnection connection)
        {

            _connection = connection;
        }

        public Oracle.DataAccess.Client.OracleConnection getConnection()
        {
            return _connection;
        }

        public void openConnection()
        {
            if (_connection == null)
            {
                throw new ApplicationException("Connection non inizializzata");
            }


            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }
        }


        public void closeConnection()
        {
            if (_connection == null)
            {
                throw new ApplicationException("Connection non inizializzata");
            }


            _connection.Close();
            _connection.Dispose();
        }


        #endregion


        #region ***Transazione**

        public void transactionBegin(ref Oracle.DataAccess.Client.OracleTransaction transaction)
        {
            if (transaction == null)
            {
                throw new ApplicationException("Transazione NON inizializzata");
            }
            _transaction = transaction;
        }

        public void transactionBegin()
        {
            if (_transaction != null)
            {
                throw new ApplicationException("Transazione già aperta");
            }
            _transaction = _connection.BeginTransaction();
        }

        public void transactionCommit()
        {
            if (_transaction == null)
            {
                throw new ApplicationException("Transazione NON inizializzata");
            }
            _transaction.Commit();
            _transaction.Dispose();
            _transaction = null;
        }

        public void transactionRollback()
        {
            if (_transaction == null)
            {
                throw new ApplicationException("Transazione NON inizializzata");

            }
            _transaction.Rollback();
            _transaction.Dispose();
            _transaction = null;
        }


        public Oracle.DataAccess.Client.OracleTransaction getTransaction()
        {
            //'26/01/2012 commento
            //' If _transaction Is Nothing Then
            //' Throw New ManagerException("Transazione NON inizializzata")
            //'xit Function
            //'  End If
            return _transaction;
        }

        #endregion



        protected long getSequence(string sequenceName)
        {
            string strSQL = "select " + sequenceName + ".nextval from dual";
            Oracle.DataAccess.Client.OracleCommand command = new Oracle.DataAccess.Client.OracleCommand(strSQL, _connection);

            return Convert.ToInt64(command.ExecuteScalar());
        }



        protected int ExecuteNonQuery(string strSQL)
        {
            Oracle.DataAccess.Client.OracleCommand command = new Oracle.DataAccess.Client.OracleCommand(strSQL, _connection);
            return ExecuteNonQuery(command);
        }

        protected int ExecuteNonQuery(Oracle.DataAccess.Client.OracleCommand command)
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

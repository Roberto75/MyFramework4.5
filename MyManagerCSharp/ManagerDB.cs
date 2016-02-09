//Direttive di compilazione per le librerire esterne

//#define MySQL
//#define Oracle
//#Const SqlServerCe = False
//#Const PostgreSQL = False
//#Const Oracle = False

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Diagnostics;


namespace MyManagerCSharp
{

    public class ManagerDB : Manager
    {

        public enum Days
        {
            Tutti = 0,
            Oggi,
            Ieri,
            Settimana_corrente,
            Settimana_precedente,
            Mese_corrente,
            Mese_precedente,
            Ultimi_7_giorni,
            Ultimi_15_giorni,
            Ultimi_30_giorni,
            Primo_semestre_anno_corrente,
            Primo_semestre_anno_precedente,
            Secondo_semestre_anno_corrente,
            Secondo_semestre_anno_precedente,
            Ultimo_semestre,
            Anno_corrente,
            Anno_precedente
        }

        public enum SortDirection
        {
            Ascending,
            Descending
        }

        protected System.Data.Common.DbConnection mConnection;
        protected System.Data.Common.DbProviderFactory mFactory;
        protected System.Data.Common.DbTransaction mTransaction;
        protected string mConnectionName;
        protected string mStrSQL;
        protected DataTable mDt;

        private string _provider;


        public ManagerDB(string connectionName)
        {
            //In questo costruttore viene passata la connessione da utilizzare 
            _provider = System.Configuration.ConfigurationManager.ConnectionStrings[connectionName].ProviderName;
            mConnectionName = connectionName;

            // Creazione dell'oggetto factory
            mFactory = System.Data.Common.DbProviderFactories.GetFactory(_provider);

            mConnection = mFactory.CreateConnection();
            mConnection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
        }

        public ManagerDB(System.Data.Common.DbConnection connection)
        {
            //In questo costruttore viene passata la connessione da utilizzare 
            mConnection = connection;

            if (mConnection == null)
            {
                return;
            }

            switch (mConnection.GetType().Name)
            {
                case "OdbcConnection":
                    _provider = "System.Data.Odbc";
                    break;
                case "OleDbConnection":
                    _provider = "System.Data.OleDb";
                    break;
                case "SqlConnection":
                    _provider = "System.Data.SqlClient";
                    break;
                case "MySqlConnection":
                    _provider = "MySql.Data.MySqlClient";
                    break;
                case "SqlCeConnection":
                    _provider = "System.Data.SqlServerCe.3.5";
                    break;
                case "NpgsqlConnection":
                    _provider = "Npgsql";
                    break;
                default:
                    throw new MyException("Il costruttore non gestisce  questo tipo di connessione: " + mConnection.GetType().Name);
            }

            mFactory = System.Data.Common.DbProviderFactories.GetFactory(_provider);
        }

        public System.Data.Common.DbConnection mGetConnection()
        {
            return mConnection;
        }

        public void mChangeConnectionString(string connectionString)
        {
            mConnection.ConnectionString = connectionString;
        }

        public void mOpenConnection()
        {
            if (String.IsNullOrEmpty(mConnection.ConnectionString))
            {
                Debug.WriteLine("ConnectionString IS NULL");
                if (String.IsNullOrEmpty(mConnectionName))
                {
                    throw new ApplicationException("ConnectionString non inizializzata");
                }
                else
                {
                    mConnection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings[mConnectionName].ConnectionString;
                }
            }

            if (mConnection.State != System.Data.ConnectionState.Open)
            {
                mConnection.Open();
            }
        }

        public void mCloseConnection()
        {
            mConnection.Close();
            mConnection.Dispose();
        }

        public DataTable mFillDataTable(System.Data.Common.DbCommand command)
        {
            DataSet dataSet;
            dataSet = new DataSet();

            mFillDataSet(command, dataSet, null, -1);

            return dataSet.Tables[0];
        }

        public DataTable mFillDataTable(string sql)
        {
            return mFillDataTable(sql, -1);
        }

        public DataTable mFillDataTable(string sql, int timeOut)
        {
            DataSet dataSet;
            dataSet = new DataSet();

            mFillDataSet(sql, dataSet, null, timeOut);

            return dataSet.Tables[0];
        }

        protected DataSet mFillDataSet(string sqlQuery, string dataSetName, string tableName, int timeOut)
        {
            DataSet dataSet;

            if (string.IsNullOrEmpty(dataSetName))
            {
                dataSet = new DataSet();
            }
            else
            {
                dataSet = new DataSet(dataSetName);
            }

            mFillDataSet(sqlQuery, dataSet, tableName, timeOut);

            return dataSet;
        }

        protected void mFillDataSet(string sqlQuery, DataSet ds, string tableName, int timeOut)
        {
            System.Data.Common.DbCommand command;

            command = mConnection.CreateCommand();
            command.Connection = mConnection;
            command.CommandText = sqlQuery;

            mFillDataSet(command, ds, tableName, timeOut);
        }

        protected void mFillDataSet(System.Data.Common.DbCommand command, DataSet ds, string tableName, int timeOut)
        {
            if (mTransaction != null)
            {
                command.Transaction = mTransaction;
            }

            if (mConnection.GetType().Name == "OleDbConnection" || mConnection.GetType().Name == "OdbcConnection")
            {
                //'Per ACCESS e PostgreSQL ...
                command.CommandText = mParseSQLforAccessAndPostgreSQL(command.CommandText);
            }
            else if (mConnection.GetType().Name == "MySqlConnection")
            {
                command.CommandText = mParseSQLforMySQL(command.CommandText);
            }
            else if (mConnection.GetType().Name == "OracleConnection")
            {
                command.CommandText = mParseSQLforOracle(command.CommandText);
            }

            System.Data.Common.DataAdapter objAdap = null;

            try
            {

                //'MODIFICA PER GESTIRE IL CASO DEL COSTRUTTORE con la CONNESSIONE.
                //'in questo caso non ho il FACTORY!!!

                //'Select Case objAdap.GetType().Name
                //'    Case "SqlDataAdapter"
                //'        DirectCast(objAdap, System.Data.SqlClient.SqlDataAdapter).SelectCommand = DirectCast(command, System.Data.SqlClient.SqlCommand)
                //'    Case "OleDbDataAdapter"
                //'        DirectCast(objAdap, System.Data.OleDb.OleDbDataAdapter).SelectCommand = DirectCast(command, System.Data.OleDb.OleDbCommand)
                //'    Case Else
                //'        Throw New ManagerException("Tipo di Adapter non supportato: " & objAdap.GetType().Name)
                //'End Select

                if (timeOut != -1)
                {
                    command.CommandTimeout = timeOut;
                }

                objAdap = _getDataAdapter(command);



                if (string.IsNullOrEmpty(tableName))
                {
                    objAdap.Fill(ds);
                }
                else
                {
                    switch (objAdap.GetType().Name)
                    {
#if MySQL
                        case "MySqlDataAdapter":
                            (objAdap as MySql.Data.MySqlClient.MySqlDataAdapter).Fill(ds, tableName);
                            break;
#endif
                        case "SqlDataAdapter":
                            (objAdap as System.Data.SqlClient.SqlDataAdapter).Fill(ds, tableName);
                            break;
                        case "OleDbDataAdapter":
                            (objAdap as System.Data.OleDb.OleDbDataAdapter).Fill(ds, tableName);
                            break;
#if Oracle
                        case "OracleDataAdapter":
                            (objAdap as Oracle.DataAccess.Client.OracleDataAdapter).Fill(ds, tableName);
                            break;
#endif
                        case "OdbcDataAdapter":
                            (objAdap as System.Data.Odbc.OdbcDataAdapter).Fill(ds, tableName);
                            break;
                        default:
                            throw new MyException("Tipo di Adapter non supportato: " + objAdap.GetType().Name);
                    }
                }

            }
            finally
            {

                if (objAdap != null)
                {
                    objAdap.Dispose();
                    objAdap = null;
                }

                if (command != null)
                {
                    command.Dispose();
                    command = null;
                }
            }
        }

        private System.Data.Common.DataAdapter _getDataAdapter(System.Data.Common.DbCommand command)
        {
            System.Data.Common.DataAdapter objAdap = null;

            switch (mConnection.GetType().Name)
            {
#if MySQL
                case "MySqlConnection":
                    objAdap = new MySql.Data.MySqlClient.MySqlDataAdapter();
                    (objAdap as MySql.Data.MySqlClient.MySqlDataAdapter).SelectCommand = (command as MySql.Data.MySqlClient.MySqlCommand);
                    break;
#endif
                case "SqlConnection":
                    objAdap = new System.Data.SqlClient.SqlDataAdapter();
                    (objAdap as System.Data.SqlClient.SqlDataAdapter).SelectCommand = (command as System.Data.SqlClient.SqlCommand);
                    break;

#if SqlServerCe 
            Case "SqlCeConnection"
                objAdap = New System.Data.SqlServerCe.SqlCeDataAdapter
                DirectCast(objAdap, System.Data.SqlServerCe.SqlCeDataAdapter).SelectCommand = DirectCast(command, System.Data.SqlServerCe.SqlCeCommand)
                break;
#endif
                case "OleDbConnection":
                    objAdap = new System.Data.OleDb.OleDbDataAdapter();
                    (objAdap as System.Data.OleDb.OleDbDataAdapter).SelectCommand = (command as System.Data.OleDb.OleDbCommand);
                    break;
#if Oracle
                case "OracleConnection":
                    objAdap = new Oracle.DataAccess.Client.OracleDataAdapter();
                    (objAdap as Oracle.DataAccess.Client.OracleDataAdapter).SelectCommand = (command as Oracle.DataAccess.Client.OracleCommand);
                    break;
#endif
                case "OdbcConnection":
                    objAdap = new System.Data.Odbc.OdbcDataAdapter();
                    (objAdap as System.Data.Odbc.OdbcDataAdapter).SelectCommand = (command as System.Data.Odbc.OdbcCommand);
                    break;

#if PostgreSQL 
            Case "NpgsqlConnection"
                objAdap = New Npgsql.NpgsqlDataAdapter
                DirectCast(objAdap, Npgsql.NpgsqlDataAdapter).SelectCommand = DirectCast(command, Npgsql.NpgsqlCommand)
#endif
                default:
                    throw new MyException("Tipo di connessione non riconosciuta: " + mConnection.GetType().Name);
            }

            return objAdap;
        }

        protected string mParseSQLforOracle(string strSQL)
        {
            strSQL = strSQL.Replace("GetDate()", "Sysdate");

            strSQL = strSQL.Replace(" @", " :");

            return strSQL;
        }

        protected string mParseSQLforMySQL(string strSQL)
        {
            strSQL = strSQL.Replace("GetDate()", "Now()");

            strSQL = strSQL.Replace(" UPPER(", " UCASE(");

            ////20/02/2014
            //strSQL = strSQL.Replace("'%", "'*");
            //strSQL = strSQL.Replace("%'", "*'");

            //12/09/2014 per MySQL
            strSQL = strSQL.Replace(" ISNULL(", " IFNULL(");

            return strSQL;
        }

        protected string mParseSQLforAccessAndPostgreSQL(string strSQL)
        {
            //analizzo la stringa SQL per renderla compatibile con ACCESS e PostgreSQL
            //sostituisco @PARAMETRO con ?

            System.Text.RegularExpressions.Regex pattern = new System.Text.RegularExpressions.Regex("@(([a-z]|[A-Z]|[0-9]|[_])+)");
            string parametro;

            parametro = pattern.Match(strSQL).Value;

            while (parametro != "")
            {
                strSQL = strSQL.Replace(parametro + " ", "?");
                parametro = pattern.Match(strSQL).Value;
            }

            strSQL = strSQL.Replace("GetDate()", "Now()");

            //20/05/2013
            strSQL = strSQL.Replace(" UPPER(", " UCASE(");

            //20/02/2014
            strSQL = strSQL.Replace("'%", "'*");
            strSQL = strSQL.Replace("%'", "*'");

            return strSQL;
        }

        public System.Data.Common.DbParameter mAddParameter(System.Data.Common.DbCommand command, string name, Object value)
        {
            System.Data.Common.DbParameter parameter = null;

            switch (command.GetType().Name)
            {
                case "SqlCommand":
                    parameter = new System.Data.SqlClient.SqlParameter();
                    parameter.Value = value;

                    if (value != null && value != DBNull.Value)
                    {
                        (parameter as System.Data.SqlClient.SqlParameter).SqlDbType = mGetSqlDbType(value);
                    }
                    else
                    {
                        parameter.Value = DBNull.Value;
                    }

                    parameter.ParameterName = name;
                    command.Parameters.Add(parameter);
                    break;
                case "OleDbCommand":
                    parameter = new System.Data.OleDb.OleDbParameter();
                    parameter.Value = value;

                    if (value != null && value != DBNull.Value)
                    {
                        (parameter as System.Data.OleDb.OleDbParameter).OleDbType = mGetOleDbType(value);
                    }
                    else
                    {
                        parameter.Value = DBNull.Value;
                    }

                    parameter.ParameterName = name;
                    command.Parameters.Add(parameter);
                    break;
#if MySQL
                case "MySqlCommand":
                    parameter = new MySql.Data.MySqlClient.MySqlParameter();
                    parameter.Value = value;
                    if (value != null && value != DBNull.Value)
                    {
                        (parameter as MySql.Data.MySqlClient.MySqlParameter).MySqlDbType = getMySqlDbType(value);
                    }
                    else
                    {
                        parameter.Value = DBNull.Value;
                    }
                    parameter.ParameterName = name;
                    command.Parameters.Add(parameter);
                    break;
#endif


#if Oracle
                case "OracleCommand":
                    parameter = new Oracle.DataAccess.Client.OracleParameter();
                    parameter.Value = value;

                    if (value != null && value != DBNull.Value)
                    {
                        (parameter as Oracle.DataAccess.Client.OracleParameter).OracleDbType = mGetOracleDbType(value);
                    }
                    else
                    {
                        parameter.Value = DBNull.Value;
                    }

                    parameter.ParameterName = name;
                    command.Parameters.Add(parameter);
                    break;

#endif
                default:
                    throw new MyException("Tipo di comando non supportato: " + command.GetType().Name);
            }

            return parameter;
        }

        protected System.Data.SqlDbType mGetSqlDbType(Object value)
        {
            switch (value.GetType().Name)
            {
                case "Double":
                    return SqlDbType.Decimal;
                case "Boolean":
                    return System.Data.SqlDbType.Bit;
                case "Int64": //Long
                    return System.Data.SqlDbType.BigInt;
                case "Int32": //Int
                    return System.Data.SqlDbType.Int;
                case "Int16": //SmallInt
                    return System.Data.SqlDbType.SmallInt;
                case "String":
                    return System.Data.SqlDbType.VarChar;
                case "Byte[]":
                    return System.Data.SqlDbType.Binary;
                case "DateTime":
                    return System.Data.SqlDbType.DateTime;
                case "Decimal":
                    return System.Data.SqlDbType.Decimal;
                case "Char":
                    return System.Data.SqlDbType.Char;
                case "Guid":
                    return System.Data.SqlDbType.UniqueIdentifier;
                default:
                    Debug.WriteLine("MyException Tipo di dato non supportato: " + value.GetType().Name);
                    throw new MyException("Tipo di dato non supportato: " + value.GetType().Name);
            }
        }

        protected System.Data.OleDb.OleDbType mGetOleDbType(Object value)
        {
            switch (value.GetType().Name)
            {
                case "Boolean":
                    return System.Data.OleDb.OleDbType.Boolean;
                case "Int64": //Long
                    return System.Data.OleDb.OleDbType.BigInt;
                case "Int32": //Int
                    return System.Data.OleDb.OleDbType.Integer;
                case "Int16": //SmallInt
                    return System.Data.OleDb.OleDbType.SmallInt;
                case "String":
                    return System.Data.OleDb.OleDbType.VarChar;
                case "Byte[]":
                    return System.Data.OleDb.OleDbType.Binary;
                case "DateTime":
                    return System.Data.OleDb.OleDbType.Date;
                case "Decimal":
                    //Roberto Rutigliano 24/02/2010 con Decimal su access non funziona!!!
                    return System.Data.OleDb.OleDbType.Double;
                case "Char":
                    return System.Data.OleDb.OleDbType.Char;
                default:
                    throw new MyException("Tipo di dato non supportato: " + value.GetType().Name);

            }
        }

#if Oracle

        protected Oracle.DataAccess.Client.OracleDbType mGetOracleDbType(Object value)
        {
            switch (value.GetType().Name)
            {
                case "Boolean":
                    return Oracle.DataAccess.Client.OracleDbType.Byte;
                case "Int64": //Long
                    return Oracle.DataAccess.Client.OracleDbType.Int64;
                case "Int32": //Int
                    return Oracle.DataAccess.Client.OracleDbType.Int32;
                case "Int16": //SmallInt
                    return Oracle.DataAccess.Client.OracleDbType.Int16;
                case "String":
                    return Oracle.DataAccess.Client.OracleDbType.Varchar2;
                case "Byte[]":
                    return Oracle.DataAccess.Client.OracleDbType.Blob;
                case "DateTime":
                    return Oracle.DataAccess.Client.OracleDbType.Date;
                case "Decimal":
                    return Oracle.DataAccess.Client.OracleDbType.Decimal;
                case "Char":
                    return Oracle.DataAccess.Client.OracleDbType.Char;
                default:
                    throw new MyException("Tipo di dato non supportato: " + value.GetType().Name);

            }
        }
#endif



#if MySQL
        protected MySql.Data.MySqlClient.MySqlDbType getMySqlDbType(Object value)
        {
            switch (value.GetType().Name)
            {

                case "Boolean":
                    return MySql.Data.MySqlClient.MySqlDbType.Bit;
                case "Int64"://Long
                    return MySql.Data.MySqlClient.MySqlDbType.Int64;
                case "Int32"://Int
                    return MySql.Data.MySqlClient.MySqlDbType.Int32;
                case "Int16"://SmallInt
                    return MySql.Data.MySqlClient.MySqlDbType.Int16;
                case "String":
                    return MySql.Data.MySqlClient.MySqlDbType.String;
                case "Byte[]":
                    return MySql.Data.MySqlClient.MySqlDbType.Binary;
                case "DateTime":
                    return MySql.Data.MySqlClient.MySqlDbType.Timestamp;
                default:
                    throw new MyException("Tipo di dato non supportato: " + value.GetType().Name);
            }
        }
#endif
        public int mExecuteNoQuery(System.Data.Common.DbCommand command)
        {
            //'Roberto Rutigliano 04/04/2008:  'Per ACCESS e PostgreSQL ...
            if (mConnection.GetType().Name == "OleDbConnection" || mConnection.GetType().Name == "OdbcConnection")
            {
                command.CommandText = mParseSQLforAccessAndPostgreSQL(command.CommandText);
            }
            else if (mConnection.GetType().Name == "MySqlConnection")
            {
                command.CommandText = mParseSQLforMySQL(command.CommandText);
            }
            else if (mConnection.GetType().Name == "OracleConnection")
            {
                command.CommandText = mParseSQLforOracle(command.CommandText);
            }


            if (mTransaction != null)
            {
                command.Transaction = mTransaction;
            }

            int numeroDiRecordAggiornati;

            try
            {
                numeroDiRecordAggiornati = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new MyException("ExecuteNonQuery terminato con errori. " + Environment.NewLine + command.CommandText, ex);
            }
            finally
            {
                if (command != null)
                {
                    command.Dispose();
                    command = null;
                }
            }
            return numeroDiRecordAggiornati;
        }


        public int mExecuteNoQuery(string sqlQuery)
        {
            return mExecuteNoQuery(sqlQuery, -1);
        }

        public int mExecuteNoQuery(string sqlQuery, int timeOut)
        {
            int numeroDiRecordAggiornati;
            System.Data.Common.DbCommand command;

            command = mConnection.CreateCommand();
            command.CommandText = sqlQuery;
            command.Connection = mConnection;

            if (timeOut != -1)
            {
                //Il valore di default è 30 secondi
                // Valore espresso in secondi
                command.CommandTimeout = timeOut;
            }


            if (mConnection.GetType().Name == "OleDbConnection" || mConnection.GetType().Name == "OdbcConnection")
            {
                command.CommandText = mParseSQLforAccessAndPostgreSQL(command.CommandText);
            }
            else if (mConnection.GetType().Name == "MySqlConnection")
            {
                command.CommandText = mParseSQLforMySQL(command.CommandText);
            }


            if (mTransaction != null)
            {
                command.Transaction = mTransaction;
            }


            try
            {
                numeroDiRecordAggiornati = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //if (ex.GetType() == typeof ( System.Data.SqlClient.SqlException))
                //{
                //    if (((System.Data.SqlClient.SqlException)ex).ErrorCode == -2146232060)
                //    {
                //        throw new MyException( MyException.ErrorNumber.Constraint_violation , command.CommandText);
                //    }
                //}

                throw new MyException("ExecuteNonQuery terminato con errori. " + Environment.NewLine + command.CommandText, ex);
            }
            finally
            {
                if (command != null)
                {
                    command.Dispose();
                    command = null;
                }
            }

            return numeroDiRecordAggiornati;
        }

        protected long mGetIdentity()
        {
            System.Data.Common.DbCommand command;
            command = mConnection.CreateCommand();

            switch (command.GetType().Name)
            {
                case "MySqlCommand":
                    command.CommandText = "SELECT LAST_INSERT_ID()";
                    break;
                case "SqlCommand":
                case "OleDbCommand":
                case "SqlCeCommand":
                    command.CommandText = "SELECT @@IDENTITY";
                    break;
                default:
                    throw new MyException("GetIdentity(): tipo non supportato " + command.GetType().Name);
            }

            command.Connection = mConnection;

            if (mTransaction != null)
            {
                command.Transaction = mTransaction;
            }


            //'Return Long.Parse(Me.mExecuteScalar(command))
            //'26/12/2009
            //'Se passo per la funzione di Pasing della stringa di blocca con @@identity

            Object obj;
            try
            {

                obj = command.ExecuteScalar();
            }
            finally
            {
                if (command != null)
                {
                    command.Dispose();
                    command = null;
                }
            }

            if (obj == null)
            {
                return -1;
            }

            return long.Parse(obj.ToString());
        }


        protected long mGetSequence(string sequenceName)
        {
            string strSQL = "select " + sequenceName + ".nextval from dual";
            System.Data.Common.DbCommand command;
            command = mConnection.CreateCommand();

            command.CommandText = strSQL;

            return Convert.ToInt64(command.ExecuteScalar());
        }


        #region "_mTransaction___"

        public void mTransactionBegin(ref System.Data.Common.DbTransaction transaction)
        {
            if (transaction == null)
            {
                throw new MyException("Transazione NON inizializzata");
            }
            mTransaction = transaction;
        }

        public void mTransactionBegin()
        {
            if (mTransaction != null)
            {
                throw new MyException("Transazione già aperta");
            }
            mTransaction = mConnection.BeginTransaction();
        }

        public void mTransactionCommit()
        {
            if (mTransaction == null)
            {
                throw new MyException("Transazione NON inizializzata");
            }
            mTransaction.Commit();
            mTransaction.Dispose();
            mTransaction = null;
        }

        public void mTransactionRollback()
        {
            if (mTransaction == null)
            {
                throw new MyException("Transazione NON inizializzata");

            }
            mTransaction.Rollback();
            mTransaction.Dispose();
            mTransaction = null;
        }

        public System.Data.Common.DbTransaction mGetTransaction()
        {
            //'26/01/2012 commento
            //' If mTransaction Is Nothing Then
            //' Throw New ManagerException("Transazione NON inizializzata")
            //'xit Function
            //'  End If
            return mTransaction;
        }
        #endregion



        public string mExecuteScalar(string sqlQuery)
        {
            return mExecuteScalar(sqlQuery, -1);
        }

        public string mExecuteScalar(string sqlQuery, int timeOut)
        {
            System.Data.Common.DbCommand command;
            command = mConnection.CreateCommand();

            command.CommandText = sqlQuery;
            command.Connection = mConnection;

            if (timeOut != -1)
            {
                command.CommandTimeout = timeOut;
            }

            if (mTransaction != null)
            {
                command.Transaction = mTransaction;
            }

            return mExecuteScalar(command);
        }

        public string mExecuteScalar(System.Data.Common.DbCommand command)
        {

            if (mConnection.GetType().Name == "OleDbConnection" || mConnection.GetType().Name == "OdbcConnection")
            {
                command.CommandText = mParseSQLforAccessAndPostgreSQL(command.CommandText);
            }
            else if (mConnection.GetType().Name == "MySqlConnection")
            {
                command.CommandText = mParseSQLforMySQL(command.CommandText);
            }
            else if (mConnection.GetType().Name == "OracleConnection")
            {
                command.CommandText = mParseSQLforOracle(command.CommandText);
            }


            Object obj;

            try
            {

                if (mTransaction != null)
                {
                    command.Transaction = mTransaction;
                }

                obj = command.ExecuteScalar();
            }
            finally
            {
                if (command != null)
                {
                    command.Dispose();
                    command = null;
                }
            }
            if (obj == null)
            {
                return "";
            }
            return obj.ToString();

        }

        protected int mExecuteNoQueryWithDuplicateKey(string sqlQuery)
        {
            System.Data.Common.DbCommand command;
            command = mConnection.CreateCommand();

            command.CommandText = sqlQuery;
            command.Connection = mConnection;

            if (mTransaction != null)
            {
                command.Transaction = mTransaction;
            }

            return mExecuteNoQueryWithDuplicateKey(command);

        }

        protected int mExecuteNoQueryWithDuplicateKey(System.Data.Common.DbCommand command)
        {

            try
            {
                return mExecuteNoQuery(command);
            }
            catch (MyManagerCSharp.MyException ex)
            {

                if (ex.InnerException.GetType() == typeof(System.Data.SqlClient.SqlException))
                {
                    if (((System.Data.SqlClient.SqlException)ex.InnerException).ErrorCode == -2146232060)
                    {
                        //ignoro la presenza di record duplicati
                        //Debug.WriteLine("Exception: " + ex.InnerException.Message);
                        return 0;
                    }
                    throw ex;
                }
                else
                {
                    Debug.WriteLine("Exception: " + ex.Message);
                    throw ex;
                }
            }
        }

        protected int mExecuteDeleteWithContraintViolation(string sql)
        {
            try
            {
                return mExecuteNoQuery(sql);
            }
            catch (MyManagerCSharp.MyException ex)
            {

                if (ex.InnerException.GetType() == typeof(System.Data.SqlClient.SqlException))
                {
                    if (((System.Data.SqlClient.SqlException)ex.InnerException).ErrorCode == -2146232060)
                    {
                        //ignoro la presenza di record dupliati
                        //Debug.WriteLine("Exception: " + ex.InnerException.Message);
                        return 0;
                    }
                    throw ex;
                }
                else
                {
                    Debug.WriteLine("Exception: " + ex.Message);
                    throw ex;
                }
            }
        }

        public string getWhereConditionByDate(string queryField, DateTime dataIniziale, DateTime dataFinale)
        {

            if (dataIniziale == dataFinale)
            {
                return String.Format(" AND (DAY({0})={1} AND  MONTH({0})={2} AND YEAR({0})={3}) ", queryField, dataIniziale.Day, dataIniziale.Month, dataIniziale.Year);
            }


            if (mConnection.GetType().Name == "MySqlConnection")
            {
                return String.Format(" AND ( CAST( {0} AS DATE)  BETWEEN CAST('{1}' AS DATE) AND CAST('{2}' AS DATE) ) ", queryField, dataIniziale.ToString("yyyy-MM-dd"), dataFinale.ToString("yyyy-MM-dd"));
            }



            //http://www.sqlusa.com/bestpractices/datetimeconversion/
            return String.Format(" AND (  CONVERT(date, {0} )  Between CONVERT(date, '{1}', 103)  AND  CONVERT(date, '{2}', 103) ) ", queryField, dataIniziale.ToString("dd/MM/yyyy"), dataFinale.ToString("dd/MM/yyyy"));

        }

        public string getWhereConditionByDate(string queryField, Days? days)
        {
            if (days == null || days == Days.Tutti )
            {
                return "";
            }

            //http://office.microsoft.com/en-us/access-help/examples-of-using-dates-as-criteria-in-access-queries-HA102809751.aspx
            string strWHERE = "";

            DateTime dataCorrente = DateTime.Now;
            DateTime inizioSettimana;
            DayOfWeek startOfWeek = DayOfWeek.Monday; //Lunedì

            switch (days)
            {
                case Days.Oggi:
                    //EXTRACT(year FROM date_added)
                    if (mConnection.GetType().Name == "OracleConnection")
                    {
                        strWHERE += String.Format(" AND ( EXTRACT(DAY FROM {0})={1} AND  EXTRACT(MONTH FROM {0})={2} AND  EXTRACT (YEAR FROM {0})={3}) ", queryField, dataCorrente.Day, dataCorrente.Month, dataCorrente.Year);
                    }
                    else
                    {
                        strWHERE += String.Format(" AND (DAY({0})={1} AND  MONTH({0})={2} AND YEAR({0})={3}) ", queryField, dataCorrente.Day, dataCorrente.Month, dataCorrente.Year);
                    }
                    break;
                case Days.Ieri:
                    DateTime ieri = dataCorrente.AddDays(-1);
                    if (mConnection.GetType().Name == "OracleConnection")
                    {
                        strWHERE += String.Format(" AND ( EXTRACT(DAY FROM {0})={1} AND  EXTRACT(MONTH FROM {0})={2} AND  EXTRACT (YEAR FROM {0})={3}) ", queryField, ieri.Day, ieri.Month, ieri.Year);
                    }
                    else
                    {
                        strWHERE += String.Format(" AND (DAY({0})={1} AND  MONTH({0})={2} AND YEAR({0})={3}) ", queryField, ieri.Day, ieri.Month, ieri.Year);
                    }
                    break;
                case Days.Ultimi_7_giorni:
                    if (mConnection.GetType().Name == "MySqlConnection")
                    {
                        strWHERE += String.Format(" AND ( {0} Between  date_add(Curdate(), INTERVAL -7 DAY)   AND  Curdate() )", queryField);
                    }
                    else if (mConnection.GetType().Name == "OracleConnection")
                    {
                        strWHERE += String.Format(" AND (  TRUNC({0})  Between TRUNC(SYSDATE)-7  AND  TRUNC(SYSDATE)  )", queryField);
                    }
                    else
                    {
                        strWHERE += String.Format(" AND (  CONVERT(date, {0} )  Between CONVERT(date, GetDate() - 7)  AND  CONVERT(date, GetDate())  )", queryField);
                    }
                    break;
                case Days.Ultimi_15_giorni:
                    if (mConnection.GetType().Name == "MySqlConnection")
                    {
                        strWHERE += String.Format(" AND ( {0} Between  date_add(Curdate(), INTERVAL -15 DAY)   AND  Curdate() )", queryField);
                    }
                    else if (mConnection.GetType().Name == "OracleConnection")
                    {
                        strWHERE += String.Format(" AND (  TRUNC({0})  Between TRUNC(SYSDATE)-15  AND  TRUNC(SYSDATE)  )", queryField);
                    }
                    else
                    {
                        strWHERE += String.Format(" AND ( CONVERT(date, {0} ) Between  CONVERT(date, GetDate() - 15)  AND  CONVERT(date, GetDate()) )", queryField);
                    }
                    break;
                case Days.Ultimi_30_giorni:
                    if (mConnection.GetType().Name == "MySqlConnection")
                    {
                        strWHERE += String.Format(" AND ( {0} Between  date_add(Curdate(), INTERVAL -30 DAY)   AND  Curdate() )", queryField);
                    }
                    else if (mConnection.GetType().Name == "OracleConnection")
                    {
                        strWHERE += String.Format(" AND (  TRUNC({0})  Between TRUNC(SYSDATE)-30  AND  TRUNC(SYSDATE)  )", queryField);
                    }
                    else
                    {
                        strWHERE += String.Format(" AND ( CONVERT(date, {0} )  Between CONVERT(date, GetDate() - 30)  AND  CONVERT(date, GetDate()) )", queryField);
                    }
                    break;
                case Days.Settimana_corrente:
                    Debug.WriteLine(dataCorrente.DayOfWeek);

                    int diff = dataCorrente.DayOfWeek - startOfWeek;
                    if (diff < 0)
                    {
                        diff += 7;
                    }

                    inizioSettimana = dataCorrente.AddDays(-1 * diff).Date;

                    strWHERE += getWhereConditionByDate(queryField, inizioSettimana, inizioSettimana.AddDays(6));


                    //if (mConnection.GetType().Name == "MySqlConnection")
                    //{
                    //    // WEEK(Now(),1) ,1 per fare iniziare la settimana da lunedì
                    //    strWHERE += String.Format(" AND ( WEEK({0},1) = WEEK(Now(),1) AND  Year({0}) = Year(Now())  )", queryField);
                    //}
                    //else
                    //{
                    //    // su T-SQL SET DATEFIRST(1) per fare iniziare la settimana da lunedì
                    //    strWHERE += String.Format(" AND ( DatePart(\"WW\", {0} ) = DatePart(\"WW\", GetDate())  AND  Year({0}) = Year(GetDate()) )", queryField);
                    //}
                    break;
                case Days.Settimana_precedente:
                    Debug.WriteLine(dataCorrente.DayOfWeek);

                    diff = dataCorrente.DayOfWeek - startOfWeek;
                    if (diff < 0)
                    {
                        diff += 7;
                    }

                    inizioSettimana = dataCorrente.AddDays(-1 * diff).Date;

                    strWHERE += getWhereConditionByDate(queryField, inizioSettimana.AddDays(-7), inizioSettimana.AddDays(-1));


                    //if (mConnection.GetType().Name == "MySqlConnection")
                    //{
                    //    strWHERE += String.Format(" AND (  YEAR({0}) *53 + WEEK({0},1) = YEAR(Now()) *53 + WEEK(GetDate(),1) -1 )", queryField);
                    //}
                    //else
                    //{
                    //    strWHERE += String.Format(" AND (  YEAR({0}) *53 + DatePart(\"WW\",{0}) = YEAR( GetDate() ) * 53 + DatePart(\"WW\" , GetDate()) -1 )", queryField);
                    //}
                    break;
                case Days.Mese_corrente:
                    strWHERE += String.Format(" AND (YEAR({0}) = YEAR(GetDate()) ) AND ( MONTH({0}) = MONTH( GetDate() ) )", queryField);
                    break;
                case Days.Mese_precedente:
                    strWHERE += String.Format(" AND (  (YEAR({0})*12 + MONTH({0}) ) =  (YEAR( GetDate() )  * 12 + MONTH( GetDate() ) -1 ) )", queryField);
                    break;
                case Days.Anno_corrente:
                    strWHERE += String.Format(" AND ( YEAR({0}) = YEAR(GetDate()) )", queryField);
                    break;
                case Days.Anno_precedente:
                    strWHERE += String.Format(" AND ( YEAR({0}) = YEAR(GetDate()) - 1 )", queryField);
                    break;
                case Days.Primo_semestre_anno_corrente:
                    strWHERE += String.Format(" AND ( YEAR({0}) = YEAR(GetDate())  AND  MONTH({0}) between 1 and 6 ) ", queryField);
                    break;
                case Days.Primo_semestre_anno_precedente:
                    strWHERE += String.Format(" AND ( YEAR({0}) = YEAR(GetDate()) -1  AND MONTH({0}) between 1 and 6 ) ", queryField);
                    break;
                case Days.Secondo_semestre_anno_corrente:
                    strWHERE += String.Format(" AND ( YEAR({0}) = YEAR(GetDate())  AND MONTH({0}) between 7 and 12 ) ", queryField);
                    break;
                case Days.Secondo_semestre_anno_precedente:
                    strWHERE += String.Format(" AND ( YEAR({0}) = YEAR(GetDate()) -1  AND  MONTH({0}) between 7 and 12 ) ", queryField);
                    break;
                case Days.Ultimo_semestre:
                    strWHERE += String.Format(" AND ( Year({0})* 12 + MONTH({0}) >= Year(GetDate())* 12 + MONTH( GetDate()) - 6   AND  Year({0})* 12 + MONTH({0}) <= Year(GetDate())* 12 + MONTH(GetDate())  ) ", queryField);
                    break;
            }

            return strWHERE;
        }

        public string getGroupByByDate(string queryField, Days? days)
        {
            if (days == null)
            {
                return "";
            }

            string strGroupBy = "";

            switch (days)
            {
                case Days.Oggi:
                case Days.Ieri:
                case Days.Ultimi_7_giorni:
                case Days.Ultimi_30_giorni:
                case Days.Ultimi_15_giorni:
                case Days.Settimana_corrente:
                case Days.Settimana_precedente:
                case Days.Mese_corrente:
                case Days.Mese_precedente:
                    strGroupBy += String.Format(" FORMAT({0}, 'yyyy-MM-dd')", queryField);
                    break;
                case Days.Anno_corrente:
                case Days.Anno_precedente:
                case Days.Primo_semestre_anno_corrente:
                case Days.Primo_semestre_anno_precedente:
                case Days.Secondo_semestre_anno_corrente:
                case Days.Secondo_semestre_anno_precedente:
                case Days.Ultimo_semestre:
                case Days.Tutti:
                    strGroupBy += String.Format(" FORMAT({0}, 'yyyy-MM')", queryField);
                    break;

            }

            return strGroupBy;
        }


        public void mReseedIdentity(string tableName, int newValue)
        {
            mStrSQL = String.Format("DBCC CHECKIDENT ( {0}, RESEED, {1} ) WITH NO_INFOMSGS", tableName, newValue);
            mExecuteNoQuery(mStrSQL);
        }


        public string mGetLastMigrationId()
        {
            mStrSQL = "SELECT MigrationId   FROM __MigrationHistory";
            mDt = mFillDataTable(mStrSQL);

            //prendo l'ultima riga!
            string temp;
            temp = mDt.Rows[mDt.Rows.Count - 1][0].ToString();

            Debug.WriteLine(temp);
            return temp;
        }

    }
}




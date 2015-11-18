//Direttive di compilazione per le librerire esterne

//#define MySQL
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
            Mese_corrente,
            Settimana_precedente,
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

        protected System.Data.Common.DbConnection _connection;
        protected System.Data.Common.DbProviderFactory _factory;
        protected System.Data.Common.DbTransaction _transaction;

        protected string _connectionName;

        private string _provider;

        protected string _strSQL;

        protected DataTable _dt;

        public ManagerDB(string connectionName)
        {
            //In questo costruttore viene passata la connessione da utilizzare 
            _provider = System.Configuration.ConfigurationManager.ConnectionStrings[connectionName].ProviderName;
            _connectionName = connectionName;

            // Creazione dell'oggetto factory
            _factory = System.Data.Common.DbProviderFactories.GetFactory(_provider);

            _connection = _factory.CreateConnection();
            _connection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
        }

        public ManagerDB(System.Data.Common.DbConnection connection)
        {
            //In questo costruttore viene passata la connessione da utilizzare 
            _connection = connection;

            if (_connection == null)
            {
                return;
            }

            switch (_connection.GetType().Name)
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
                    throw new MyException("Il costruttore non gestisce  questo tipo di connessione: " + _connection.GetType().Name);
            }

            _factory = System.Data.Common.DbProviderFactories.GetFactory(_provider);
        }

        public System.Data.Common.DbConnection getConnection()
        {
            return _connection;
        }

        public void changeConnectionString(string connectionString)
        {
            _connection.ConnectionString = connectionString;
        }

        public void openConnection()
        {
            if (String.IsNullOrEmpty(_connection.ConnectionString))
            {
                Debug.WriteLine("ConnectionString IS NULL");
                if (String.IsNullOrEmpty(_connectionName))
                {
                    throw new ApplicationException("ConnectionString non inizializzata");
                }
                else
                {
                    _connection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings[_connectionName].ConnectionString;
                }
            }

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }
        }

        public void closeConnection()
        {
            _connection.Close();
            _connection.Dispose();
        }

        public DataTable _fillDataTable(System.Data.Common.DbCommand command)
        {
            DataSet dataSet;
            dataSet = new DataSet();

            _fillDataSet(command, dataSet, null, -1);

            return dataSet.Tables[0];
        }

        public DataTable _fillDataTable(string sql)
        {
            return _fillDataTable(sql, -1);
        }

        public DataTable _fillDataTable(string sql, int timeOut)
        {
            DataSet dataSet;
            dataSet = new DataSet();

            _fillDataSet(sql, dataSet, null, timeOut);

            return dataSet.Tables[0];
        }

        protected DataSet _fillDataSet(string sqlQuery, string dataSetName, string tableName, int timeOut)
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

            _fillDataSet(sqlQuery, dataSet, tableName, timeOut);

            return dataSet;
        }

        protected void _fillDataSet(string sqlQuery, DataSet ds, string tableName, int timeOut)
        {
            System.Data.Common.DbCommand command;

            command = _connection.CreateCommand();
            command.Connection = _connection;
            command.CommandText = sqlQuery;

            _fillDataSet(command, ds, tableName, timeOut);
        }

        protected void _fillDataSet(System.Data.Common.DbCommand command, DataSet ds, string tableName, int timeOut)
        {
            if (_transaction != null)
            {
                command.Transaction = _transaction;
            }

            if (_connection.GetType().Name == "OleDbConnection" || _connection.GetType().Name == "OdbcConnection")
            {
                //'Per ACCESS e PostgreSQL ...
                command.CommandText = parseSQLforAccessAndPostgreSQL(command.CommandText);
            }
            else if (_connection.GetType().Name == "MySqlConnection")
            {
                command.CommandText = parseSQLforMySQL(command.CommandText);
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

                objAdap = getDataAdapter(command);



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
                    Case "OracleDataAdapter"
                        DirectCast(objAdap, System.Data.OracleClient.OracleDataAdapter).Fill(ds, table)
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



        private System.Data.Common.DataAdapter getDataAdapter(System.Data.Common.DbCommand command)
        {
            System.Data.Common.DataAdapter objAdap = null;

            switch (_connection.GetType().Name)
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
            Case "OracleConnection"
                objAdap = New System.Data.OracleClient.OracleDataAdapter
                DirectCast(objAdap, System.Data.OracleClient.OracleDataAdapter).SelectCommand = DirectCast(command, System.Data.OracleClient.OracleCommand)
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
                    throw new MyException("Tipo di connessione non riconosciuta: " + _connection.GetType().Name);
            }

            return objAdap;
        }



        protected string parseSQLforMySQL(string strSQL)
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

        protected string parseSQLforAccessAndPostgreSQL(string strSQL)
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





        public System.Data.Common.DbParameter _addParameter(System.Data.Common.DbCommand command, string name, Object value)
        {
            System.Data.Common.DbParameter parameter = null;

            switch (command.GetType().Name)
            {
                case "SqlCommand":
                    parameter = new System.Data.SqlClient.SqlParameter();
                    parameter.Value = value;

                    if (value != null && value != DBNull.Value)
                    {
                        (parameter as System.Data.SqlClient.SqlParameter).SqlDbType = getSqlDbType(value);
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
                        (parameter as System.Data.OleDb.OleDbParameter).OleDbType = getOleDbType(value);
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
                default:
                    throw new MyException("Tipo di comando non supportato: " + command.GetType().Name);
            }

            return parameter;
        }

        protected System.Data.SqlDbType getSqlDbType(Object value)
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


        protected System.Data.OleDb.OleDbType getOleDbType(Object value)
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
        public int _executeNoQuery(System.Data.Common.DbCommand command)
        {
            //'Roberto Rutigliano 04/04/2008:  'Per ACCESS e PostgreSQL ...
            if (_connection.GetType().Name == "OleDbConnection" || _connection.GetType().Name == "OdbcConnection")
            {
                command.CommandText = parseSQLforAccessAndPostgreSQL(command.CommandText);
            }
            else if (_connection.GetType().Name == "MySqlConnection")
            {
                command.CommandText = parseSQLforMySQL(command.CommandText);
            }


            if (_transaction != null)
            {
                command.Transaction = _transaction;
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


        public int _executeNoQuery(string sqlQuery)
        {
            return _executeNoQuery(sqlQuery, -1);
        }

        public int _executeNoQuery(string sqlQuery, int timeOut)
        {
            int numeroDiRecordAggiornati;
            System.Data.Common.DbCommand command;

            command = _connection.CreateCommand();
            command.CommandText = sqlQuery;
            command.Connection = _connection;

            if (timeOut != -1)
            {
                //Il valore di default è 30 secondi
                // Valore espresso in secondi
                command.CommandTimeout = timeOut;
            }


            if (_connection.GetType().Name == "OleDbConnection" || _connection.GetType().Name == "OdbcConnection")
            {
                command.CommandText = parseSQLforAccessAndPostgreSQL(command.CommandText);
            }
            else if (_connection.GetType().Name == "MySqlConnection")
            {
                command.CommandText = parseSQLforMySQL(command.CommandText);
            }


            if (_transaction != null)
            {
                command.Transaction = _transaction;
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

        protected long _getIdentity()
        {
            System.Data.Common.DbCommand command;
            command = _connection.CreateCommand();

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

            command.Connection = _connection;

            if (_transaction != null)
            {
                command.Transaction = _transaction;
            }


            //'Return Long.Parse(Me._executeScalar(command))
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




        #region "__TRANSACTION___"

        public void _transactionBegin(ref System.Data.Common.DbTransaction transaction)
        {
            if (transaction == null)
            {
                throw new MyException("Transazione NON inizializzata");
            }
            _transaction = transaction;
        }

        public void _transactionBegin()
        {
            if (_transaction != null)
            {
                throw new MyException("Transazione già aperta");
            }
            _transaction = _connection.BeginTransaction();
        }

        public void _transactionCommit()
        {
            if (_transaction == null)
            {
                throw new MyException("Transazione NON inizializzata");
            }
            _transaction.Commit();
            _transaction.Dispose();
            _transaction = null;
        }

        public void _transactionRollback()
        {
            if (_transaction == null)
            {
                throw new MyException("Transazione NON inizializzata");

            }
            _transaction.Rollback();
            _transaction.Dispose();
            _transaction = null;
        }

        public System.Data.Common.DbTransaction _getTransaction()
        {
            //'26/01/2012 commento
            //' If _transaction Is Nothing Then
            //' Throw New ManagerException("Transazione NON inizializzata")
            //'xit Function
            //'  End If
            return _transaction;
        }
        #endregion





        public string _executeScalar(string sqlQuery)
        {
            return _executeScalar(sqlQuery, -1);
        }

        public string _executeScalar(string sqlQuery, int timeOut)
        {
            System.Data.Common.DbCommand command;
            command = _connection.CreateCommand();

            command.CommandText = sqlQuery;
            command.Connection = _connection;

            if (timeOut != -1)
            {
                command.CommandTimeout = timeOut;
            }

            if (_transaction != null)
            {
                command.Transaction = _transaction;
            }

            return _executeScalar(command);
        }

        public string _executeScalar(System.Data.Common.DbCommand command)
        {

            if (_connection.GetType().Name == "OleDbConnection" || _connection.GetType().Name == "OdbcConnection")
            {
                command.CommandText = parseSQLforAccessAndPostgreSQL(command.CommandText);
            }
            else if (_connection.GetType().Name == "MySqlConnection")
            {
                command.CommandText = parseSQLforMySQL(command.CommandText);
            }


            Object obj;

            try
            {

                if (_transaction != null)
                {
                    command.Transaction = _transaction;
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


        protected int executeNoQueryWithDuplicateKey(string sqlQuery)
        {
            System.Data.Common.DbCommand command;
            command = _connection.CreateCommand();

            command.CommandText = sqlQuery;
            command.Connection = _connection;

            if (_transaction != null)
            {
                command.Transaction = _transaction;
            }

            return executeNoQueryWithDuplicateKey(command);

        }

        protected int executeNoQueryWithDuplicateKey(System.Data.Common.DbCommand command)
        {

            try
            {
                return _executeNoQuery(command);
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

        protected int executeDeleteWithContraintViolation(string sql)
        {
            try
            {
                return _executeNoQuery(sql);
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


            if (_connection.GetType().Name == "MySqlConnection")
            {
                return String.Format(" AND ( CAST( {0} AS DATE)  BETWEEN CAST('{1}' AS DATE) AND CAST('{2}' AS DATE) ) ", queryField, dataIniziale.ToString("yyyy-MM-dd"), dataFinale.ToString("yyyy-MM-dd"));
            }



            //http://www.sqlusa.com/bestpractices/datetimeconversion/
            return String.Format(" AND (  CONVERT(date, {0} )  Between CONVERT(date, '{1}', 103)  AND  CONVERT(date, '{2}', 103) ) ", queryField, dataIniziale.ToString("dd/MM/yyyy"), dataFinale.ToString("dd/MM/yyyy"));

        }

        public string getWhereConditionByDate(string queryField, Days? days)
        {
            if (days == null)
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
                    strWHERE += String.Format(" AND (DAY({0})={1} AND  MONTH({0})={2} AND YEAR({0})={3}) ", queryField, dataCorrente.Day, dataCorrente.Month, dataCorrente.Year);
                    break;
                case Days.Ieri:
                    DateTime ieri = dataCorrente.AddDays(-1);
                    strWHERE += String.Format(" AND (DAY({0})={1} AND  MONTH({0})={2} AND YEAR({0})={3}) ", queryField, ieri.Day, ieri.Month, ieri.Year);
                    break;
                case Days.Ultimi_7_giorni:
                    if (_connection.GetType().Name == "MySqlConnection")
                    {
                        strWHERE += String.Format(" AND ( {0} Between  date_add(Curdate(), INTERVAL -7 DAY)   AND  Curdate() )", queryField);
                    }
                    else
                    {
                        strWHERE += String.Format(" AND (  CONVERT(date, {0} )  Between CONVERT(date, GetDate() - 7)  AND  CONVERT(date, GetDate())  )", queryField);
                    }
                    break;
                case Days.Ultimi_30_giorni:
                    if (_connection.GetType().Name == "MySqlConnection")
                    {
                        strWHERE += String.Format(" AND ( {0} Between  date_add(Curdate(), INTERVAL -30 DAY)   AND  Curdate() )", queryField);
                    }
                    else
                    {
                        strWHERE += String.Format(" AND ( CONVERT(date, {0} )  Between CONVERT(date, GetDate() - 30)  AND  CONVERT(date, GetDate()) )", queryField);
                    }
                    break;
                case Days.Ultimi_15_giorni:
                    if (_connection.GetType().Name == "MySqlConnection")
                    {
                        strWHERE += String.Format(" AND ( {0} Between  date_add(Curdate(), INTERVAL -15 DAY)   AND  Curdate() )", queryField);
                    }
                    else
                    {
                        strWHERE += String.Format(" AND ( CONVERT(date, {0} ) Between  CONVERT(date, GetDate() - 15)  AND  CONVERT(date, GetDate()) )", queryField);
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


                    //if (_connection.GetType().Name == "MySqlConnection")
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


                    //if (_connection.GetType().Name == "MySqlConnection")
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


        public void reseedIdentity(string tableName, int newValue)
        {
            _strSQL = String.Format("DBCC CHECKIDENT ( {0}, RESEED, {1} ) WITH NO_INFOMSGS", tableName, newValue);
            _executeNoQuery(_strSQL);
        }


        public string getLastMigrationId()
        {
            _strSQL = "SELECT MigrationId   FROM __MigrationHistory";
            _dt = _fillDataTable(_strSQL);

            //prendo l'ultima riga!
            string temp;
            temp = _dt.Rows[_dt.Rows.Count -1][0].ToString();

            Debug.WriteLine(temp);
            return temp;
        }

    }
}




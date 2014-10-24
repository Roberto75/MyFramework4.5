using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace MyManagerCSharp.Alert
{
    public class AlertManager : ManagerDB
    {

        private string SQL_SELECT_UTENTI_SMALL = " u.my_login, u.nome , u.cognome, u.email, u.user_id ";

        public AlertManager(string connectionName)
            : base(connectionName)
        {

        }

        public AlertManager(System.Data.Common.DbConnection connection)
            : base(connection)
        {

        }

        public List<MyManagerCSharp.Alert.Models.MyAlert> getList()
        {
            List<MyManagerCSharp.Alert.Models.MyAlert> risultato = new List<Models.MyAlert>();

            _strSQL = "SELECT * FROM MyAlert WHERE IS_ENABLED = 1 ORDER BY DESCRIZIONE";

            _dt = _fillDataTable(_strSQL);

            foreach (System.Data.DataRow row in _dt.Rows)
            {
                risultato.Add(new MyManagerCSharp.Alert.Models.MyAlert(row));
            }

            return risultato;
        }

        public List<MyManagerCSharp.Alert.Models.MyAlert> getMyAlert(long userId)
        {
            List<MyManagerCSharp.Alert.Models.MyAlert> risultato = new List<Models.MyAlert>();

            _strSQL = "SELECT t1.* " +
                        " from MyAlert as t1 " +
                        " join MyAlert_Utente as t2 on t1.id = t2.alert_id " +
                        " where t2.user_id = " + userId;

            _strSQL += "ORDER BY t1.DESCRIZIONE";

            _dt = _fillDataTable(_strSQL);

            foreach (System.Data.DataRow row in _dt.Rows)
            {
                risultato.Add(new MyManagerCSharp.Alert.Models.MyAlert(row));
            }

            return risultato;
        }

        public bool update(IEnumerable<Models.MyAlert> alerts, long userId)
        {
            try
            {
                _transactionBegin();

                _strSQL = "DELETE FROM  MyAlert_Utente WHERE user_id = " + userId;
                _executeNoQuery(_strSQL);

                foreach (Models.MyAlert a in alerts)
                {
                    Debug.WriteLine("Alert: " + a.id);
                    add( userId, a.id);
                }

                _transactionCommit();
                return true;
            }
            catch (Exception ex)
            {
                _transactionRollback();
                throw ex;
            }
        }

        public bool add(long userId, long alertId)
        {
            _strSQL = "INSERT INTO MyAlert_Utente ( date_added,  alert_id, user_id ) VALUES ( GetDate() , " + alertId + "," + userId + ")";

            _executeNoQuery(_strSQL);
            return true;
        }
        
        public List<MyManagerCSharp.Models.MyUserSmall> getUsers(long alertId)
        {

            _strSQL = "SELECT DISTINCT " + SQL_SELECT_UTENTI_SMALL +
                       " FROM MyAlert_Utente as t1 " +
                       " join Utente as u on t1.user_id = u.user_id " +
                       " WHERE ALERT_ID = " + alertId;
                     
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


        public Models.MyAlert getAlert(string nome)
        {
            _strSQL = "SELECT * FROM MYALERT WHERE UPPER(NOME) = @NOME ";

            System.Data.Common.DbCommand command;
            command = _connection.CreateCommand();

            _addParameter(command, "@NOME", nome.ToUpper().Trim());

            command.CommandText = _strSQL;
            _dt = _fillDataTable(command);

            if (_dt.Rows.Count > 1)
            {
                throw new MyManagerCSharp.MyException(MyManagerCSharp.MyException.ErrorNumber.Record_duplicato);
            }

            if (_dt.Rows.Count == 0)
            {
                return null;
            }

            return new Models.MyAlert(_dt.Rows[0]);
        }

    }
}

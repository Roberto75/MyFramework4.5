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

            mStrSQL = "SELECT * FROM MyAlert WHERE IS_ENABLED = 1 ORDER BY DESCRIZIONE";

            mDt = mFillDataTable(mStrSQL);

            foreach (System.Data.DataRow row in mDt.Rows)
            {
                risultato.Add(new MyManagerCSharp.Alert.Models.MyAlert(row));
            }

            return risultato;
        }


        public MyManagerCSharp.Alert.Models.MyAlert getAlertByUser(long userId, string alertName, double? valoreMinimo)
        {
            System.Data.Common.DbCommand command;
            command = mConnection.CreateCommand();

            mStrSQL = "SELECT  t1.*  " +
                " from MyAlert as t1  " +
                " join MyAlert_Utente as t2 on t1.id = t2.alert_id " +
                " where t2.USER_ID =  " + userId + " AND  t1.nome = @NOME AND t2.valore_minimo >= @VALORE ";
            
            command.CommandText = mStrSQL;

            mAddParameter(command, "@NOME", alertName);

            if (valoreMinimo == null)
            {
                mAddParameter(command, "@VALORE", 0);
            }
            else
            {
                mAddParameter(command, "@VALORE", valoreMinimo.Value);
            }

            mDt = mFillDataTable(command);

            if (mDt.Rows.Count == 0)
            {
                return null;
            }

            if (mDt.Rows.Count > 1)
            {
                throw new ApplicationException("ALERT > 1");
            }

            MyManagerCSharp.Alert.Models.MyAlert risultato;
            risultato = new MyManagerCSharp.Alert.Models.MyAlert(mDt.Rows[0]);

            return risultato;
        }
        
        public List<MyManagerCSharp.Alert.Models.MyAlert> getMyAlertV2(long userId)
        {
            List<MyManagerCSharp.Alert.Models.MyAlert> risultato = new List<Models.MyAlert>();

            mStrSQL = "SELECT  t2.id as is_selected ,  t2.valore_minimo, t1.*  " +
                " from MyAlert as t1 " +
                " left  join MyAlert_Utente as t2 on t1.id = t2.alert_id and  t2.user_id = " + userId +
                " ORDER BY t1.DESCRIZIONE ";

            mDt = mFillDataTable(mStrSQL);

            foreach (System.Data.DataRow row in mDt.Rows)
            {
                risultato.Add(new MyManagerCSharp.Alert.Models.MyAlert(row));
            }

            return risultato;
        }
        

        public bool update(IEnumerable<Models.MyAlert> alerts, long userId)
        {
            try
            {
                mTransactionBegin();

                mStrSQL = "DELETE FROM  MyAlert_Utente WHERE user_id = " + userId;
                mExecuteNoQuery(mStrSQL);

                foreach (Models.MyAlert a in alerts)
                {
                    Debug.WriteLine("Alert: " + a.id);
                    if (a.isSelected)
                    {
                        add(userId, a.id, a.valoreMinimo);
                    }
                }

                mTransactionCommit();
                return true;
            }
            catch (Exception ex)
            {
                mTransactionRollback();
                throw ex;
            }
        }

        public bool add(long userId, long alertId, double? valoreMinimo)
        {
            System.Data.Common.DbCommand command;
            command =  mConnection.CreateCommand();

            if (valoreMinimo != null)
            {
                mStrSQL = "INSERT INTO MyAlert_Utente ( date_added,  alert_id, user_id, valore_minimo ) VALUES ( GetDate() , " + alertId + "," + userId + ", @SEVERITY )";
                mAddParameter(command, "@SEVERITY", valoreMinimo.Value);
            }
            else
            {
                mStrSQL = "INSERT INTO MyAlert_Utente ( date_added,  alert_id, user_id ) VALUES ( GetDate() , " + alertId + "," + userId + ")";
            }

            command.CommandText = mStrSQL;

            mExecuteNoQuery(command);
            return true;
        }

        public List<MyManagerCSharp.Models.MyUserSmall> getUsers(long alertId, double? valoreMinimo)
        {
            System.Data.Common.DbCommand command;
            command =  mConnection.CreateCommand();

            mStrSQL = "SELECT DISTINCT " + SQL_SELECT_UTENTI_SMALL +
                       " FROM MyAlert_Utente as t1 " +
                       " join Utente as u on t1.user_id = u.user_id " +
                       " WHERE ALERT_ID = " + alertId;

            if (valoreMinimo != null)
            {
                mStrSQL += " AND t1.valore_minimo >= @VALORE ";
                mAddParameter(command, "@VALORE", valoreMinimo);
            }

            command.CommandText = mStrSQL;

            mDt = mFillDataTable(command);

            List<MyManagerCSharp.Models.MyUserSmall> risultato = new List<MyManagerCSharp.Models.MyUserSmall>();

            MyManagerCSharp.Models.MyUserSmall userSmall;
            foreach (System.Data.DataRow row in mDt.Rows)
            {
                userSmall = new MyManagerCSharp.Models.MyUserSmall(row);

                risultato.Add(userSmall);
            }

            return risultato;

        }


        public Models.MyAlert getAlert(string nome)
        {
            mStrSQL = "SELECT * FROM MYALERT WHERE UPPER(NOME) = @NOME ";

            System.Data.Common.DbCommand command;
            command =  mConnection.CreateCommand();

            mAddParameter(command, "@NOME", nome.ToUpper().Trim());

            command.CommandText = mStrSQL;
            mDt = mFillDataTable(command);

            if (mDt.Rows.Count > 1)
            {
                throw new MyManagerCSharp.MyException(MyManagerCSharp.MyException.ErrorNumber.Record_duplicato);
            }

            if (mDt.Rows.Count == 0)
            {
                return null;
            }

            return new Models.MyAlert(mDt.Rows[0]);
        }

    }
}

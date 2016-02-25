using MyManagerCSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace My.Shared.Log
{
    public class LogEntity : ManagerDB
    {

        public enum TipoOperazione
        {
            Inset,
            Update,
            Delete
        }

        public LogEntity(string connectionName)
            : base(connectionName)
        {

        }

        public LogEntity(System.Data.Common.DbConnection connection)
            : base(connection)
        {

        }

        protected void insert(string userName, TipoOperazione operazione, long entityId, string entityName, object valoreIniziale, object valoreNuovo)
        {

            string strSQLParametri;

            mStrSQL = "INSERT INTO MyLogEntity ( DATE_ADDED  ";
            strSQLParametri = " VALUES ( GetDate()  ";

            System.Data.Common.DbCommand command;
            command = mConnection.CreateCommand();
            command.Connection = mConnection;

            mStrSQL += ",user_name ";
            strSQLParametri += ", @USER_NAME ";
            mAddParameter(command, "@USER_NAME", userName);

            mStrSQL += ",entity_id ";
            strSQLParametri += ", @ENTITY_ID ";
            mAddParameter(command, "@ENTITY_ID", entityId);


            if (valoreIniziale != null && valoreNuovo != null)
            {

                if (valoreIniziale.GetType().FullName != valoreNuovo.GetType().FullName)
                {
                    throw new ArgumentException("I due oggetti non sono controntabili perchè sono di tipo diverso: " + valoreIniziale.GetType().FullName + " != " + valoreNuovo.GetType().FullName);
                }

                string temp;
                temp = cercaDatiModificati(valoreIniziale, valoreNuovo);

                Debug.WriteLine(temp);

            }

            //tring entityName;
            //entityName = entity.GetType().Name;

            mStrSQL += ",entity_name ";
            strSQLParametri += ", @ENTITY_NAME ";
            mAddParameter(command, "@ENTITY_NAME", entityName);

            command.CommandText = mStrSQL + " ) " + strSQLParametri + " )";
            command.CommandType = System.Data.CommandType.Text;

            mExecuteNoQuery(command);
        }



        private string cercaDatiModificati(object originale, object modificato)
        {
            if (originale == null)
            {
                //return "Attenzione l'oggetto originale == NULL";
                return "";
            }


            string retString = String.Empty;
            Type tipoOriginale = originale.GetType();
            Type tipoModificato = modificato.GetType();

            if (tipoOriginale != tipoModificato)
            {
                return "I due oggetti non sono confrontabili";
            }

            PropertyInfo[] propertiesOriginale = tipoOriginale.GetProperties();
            PropertyInfo[] propertiesModificato = tipoModificato.GetProperties();

            foreach (PropertyInfo propertyOriginale in propertiesOriginale)
            {
                PropertyInfo propertyModificato = (from p in propertiesModificato
                                                   where p.Name == propertyOriginale.Name
                                                   select p).FirstOrDefault();

                if (propertyModificato == null)
                {
                    continue;
                }

                object valoreOriginale = propertyOriginale.GetValue(originale);
                object valoreModificato = propertyModificato.GetValue(modificato);


                if (valoreOriginale == null && valoreModificato == null)
                {
                    continue;
                }

                if (valoreOriginale != null && valoreModificato != null && valoreOriginale.Equals(valoreModificato))
                {
                    continue;
                }

                retString += String.Format(";{0}@{1}@{2}", propertyOriginale.Name, valoreOriginale == null ? "null" : valoreOriginale.ToString(), valoreModificato == null ? "null" : valoreModificato.ToString());
            }

            Debug.WriteLine(retString);

            if (String.IsNullOrEmpty(retString))
            {
                return "";
            }


            //il primo campo indica il nome dell'oggetto
            return tipoOriginale.Name + retString;
        }

    }
}

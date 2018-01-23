using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyManagerCSharp
{
    public class RegioniProvinceComuniManager : ManagerDB
    {
        public RegioniProvinceComuniManager(string connectionName)
            : base(connectionName)
        {

        }

        public RegioniProvinceComuniManager(System.Data.Common.DbConnection connection)
            : base(connection)
        {

        }


        public System.Data.DataTable getProvince(long regioneId)
        {
            mStrSQL = "SELECT * FROM PROVINCE WHERE regione_id =" + regioneId + "  ORDER BY VALORE";

            mDt = mFillDataTable(mStrSQL);
            return mDt;
        }


        public System.Data.DataTable getComuni(string provinciaId)
        {
            //'provinciaId = SIGLA
            //'Escludo l'ITALIA tra gli STATI ESTERI
            mStrSQL = "SELECT * FROM COMUNI WHERE provincia_id = '" + provinciaId + "' AND ID <> 'IT' ORDER BY VALORE";

            mDt = mFillDataTable(mStrSQL);
            return mDt;
        }


        public System.Data.DataTable getRegioni()
        {
            mStrSQL = "SELECT * FROM REGIONI ORDER BY VALORE ";
            return mFillDataTable(mStrSQL);
        }

        public List<Models.MyItem> getComboRegioni()
        {
            mStrSQL = "SELECT * FROM REGIONI ORDER BY VALORE ";

            mDt = mFillDataTable(mStrSQL);

            List<Models.MyItem> risultato = new List<Models.MyItem>();

            Models.MyItem item;
            foreach (System.Data.DataRow row in mDt.Rows)
            {
                item = new Models.MyItem(row["id"].ToString(), row["valore"].ToString());

                risultato.Add(item);
            }

            return risultato;

        }



        public List<Models.MyItem> getComboProvince(long regioneId)
        {

            mStrSQL = "SELECT * FROM PROVINCE WHERE regione_id =" + regioneId + "  ORDER BY VALORE";

            mDt = mFillDataTable(mStrSQL);


            List<Models.MyItem> risultato = new List<Models.MyItem>();

            Models.MyItem item;
            foreach (System.Data.DataRow row in mDt.Rows)
            {
                item = new Models.MyItem(row["id"].ToString(), row["valore"].ToString());

                risultato.Add(item);
            }

            return risultato;

        }


        public List<Models.MyItem> getComboComuni(string provinciaId)
        {
            //'provinciaId = SIGLA
            //'Escludo l'ITALIA tra gli STATI ESTERI
            mStrSQL = "SELECT * FROM COMUNI WHERE provincia_id = '" + provinciaId + "' AND ID <> 'IT' ORDER BY VALORE";

            mDt = mFillDataTable(mStrSQL);


            List<Models.MyItem> risultato = new List<Models.MyItem>();

            Models.MyItem item;
            foreach (System.Data.DataRow row in mDt.Rows)
            {
                item = new Models.MyItem(row["id"].ToString(), row["valore"].ToString());

                risultato.Add(item);
            }

            return risultato;

        }



        public long getRegioneIdByLabel(string regione)
        {
            if (String.IsNullOrEmpty(regione))
            {
                throw new ArgumentException("regione is null");
            }

            if (regione == "Friuli-Venezia Giulia")
            {
                regione = "Friuli Venezia Giulia";
            }



            mStrSQL = "SELECT ID FROM REGIONI WHERE UCASE (VALORE) = @REGIONE ";

            System.Data.Common.DbCommand command;
            command = mConnection.CreateCommand();
            command.CommandText = mStrSQL;

            mAddParameter(command, "@REGIONE", regione.ToUpper());

            string temp = mExecuteScalar(command);
            if (String.IsNullOrEmpty(temp))
            {
                throw new MyException("Regione non trovata: " + regione);
            }


            return long.Parse(temp);
        }


        public System.Data.DataTable getRegioneByLabel(string regione)
        {
            //'  mStrSQL = "SELECT * FROM REGIONI WHERE UCASE (VALORE)  = '" & regione.ToUpper & "'"
            mStrSQL = "SELECT * FROM REGIONI WHERE UCASE (VALORE)  = @REGIONE ";

            System.Data.Common.DbCommand command;
            command = mConnection.CreateCommand();



            command.CommandText = mStrSQL;

            mAddParameter(command, "@REGIONE", regione.ToUpper());

            return mFillDataTable(command);
        }



        public string getRegioneById(int regioneId)
        {
            //'  mStrSQL = "SELECT * FROM REGIONI WHERE UCASE (VALORE)  = '" & regione.ToUpper & "'"
            mStrSQL = "SELECT valore FROM REGIONI WHERE  ID = " + regioneId;
            return mExecuteScalar(mStrSQL);
        }


        public System.Data.DataTable getProvinciaBySigla(string sigla)
        {
            mStrSQL = "SELECT * FROM PROVINCE WHERE UCASE (ID)  = '" + sigla.ToUpper() + "'";
            return mFillDataTable(mStrSQL);
        }

        public System.Data.DataTable getProvinciaByValore(string valore)
        {
            mStrSQL = "SELECT * FROM PROVINCE WHERE UCASE (valore)  = '" + valore.ToUpper().Replace("'", "''") + "'";
            return mFillDataTable(mStrSQL);
        }

        public System.Data.DataTable getComuneByCodiceISTAT(string cidiceISTAT)
        {
            mStrSQL = "SELECT * FROM COMUNI WHERE UCASE (ID)  = '" + cidiceISTAT.ToUpper() + "'";
            return mFillDataTable(mStrSQL);
        }

        public System.Data.DataTable getComuneByValore(string valore)
        {
            mStrSQL = "SELECT * FROM COMUNI WHERE UCASE (valore)  = '" + valore.ToUpper().Replace("'", "''") + "'";
            return mFillDataTable(mStrSQL);
        }

        public System.Data.DataTable getNazioneByDescrizione(string valore)
        {
            mStrSQL = "SELECT * FROM COMUNI WHERE UCASE (VALORE)  = '" + valore.ToUpper().Replace("'", "''") + "'";
            return mFillDataTable(mStrSQL);
        }


        public string getProvinciaIdByLabel(string provincia)
        {
            if (String.IsNullOrEmpty(provincia))
            {
                throw new ArgumentException("provincia is null");
            }


            if (provincia == "Reggio Emilia")
            {
                provincia = "Reggio nell'Emilia";
            }
            else if (provincia == "Monza e Brianza")
            {
                provincia = "Monza e della Brianza";
            }
            else if (provincia == "Bolzano")
            {
                provincia = "Bolzano/Bozen";
            }
            else if (provincia == "Massa e Carrara")
            {
                provincia = "Massa-Carrara";
            }
            else if (provincia == "Reggio Calabria")
            {
                provincia = "Reggio di Calabria";
            }
            else if (provincia == "Aosta")
            {
                provincia = "Valle d'Aosta/Vallée d'Aoste";
            }

            mStrSQL = "SELECT ID FROM PROVINCE WHERE UCASE (VALORE) = @PROVINCIA ";

            System.Data.Common.DbCommand command;
            command = mConnection.CreateCommand();
            command.CommandText = mStrSQL;

            mAddParameter(command, "@PROVINCIA", provincia.ToUpper());

            string temp = mExecuteScalar(command);
            if (String.IsNullOrEmpty(temp))
            {
                throw new MyException("Provincia non trovata: " + provincia);
            }
            return temp;
        }
    }
}

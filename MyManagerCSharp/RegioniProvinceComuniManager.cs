﻿using System;
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

    }
}

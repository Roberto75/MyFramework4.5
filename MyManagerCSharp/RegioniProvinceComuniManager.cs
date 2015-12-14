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

            m_dt = mFillDataTable(mStrSQL);
            return m_dt;
        }


        public System.Data.DataTable getComuni(string provinciaId)
        {
            //'provinciaId = SIGLA
            //'Escludo l'ITALIA tra gli STATI ESTERI
            mStrSQL = "SELECT * FROM COMUNI WHERE provincia_id = '" + provinciaId + "' AND ID <> 'IT' ORDER BY VALORE";

            m_dt = mFillDataTable(mStrSQL);
            return m_dt;
        }


        public System.Data.DataTable getRegioneByLabel(string regione)
        {
            //' _strSql = "SELECT * FROM REGIONI WHERE UCASE (VALORE)  = '" & regione.ToUpper & "'"
            mStrSQL = "SELECT * FROM REGIONI WHERE UCASE (VALORE)  = @REGIONE ";

            System.Data.Common.DbCommand command;
            command =  mConnection.CreateCommand();



            command.CommandText = mStrSQL;

            mAddParameter(command, "@REGIONE", regione.ToUpper());

            return mFillDataTable(command);
        }



        public string getRegioneById(int regioneId)
        {
            //' _strSql = "SELECT * FROM REGIONI WHERE UCASE (VALORE)  = '" & regione.ToUpper & "'"
            mStrSQL = "SELECT valore FROM REGIONI WHERE  ID = " + regioneId;
            return mExecuteScalar(mStrSQL );
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

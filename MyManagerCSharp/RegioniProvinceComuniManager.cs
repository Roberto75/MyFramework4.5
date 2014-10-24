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
            _strSQL = "SELECT * FROM PROVINCE WHERE regione_id =" + regioneId + "  ORDER BY VALORE";

            _dt = _fillDataTable(_strSQL);
            return _dt;
        }


        public System.Data.DataTable getComuni(string provinciaId)
        {
            //'provinciaId = SIGLA
            //'Escludo l'ITALIA tra gli STATI ESTERI
            _strSQL = "SELECT * FROM COMUNI WHERE provincia_id = '" + provinciaId + "' AND ID <> 'IT' ORDER BY VALORE";

            _dt = _fillDataTable(_strSQL);
            return _dt;
        }


        public System.Data.DataTable getRegioneByLabel(string regione)
        {
            //' _strSql = "SELECT * FROM REGIONI WHERE UCASE (VALORE)  = '" & regione.ToUpper & "'"
            _strSQL = "SELECT * FROM REGIONI WHERE UCASE (VALORE)  = @REGIONE ";

            System.Data.Common.DbCommand command;
            command = _connection.CreateCommand();



            command.CommandText = _strSQL;

            _addParameter(command, "@REGIONE", regione.ToUpper());

            return _fillDataTable(command);
        }



        public string getRegioneById(int regioneId)
        {
            //' _strSql = "SELECT * FROM REGIONI WHERE UCASE (VALORE)  = '" & regione.ToUpper & "'"
            _strSQL = "SELECT valore FROM REGIONI WHERE  ID = " + regioneId;
            return _executeScalar(_strSQL );
        }


        public System.Data.DataTable getProvinciaBySigla(string sigla)
        {
            _strSQL = "SELECT * FROM PROVINCE WHERE UCASE (ID)  = '" + sigla.ToUpper() + "'";
            return _fillDataTable(_strSQL);
        }

        public System.Data.DataTable getProvinciaByValore(string valore)
        {
            _strSQL = "SELECT * FROM PROVINCE WHERE UCASE (valore)  = '" + valore.ToUpper().Replace("'", "''") + "'";
            return _fillDataTable(_strSQL);
        }

        public System.Data.DataTable getComuneByCodiceISTAT(string cidiceISTAT)
        {
            _strSQL = "SELECT * FROM COMUNI WHERE UCASE (ID)  = '" + cidiceISTAT.ToUpper() + "'";
            return _fillDataTable(_strSQL);
        }

        public System.Data.DataTable getComuneByValore(string valore)
        {
            _strSQL = "SELECT * FROM COMUNI WHERE UCASE (valore)  = '" + valore.ToUpper().Replace("'", "''") + "'";
            return _fillDataTable(_strSQL);
        }

        public System.Data.DataTable getNazioneByDescrizione(string valore)
        {
            _strSQL = "SELECT * FROM COMUNI WHERE UCASE (VALORE)  = '" + valore.ToUpper().Replace("'", "''") + "'";
            return _fillDataTable(_strSQL);
        }

    }
}

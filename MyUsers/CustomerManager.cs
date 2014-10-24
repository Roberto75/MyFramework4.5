using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyManagerCSharp;
using System.Data;


namespace MyUsers
{
    public class CustomerManager : ManagerDB
    {

        public CustomerManager(string connectionName)
            : base(connectionName)
        {

        }

        public CustomerManager(System.Data.Common.DbConnection connection)
            : base(connection)
        {

        }



        public List<Models.MyCustomer> getList()
        {

            List<Models.MyCustomer> risultato;
            risultato = new List<Models.MyCustomer>();

            _strSQL = "SELECT * FROM CUSTOMER";

            _dt = _fillDataTable(_strSQL);

            foreach (DataRow row in _dt.Rows)
            {
                risultato.Add(new Models.MyCustomer(row));
            }

            return risultato;
        }


        public bool set(Models.MyUser u)
        {
            if (u.customerId == null)
            {
                return true;
            }

            Models.MyCustomer c;
            c = getCustomer(u.customerId.Value);

            if (c != null)
            {
                u.Customer = c;
            }


            return true;
        }


        public Models.MyCustomer getCustomer(long cutomerId)
        {
            _strSQL = "SELECT * FROM CUSTOMER WHERE customer_id = " + cutomerId;

            _dt = _fillDataTable(_strSQL);

            if (_dt.Rows.Count > 1)
            {
                throw new MyManagerCSharp.MyException("id > 0");
            }

            if (_dt.Rows.Count == 0)
            {
                return null;
            }

            Models.MyCustomer c;
            c = new Models.MyCustomer(_dt.Rows[0]);

            return c;

        }

        public long insert(Models.MyCustomer c)
        {
            long newId = -1;
            string strSQLParametri = "";

            _strSQL = "INSERT INTO CUSTOMER ( DATE_ADDED  ";
            strSQLParametri = " VALUES ( GetDate()  ";

            System.Data.Common.DbCommand command;
            command = _connection.CreateCommand();

            if (!String.IsNullOrEmpty(c.ragioneSociale))
            {
                _strSQL += ",RAGIONE_SOCIALE ";
                strSQLParametri += ", @RAGIONE_SOCIALE ";
                _addParameter(command, "@RAGIONE_SOCIALE", c.ragioneSociale);
            }


            command.CommandText = _strSQL + " ) " + strSQLParametri + " )";

            //If test_mode = True) {
            //    Me._transactionBegin()
            //    _executeNoQuery(command)
            //    Me._transactionRollback()
            //    Return -1
            //}

            _executeNoQuery(command);

            newId = _getIdentity();

            return newId;
        }



    }
}

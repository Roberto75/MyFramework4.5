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

            mStrSQL = "SELECT * FROM CUSTOMER";

            mDt = mFillDataTable(mStrSQL);

            foreach (DataRow row in mDt.Rows)
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
            mStrSQL = "SELECT * FROM CUSTOMER WHERE customer_id = " + cutomerId;

            mDt = mFillDataTable(mStrSQL);

            if (mDt.Rows.Count > 1)
            {
                throw new MyManagerCSharp.MyException("id > 0");
            }

            if (mDt.Rows.Count == 0)
            {
                return null;
            }

            Models.MyCustomer c;
            c = new Models.MyCustomer(mDt.Rows[0]);

            return c;

        }

        public long insert(Models.MyCustomer c)
        {
            long newId = -1;
            string strSQLParametri = "";

            mStrSQL = "INSERT INTO CUSTOMER ( DATE_ADDED  ";
            strSQLParametri = " VALUES ( GetDate()  ";

            System.Data.Common.DbCommand command;
            command = mConnection.CreateCommand();

            if (!String.IsNullOrEmpty(c.ragioneSociale))
            {
                mStrSQL += ",RAGIONE_SOCIALE ";
                strSQLParametri += ", @RAGIONE_SOCIALE ";
                mAddParameter(command, "@RAGIONE_SOCIALE", c.ragioneSociale);
            }


            command.CommandText = mStrSQL + " ) " + strSQLParametri + " )";

            //If test_mode = True) {
            //    Me.mTransactionBegin()
            //    mExecuteNoQuery(command)
            //    Me.mTransactionRollback()
            //    Return -1
            //}

            mExecuteNoQuery(command);

            newId = mGetIdentity();

            return newId;
        }



    }
}

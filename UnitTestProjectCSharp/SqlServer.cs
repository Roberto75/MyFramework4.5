using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyManagerCSharp;
using System.Diagnostics;
using System.Data;

namespace UnitTestProjectCSharp
{
    [TestClass]
    public class SqlServer
    {
        [TestMethod]
        public void Sloveno()
        {
            string sqlQuery;
            sqlQuery = "INSERT INTO NOTE ( NOTA ) VALUES ( @NOTA )";

            string nota;
            
            ManagerDB manager = new ManagerDB("DefaultConnection");
            manager.mOpenConnection();

            try
            {
                System.Data.Common.DbCommand command;
                command = manager.mGetConnection().CreateCommand();
                command.Connection = manager.mGetConnection();

                nota = "Dejavnosti Googlovo brezplačno storitev takoj prevede besede, stavke in spletne strani med italijansko in več kot 100 drugih jezikih. Italijanski, Afrikaans, albanski, amharščina, arabščina, ..";
                //nota = "CIAO città!";
                manager.mAddParameter(command, "@NOTA", nota);

                command.CommandText = sqlQuery;
                command.CommandType = CommandType.Text;

                manager.mExecuteNoQuery(command);



                sqlQuery = "SELECT * FROM NOTE";
                DataTable dt;
                dt = manager.mFillDataTable(sqlQuery);

                foreach (DataRow row  in dt.Rows)
                {
                    Debug.WriteLine(row["nota"]);
                }
                


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                manager.mCloseConnection();
            }







        }
    }
}

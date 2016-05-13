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
        public void Unicode()
        {
            string sqlQuery;


            //string notaNVARCHAR;
            //string notaVARCHAR
            string nota;

            ManagerDB manager = new ManagerDB("DefaultConnection");
            manager.mOpenConnection();

            try
            {
                //System.Data.Common.DbCommand command;
                //command = manager.mGetConnection().CreateCommand();
                //command.Connection = manager.mGetConnection();


                //nota = "CIAO città!";

                //sqlQuery = "INSERT INTO NOTE ( NOTA , NOTA2 ) VALUES ( N@NOTA , N@NOTA2 )";
                //manager.mAddParameter(command, "@NOTA", nota);
                //manager.mAddParameter(command, "@NOTA2", nota);

                //command.CommandText = sqlQuery;
                //command.CommandType = CommandType.Text;

                //manager.mExecuteNoQuery(command);


                //nota = "brezplačno jezikih.frikaans, albanski, amharščina, arabščina,  ..";
                //sqlQuery = "INSERT INTO NOTE ( NOTA , NOTA2 ) VALUES ( '" + nota + "', '" + nota + "' )";
                //manager.mExecuteNoQuery(sqlQuery);


                //sqlQuery = "INSERT INTO NOTE ( NOTA , NOTA2 ) VALUES ( N'" + nota + "',  N'" + nota + "' )";
                //manager.mExecuteNoQuery(sqlQuery);

                //nota = "città perchè però più ";
                //sqlQuery = "INSERT INTO NOTE ( NOTA , NOTA2 ) VALUES ( '" + nota + "', '" + nota + "' )";
                //manager.mExecuteNoQuery(sqlQuery);

                //sqlQuery = "INSERT INTO NOTE ( NOTA , NOTA2 ) VALUES ( N'" + nota + "', N'" + nota + "' )";
                //manager.mExecuteNoQuery(sqlQuery);


                nota = "الجنة هو اللث من ثلاثة أجزاء التي تالكوميديا ​​هية لدانتي أليغييري ، بعد الجحيم وم";
                sqlQuery = "INSERT INTO NOTE ( NOTA , NOTA2 ) VALUES ( N'" + nota + "', N'" + nota + "' )";
                //manager.mExecuteNoQuery(sqlQuery);


                nota = "天堂是三部分构成由但丁神曲地狱炼狱之后的第三次。在他的书信十三但丁他献给诗灿大斯卡拉。";
                sqlQuery = "INSERT INTO NOTE ( NOTA , NOTA2 ) VALUES ( N'" + nota + "', N'" + nota + "' )";
                manager.mExecuteNoQuery(sqlQuery);

                sqlQuery = "SELECT * FROM NOTE";
                DataTable dt;
                dt = manager.mFillDataTable(sqlQuery);

                foreach (DataRow row  in dt.Rows)
                {
                    Debug.WriteLine("NVARCHAR: " + row["nota"] + "  - VARCHAR: "+ row["nota2"]);
                    //Debug.WriteLine("NOTA 2" + row["nota2"]);
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

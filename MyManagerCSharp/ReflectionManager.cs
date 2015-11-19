using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace My.Shared
{
    public class ReflectionManager
    {
        //string strSQL="CREATE TABLE ";

        public static string toCreateTable(Object obj)
        {

            string strSQL = "";

            strSQL = String.Format("CREATE TABLE {0} ", obj.GetType().Name)+Environment.NewLine;

            PropertyInfo[] properties = getProperties(obj);


            //strSQL = "CREATE TABLE customers ( customer_id number(10) NOT NULL, customer_name varchar2(50) NOT NULL,  city varchar2(50));";
            bool primo = true;
            foreach (PropertyInfo property in properties)
            {
                if (!primo)
                {
                    strSQL += "," + Environment.NewLine ;
                }
                //Debug.WriteLine(property.Name+ " " + property.PropertyType);
                strSQL += property.Name + " "; //nome campo
                
                if (property.PropertyType == typeof(int))
                {
                    strSQL += "number ";
                }
                else if (property.PropertyType == typeof(string))
                {
                    strSQL += "varchar2 ";
                }
               
                
                primo = false;

            }
            strSQL += ");";
            Debug.WriteLine(strSQL);

            return strSQL;
        }


        public static PropertyInfo[] getProperties(Object val)
        {
            PropertyInfo[] pi;
            Type ty = val.GetType();
            
            pi = ty.GetProperties();

            return pi;
        }
    }

}
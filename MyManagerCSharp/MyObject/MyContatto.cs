using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyManagerCSharp.MyObject
{
    public class MyContatto :IComparable 
    {

        public string MyKey
        {
            get
            {
                string fullName;

                if (String.IsNullOrEmpty(Nome))
                {
                    fullName = Cognome;
                }
                else
                {
                    fullName = Nome + " " + Cognome;
                }

                if (String.IsNullOrEmpty(fullName))
                {
                    fullName = Societa;
                }

                return fullName;
            }
        }


        public string GoogleId { get; set; }
        public string OutlookId { get; set; }
        

        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string Societa { get; set; }

        public string Note { get; set; }
        public DateTime Compleanno { get; set; }
        public string Cellulare { get; set; }
        public string TelefonoCasa { get; set; }
        public string TelefonoUfficio { get; set; }
        public string EmailPersonale { get; set; }
        public string EmailUfficio { get; set; }
        public DateTime dateUpdated { get; set; }


        //Interface method
        public int CompareTo(object o)
        {
            MyObject.MyContatto c1 = (MyObject.MyContatto)o;

            if (this.MyKey == null)
            {
                return 1;
            }

            if (c1.MyKey == null)
            {
                return -1;
            }

            return this.MyKey.CompareTo(c1.MyKey);
        }



      


        // Nested class to do ascending sort on year property.
        public class CompareByMyKey : IComparer<MyContatto>
        {
            int IComparer<MyContatto>.Compare(MyObject.MyContatto c1, MyObject.MyContatto c2)
            {
                if (c1.MyKey == null)
                {
                    return 1;
                }


                if (c2.MyKey == null)
                {
                    return -1;
                }

                return c1.MyKey.CompareTo(c2.MyKey);
            }
        }


        public class CompareByCognome : IComparer<MyContatto>
        {
            int IComparer<MyContatto>.Compare(MyObject.MyContatto c1, MyObject.MyContatto c2)
            {
                if (c1.Cognome  == null)
                {
                    return 1;
                }


                if (c2.Cognome == null)
                {
                    return -1;
                }

                return c1.Cognome.CompareTo(c2.Cognome);
            }
        }

    }

        


    
}

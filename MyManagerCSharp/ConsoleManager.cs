using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyManagerCSharp
{
    public class ConsoleManager
    {
        public static string calcolaTempoDiElaborazione(DateTime dataIniziale, DateTime dataFinale)
        {

            string risultato = "";

            TimeSpan ts = dataFinale.Subtract(dataIniziale);

            if (ts == TimeSpan.Zero)
            {
                return "0";
            }

            if (ts.Days > 0)
            {
                risultato = String.Concat(ts.Days, "g ");
            }

            if (ts.Hours > 0)
            {
                risultato = String.Concat(risultato, ts.Hours, "h ");
            }

            if (ts.Minutes > 0)
            {
                risultato = String.Concat(risultato, ts.Minutes, "' ");
            }
            else
            {
                if (!String.IsNullOrEmpty(risultato) && ts.Seconds > 0)
                {
                    risultato = String.Concat(risultato, "0' ");
                }
            }


            if (ts.Seconds > 0)
            {
                risultato = String.Concat(risultato, ts.Seconds);
                if (ts.Milliseconds > 0)
                {
                    risultato = String.Concat(risultato, ",", ts.Milliseconds);
                }
                risultato = String.Concat(risultato, "''");
            }
            else
            {
                if (ts.Milliseconds > 0)
                {
                    risultato = String.Concat(risultato, "0,", ts.Milliseconds, "''");
                }
            }

            return risultato;

        }

    }
}

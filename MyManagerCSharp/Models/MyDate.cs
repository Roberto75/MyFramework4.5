using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyManagerCSharp.Models
{
    public class MyDate
    {
        protected  DateTime? _dataInizio;
        protected DateTime? _dataFine;
        protected ManagerDB.Days? _days;

        public bool DaysIsEnabled { get; set; }

        public ManagerDB.Days? Days
        {
            get
            {
                return _days;
            }

            set
            {
                _days = value;
            }
        }

        public DateTime? DataInizio
        {
            get
            {
                //if (_days == null)
                //{
                //    return _dataInizio;
                //}

                //return decodeDataInizio();

                return _dataInizio;
            }
            set
            {
             //   _days = null;
                _dataInizio = value;
            }

        }
        
        public DateTime? DataFine
        {
            get
            {

                return _dataFine;
                //if (_days == null)
                //{
                //    return _dataFine;
                //}

                //return decodeDataFine();
            }
            set
            {
            //    _days = null;
                _dataFine = value;
            }
        }
        
        public DateTime decodeDaysDataInizio()
        {
            if (_days == null)
            {
                throw new ArgumentNullException("Days");
            }

            DayOfWeek startOfWeek = DayOfWeek.Monday;  //Lunedì
            DateTime risultato = DateTime.MinValue;
            DateTime dataCorrente = DateTime.Now;

            switch ((ManagerDB.Days)_days)
            {

                case ManagerDB.Days.Tutti:
                    risultato = DateTime.MinValue ;
                    break;
                case ManagerDB.Days.Oggi:
                    risultato = dataCorrente;
                    break;
                case ManagerDB.Days.Ieri:
                    risultato = dataCorrente.AddDays(-1);
                    break;
                case ManagerDB.Days.Ultimi_7_giorni:
                    risultato = dataCorrente.AddDays(-6);
                    break;
                case ManagerDB.Days.Ultimi_15_giorni:
                    risultato = dataCorrente.AddDays(-14);
                    break;
                case ManagerDB.Days.Ultimi_30_giorni:
                    risultato = dataCorrente.AddDays(-29);
                    break;
                case ManagerDB.Days.Settimana_corrente:
                    //  Debug.WriteLine(dataCorrente.DayOfWeek);

                    int diff = dataCorrente.DayOfWeek - startOfWeek;
                    if (diff < 0)
                    {
                        diff += 7;
                    }
                    DateTime inizioSettimana;
                    inizioSettimana = dataCorrente.AddDays(-1 * diff).Date;

                    // strWHERE += getWhereConditionByDate(queryField, inizioSettimana, inizioSettimana.AddDays(6));
                    risultato = inizioSettimana;
                    break;
                case ManagerDB.Days.Settimana_precedente:
                    // Debug.WriteLine(dataCorrente.DayOfWeek);

                    diff = dataCorrente.DayOfWeek - startOfWeek;
                    if (diff < 0)
                    {
                        diff += 7;
                    }

                    inizioSettimana = dataCorrente.AddDays(-1 * diff).Date;

                    // strWHERE += getWhereConditionByDate(queryField, inizioSettimana.AddDays(-7), inizioSettimana.AddDays(-1));

                    risultato = inizioSettimana.AddDays(-7);
                    break;
                case ManagerDB.Days.Mese_corrente:
                    // strWHERE += String.Format(" AND (YEAR({0}) = YEAR(GetDate()) ) AND ( MONTH({0}) = MONTH( GetDate() ) )", queryField);
                    risultato = new DateTime(dataCorrente.Year, dataCorrente.Month, 1);
                    break;
                case ManagerDB.Days.Mese_precedente:
                    // strWHERE += String.Format(" AND (  (YEAR({0})*12 + MONTH({0}) ) =  (YEAR( GetDate() )  * 12 + MONTH( GetDate() ) -1 ) )", queryField);
                    risultato = new DateTime(dataCorrente.Year, dataCorrente.Month, 1).AddMonths(-1);
                    break;
                case ManagerDB.Days.Anno_corrente:
                    risultato = new DateTime(dataCorrente.Year, 1, 1);
                    // strWHERE += String.Format(" AND ( YEAR({0}) = YEAR(GetDate()) )", queryField);
                    break;
                case ManagerDB.Days.Anno_precedente:
                    //strWHERE += String.Format(" AND ( YEAR({0}) = YEAR(GetDate()) - 1 )", queryField);
                    risultato = new DateTime(dataCorrente.Year - 1, 1, 1);
                    break;
                case ManagerDB.Days.Primo_semestre_anno_corrente:
                    //strWHERE += String.Format(" AND ( YEAR({0}) = YEAR(GetDate())  AND  MONTH({0}) between 1 and 6 ) ", queryField);
                    risultato = new DateTime(dataCorrente.Year, 1, 1);
                    break;
                case ManagerDB.Days.Primo_semestre_anno_precedente:
                    //strWHERE += String.Format(" AND ( YEAR({0}) = YEAR(GetDate()) -1  AND MONTH({0}) between 1 and 6 ) ", queryField);
                    risultato = new DateTime(dataCorrente.Year - 1, 1, 1);
                    break;
                case ManagerDB.Days.Secondo_semestre_anno_corrente:
                    risultato = new DateTime(dataCorrente.Year, 7, 1);
                    //strWHERE += String.Format(" AND ( YEAR({0}) = YEAR(GetDate())  AND MONTH({0}) between 7 and 12 ) ", queryField);
                    break;
                case ManagerDB.Days.Secondo_semestre_anno_precedente:
                    risultato = new DateTime(dataCorrente.Year - 1, 7, 1);
                    //strWHERE += String.Format(" AND ( YEAR({0}) = YEAR(GetDate()) -1  AND  MONTH({0}) between 7 and 12 ) ", queryField);
                    break;
                case ManagerDB.Days.Ultimo_semestre:
                    // strWHERE += String.Format(" AND ( Year({0})* 12 + MONTH({0}) >= Year(GetDate())* 12 + MONTH( GetDate()) - 6   AND  Year({0})* 12 + MONTH({0}) <= Year(GetDate())* 12 + MONTH(GetDate())  ) ", queryField);
                    risultato = dataCorrente.AddMonths(-5);
                    risultato = new DateTime(risultato.Year, risultato.Month, 1);
                    break;
            }

            return risultato;

        }

        public DateTime decodeDaysDataFine()
        {
            if (_days == null)
            {
                throw new ArgumentNullException("Days");
            }

            DayOfWeek startOfWeek = DayOfWeek.Monday;  //Lunedì
            DateTime risultato = DateTime.MinValue;
            DateTime dataCorrente = DateTime.Now;

            switch ((ManagerDB.Days)_days)
            {

                case ManagerDB.Days.Tutti:
                    risultato = dataCorrente;
                    break;
                case ManagerDB.Days.Oggi:
                    risultato = dataCorrente;
                    break;
                case ManagerDB.Days.Ieri:
                    risultato = dataCorrente.AddDays(-1);
                    break;
                case ManagerDB.Days.Ultimi_7_giorni:
                    risultato = dataCorrente;
                    break;
                case ManagerDB.Days.Ultimi_15_giorni:
                    risultato = dataCorrente;
                    break;
                case ManagerDB.Days.Ultimi_30_giorni:
                    risultato = dataCorrente;
                    break;
                case ManagerDB.Days.Settimana_corrente:
                    //Debug.WriteLine(dataCorrente.DayOfWeek);

                    int diff = dataCorrente.DayOfWeek - startOfWeek;
                    if (diff < 0)
                    {
                        diff += 7;
                    }
                    DateTime inizioSettimana;
                    inizioSettimana = dataCorrente.AddDays(-1 * diff).Date;

                    // strWHERE += getWhereConditionByDate(queryField, inizioSettimana, inizioSettimana.AddDays(6));
                    risultato = inizioSettimana.AddDays(6);
                    break;
                case ManagerDB.Days.Settimana_precedente:
                    //Debug.WriteLine(dataCorrente.DayOfWeek);

                    diff = dataCorrente.DayOfWeek - startOfWeek;
                    if (diff < 0)
                    {
                        diff += 7;
                    }

                    inizioSettimana = dataCorrente.AddDays(-1 * diff).Date;

                    // strWHERE += getWhereConditionByDate(queryField, inizioSettimana.AddDays(-7), inizioSettimana.AddDays(-1));

                    risultato = inizioSettimana.AddDays(-1);
                    break;
                case ManagerDB.Days.Mese_corrente:
                    // strWHERE += String.Format(" AND (YEAR({0}) = YEAR(GetDate()) ) AND ( MONTH({0}) = MONTH( GetDate() ) )", queryField);
                    risultato = dataCorrente.AddMonths(1); // mese successivo
                    risultato = new DateTime(risultato.Year, risultato.Month, 1); // il primo del mese succcessivo
                    risultato = risultato.AddDays(-1); // ultimo giorno del mese corrente!
                    break;
                case ManagerDB.Days.Mese_precedente:
                    // strWHERE += String.Format(" AND (  (YEAR({0})*12 + MONTH({0}) ) =  (YEAR( GetDate() )  * 12 + MONTH( GetDate() ) -1 ) )", queryField);
                    risultato = new DateTime(dataCorrente.Year, dataCorrente.Month, 1).AddDays(-1);
                    break;
                case ManagerDB.Days.Anno_corrente:
                    risultato = new DateTime(dataCorrente.Year, 12, 31);
                    // strWHERE += String.Format(" AND ( YEAR({0}) = YEAR(GetDate()) )", queryField);
                    break;
                case ManagerDB.Days.Anno_precedente:
                    //strWHERE += String.Format(" AND ( YEAR({0}) = YEAR(GetDate()) - 1 )", queryField);
                    risultato = new DateTime(dataCorrente.Year - 1, 12, 31);
                    break;
                case ManagerDB.Days.Primo_semestre_anno_corrente:
                    //strWHERE += String.Format(" AND ( YEAR({0}) = YEAR(GetDate())  AND  MONTH({0}) between 1 and 6 ) ", queryField);
                    risultato = new DateTime(dataCorrente.Year, 6, 30);
                    break;
                case ManagerDB.Days.Primo_semestre_anno_precedente:
                    //strWHERE += String.Format(" AND ( YEAR({0}) = YEAR(GetDate()) -1  AND MONTH({0}) between 1 and 6 ) ", queryField);
                    risultato = new DateTime(dataCorrente.Year - 1, 6, 30);
                    break;
                case ManagerDB.Days.Secondo_semestre_anno_corrente:
                    risultato = new DateTime(dataCorrente.Year, 7, 1);
                    //strWHERE += String.Format(" AND ( YEAR({0}) = YEAR(GetDate())  AND MONTH({0}) between 7 and 12 ) ", queryField);
                    break;
                case ManagerDB.Days.Secondo_semestre_anno_precedente:
                    risultato = new DateTime(dataCorrente.Year - 1, 12, 31);
                    break;
                case ManagerDB.Days.Ultimo_semestre:
                    risultato = new DateTime(dataCorrente.Year, dataCorrente.Month + 1, 1);
                    risultato = risultato.AddDays(-1); //ultimo giorno del mese corrente
                    break;
            }

            return risultato;

        }

        public override string ToString()
        {
            if (_dataInizio == null)
            {
                return "Data inizio == NULL";
            }


            string temp;
            temp = String.Format("{0} - {1}", DataInizio.Value.ToShortDateString(), DataFine.Value.ToShortDateString());

            if (_days != null)
            {

                temp = " [Days: " + _days.Value + "] \t" + temp;
            }

            return temp;
        }

    }
}

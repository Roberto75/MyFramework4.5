using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annunci
{
    public class AnnunciAdminManager : AnnunciManager
    {


        private const string _sqlElencoAnnunci = "SELECT UTENTI.my_login, UTENTI.user_id, ANNUNCIO.annuncio_id, ANNUNCIO.MY_STATO " +
            ", ANNUNCIO.REGIONE, ANNUNCIO.PROVINCIA, ANNUNCIO.COMUNE , ANNUNCIO.REGIONE_ID, ANNUNCIO.PROVINCIA_ID, ANNUNCIO.COMUNE_ID  " +
            ", ANNUNCIO.DESCRIZIONE, ANNUNCIO.DATE_LAST_CLICK, ANNUNCIO.date_start_click_parziale, ANNUNCIO.COUNT_CLICK, ANNUNCIO.COUNT_CLICK_PARZIALE" +
            ", ANNUNCIO.DATE_DELETED " +
            ", ANNUNCIO.date_added " +
            ", ANNUNCIO.tipo, ANNUNCIO.nome AS nome, ANNUNCIO.prezzo, categorie.nome AS categoria, categorie.categoria_id, Switch(tipo=1,'Vendo',tipo=2,'Compro',tipo=3,'Scambio') AS tipo_desc " +
            " FROM categorie INNER JOIN (ANNUNCIO LEFT JOIN UTENTI ON ANNUNCIO.fk_user_id=UTENTI.user_id) ON categorie.categoria_id=ANNUNCIO.fk_categoria_id";



        public AnnunciAdminManager(string connectionName)
            : base(connectionName)
        {

        }

        public AnnunciAdminManager(System.Data.Common.DbConnection connection)
            : base(connection)
        {

        }

        //per la parte di AMMINISTRAZIONE
        public List<Annunci.Models.Trattativa> getTrattativeByAnnuncio(long annuncioId, Models.Trattativa.TipoTrattativa tipo)
        {


            mStrSQL = "SELECT TRATTATIVA.date_added as date_added , TRATTATIVA.trattativa_id , 'dummy' as categoria " +
                ",ANNUNCIO.annuncio_id, ANNUNCIO.prezzo, ANNUNCIO.tipo, ANNUNCIO.nome,  UTENTI.user_id,  UTENTI.my_login, TRATTATIVA.stato" +
       " FROM " +
        " (TRATTATIVA INNER JOIN UTENTI ON TRATTATIVA.fk_user_id=UTENTI.user_id) INNER JOIN ANNUNCIO ON ANNUNCIO.annuncio_id=TRATTATIVA.fk_annuncio_id" +
        " WHERE  TRATTATIVA.DATE_DELETED_OWNER IS NULL AND FK_ANNUNCIO_ID= " + annuncioId +
       " order BY TRATTATIVA.date_added DESC ";

            List<Models.Trattativa> risultato;
            risultato = new List<Models.Trattativa>();

            mDt = mFillDataTable(mStrSQL);

            Models.Trattativa trattativa;

            foreach (System.Data.DataRow row in mDt.Rows)
            {
                trattativa = new Models.Trattativa(row, tipo);

                risultato.Add(trattativa);
            }

            return risultato;
        }



        public List<Annunci.Models.Annuncio> getListAnnunciByUserId(long userId)
        {

            mStrSQL = _sqlElencoAnnunci;
            string strWHERE = " WHERE ANNUNCIO.fk_user_id = " + userId;


            mStrSQL += strWHERE;
            mStrSQL += " ORDER BY ANNUNCIO.nome desc";
            mDt = mFillDataTable(mStrSQL);

            List<Models.Annuncio> risultato;
            risultato = new List<Models.Annuncio>();
            foreach (DataRow row in mDt.Rows)
            {
                risultato.Add(new Models.Annuncio(row, Models.Annuncio.SelectFileds.Full));
            }

            return risultato;
        }


        public long countAnnunciByStato(StatoAnnuncio stato)
        {
            return countAnnunciByStato(stato, -1);
        }
        public long countAnnunciByStato(StatoAnnuncio stato, long userId)
        {
            mStrSQL = "SELECT COUNT(*) FROM ANNUNCIO WHERE MY_STATO = '" + stato.ToString() + "'";

            if (userId != -1)
            {
                mStrSQL += " AND fk_user_id = " + userId;
            }
            return long.Parse(mExecuteScalar(mStrSQL));
        }

        public System.Collections.Hashtable countAnnunciByStato()
        {
            return countAnnunciByStato(-1);
        }

        public System.Collections.Hashtable countAnnunciByStato(long userId)
        {
            System.Collections.Hashtable risultato = new System.Collections.Hashtable();

            foreach (var value in Enum.GetValues(typeof(StatoAnnuncio)))
            {
                risultato.Add((StatoAnnuncio)value, countAnnunciByStato((StatoAnnuncio)value, userId));
            }

            return risultato;
        }



        public System.Collections.Hashtable countTrattativeByStato()
        {
            TrattativaManager m = new TrattativaManager(mConnection);
            return m.countTrattativeByStato();
        }

        public System.Collections.Hashtable countTrattativeByStato(long userId)
        {
            TrattativaManager m = new TrattativaManager(mConnection);
            return m.countTrattativeByStato(userId);
        }




        public Models.Annuncio getAnnuncio(long id)
        {
            System.Data.Common.DbCommand command;
            command = mConnection.CreateCommand();

            mStrSQL = "select ANNUNCIO.* , 'dummy' as categoria , ANNUNCIO.fk_categoria_id as categoria_id , UTENTI.my_login AS my_login, UTENTI.user_id AS user_id from ANNUNCIO " +
                " LEFT JOIN UTENTI ON ANNUNCIO.fk_user_id = UTENTI.user_id " +
                " WHERE annuncio_id = @ID ";

            mAddParameter(command, "@ID", id);
            command.CommandText = mStrSQL;

            command.CommandTimeout = 60;

            mDt = mFillDataTable(command);


            if (mDt.Rows.Count == 0)
            {
                return null;
            }


            if (mDt.Rows.Count > 1)
            {
                throw new MyManagerCSharp.MyException("id > 0");
            }

            Models.Annuncio annuncio = new Models.Annuncio(mDt.Rows[0], Models.Annuncio.SelectFileds.Full);

            return annuncio;
        }


    }
}

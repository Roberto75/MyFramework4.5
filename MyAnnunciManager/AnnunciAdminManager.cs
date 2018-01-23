using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annunci
{
    public class AnnunciAdminManager : AnnunciManager
    {

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

      








        }
}

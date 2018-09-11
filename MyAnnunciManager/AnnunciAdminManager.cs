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


        public bool checkUserExists(long userId)
        {
            mStrSQL = "SELECT COUNT(*)  FROM UTENTI WHERE USER_ID = " + userId;

            return int.Parse(mExecuteScalar(mStrSQL)) == 1;
        }

        public Models.MyUser getUtenteByExternalId(string externalId, Models.Immobile.TipoSourceId? sourceId)
        {
            return getUtenteByExternalId(externalId, sourceId, -1);
        }


        public Models.MyUser getUtenteByExternalId(string externalId, Models.Immobile.TipoSourceId? sourceId, long excludeUserId)
        {
            mStrSQL = " SELECT * FROM UTENTI LEFT  join source on utenti.source_id = source.source_id  " +
                  " WHERE EXTERNAL_ID = '" + externalId + "' ";

            if (sourceId != null)
            {
                mStrSQL += " AND utenti.SOURCE_ID = " + (int)sourceId;
            }
            if (excludeUserId != -1)
            {
                //escludo se stesso
                mStrSQL += " AND user_id <> " + excludeUserId;
            }



            mDt = mFillDataTable(mStrSQL);


            if (mDt.Rows.Count == 0)
            {
                return null;
            }

            if (mDt.Rows.Count > 1)
            {
                throw new MyManagerCSharp.MyException("id > 1");
            }

            Models.MyUser u;
            u = new Models.MyUser(mDt.Rows[0]);

            return u;
        }


        public bool update(Models.MyUser u)
        {

            mStrSQL = "UPDATE UTENTI SET DATE_MODIFIED = GetDate()  ";

            System.Data.Common.DbCommand command;
            command = mConnection.CreateCommand();


            if (u.sourceId == null || u.sourceId == Models.Immobile.TipoSourceId.Undefined)
            {
                mStrSQL += " ,EXTERNAL_ID = NULL , SOURCE_ID = NULL ";
            }
            else
            {
                mStrSQL += " ,EXTERNAL_ID = @EXTERNAL_ID , SOURCE_ID =" + (int)u.sourceId;
                mAddParameter(command, "@EXTERNAL_ID", u.externalId);
            }

            if (u.statoId == Models.Immobile.StatoUtente.Undefined)
            {
                mStrSQL += ",STATO_ID = NULL , DATE_STATO = NULL ";
            }
            else
            {
                //veifico che lo stato precedente sia diverso!

                Models.Immobile.StatoUtente statoPrecedente;

                statoPrecedente = getStatoUtente((long)u.userId);

                if (statoPrecedente != u.statoId)
                {
                    mStrSQL += ",STATO_ID = " + (int)u.statoId + " , DATE_STATO = GetDate() ";
                }
            }


            if (String.IsNullOrEmpty(u.nota))
            {
                mStrSQL += ",NOTA = NULL ";
            }
            else
            {
                mStrSQL += ",NOTA = @NOTA ";
                mAddParameter(command, "@NOTA", u.nota);
            }


            mStrSQL += " WHERE USER_ID=" + u.userId;

            command.CommandText = mStrSQL;

            mExecuteNoQuery(command);

            return true;
        }


        Models.Immobile.StatoUtente getStatoUtente(long userId)
        {
            //uso temp invece di mStrSQL in quanto uso questa funzione all'inerno del manager e qundi mi va a modificare la 
            // query originale
            string temp = "SELECT STATO_ID FROM UTENTI WHERE USER_ID = " + userId;

            string stato;
            stato = mExecuteScalar(temp);

            if (String.IsNullOrEmpty(stato))
            {
                return Models.Immobile.StatoUtente.Undefined;
            }

            return (Models.Immobile.StatoUtente)Enum.Parse(typeof(Models.Immobile.StatoUtente), stato);
        }



        public List<Models.MyUser> getUsers(out int totalRecords, Models.MyUser filter, int pageSize = -1, int pageIndex = -1, string sort = "MY_LOGIN", string sortOrder = "ASC")
        {
            List<Models.MyUser> risultato;
            risultato = new List<Models.MyUser>();

            mStrSQL = "SELECT *  from utenti  LEFT  join source on utenti.source_id = source.source_id  ";

            System.Data.Common.DbCommand command;
            command = mConnection.CreateCommand();



            string strWHERE = "";
            if (filter != null)
            {
                if (filter.userId != null)
                {
                    strWHERE += " AND user_id = " + filter.userId;
                }

                if (!String.IsNullOrEmpty(filter.nome))
                {
                    strWHERE += " AND UCASE(nome) like  @NOME";
                    mAddParameter(command, "@NOME", "%" + filter.nome.ToUpper().Trim() + "%");
                }

                if (!String.IsNullOrEmpty(filter.cognome))
                {
                    strWHERE += " AND UCASE(cognome) like  @COGNOME";
                    mAddParameter(command, "@COGNOME", "%" + filter.cognome.ToUpper().Trim() + "%");
                }

                if (!String.IsNullOrEmpty(filter.email))
                {
                    strWHERE += " AND UCASE(email) like  @EMAIL";
                    mAddParameter(command, "@EMAIL", "%" + filter.email.ToUpper().Trim() + "%");
                }

                if (!String.IsNullOrEmpty(filter.login))
                {
                    strWHERE += " AND UCASE(my_login) like  @MY_LOGIN";
                    mAddParameter(command, "@MY_LOGIN", "%" + filter.login.ToUpper().Trim() + "%");
                }

                if (!String.IsNullOrEmpty(filter.externalId))
                {
                    strWHERE += " AND UCASE(EXTERNAL_ID) like  @EXTERNAL_ID";
                    mAddParameter(command, "@EXTERNAL_ID", "%" + filter.externalId.Trim() + "%");
                }

                if (filter.sourceId != null && filter.sourceId != Models.Immobile.TipoSourceId.Undefined)
                {
                    strWHERE += " AND utenti.source_id  = " + (int)filter.sourceId;
                }

                if (filter.statoId != null && filter.statoId != Models.Immobile.StatoUtente.Undefined)
                {
                    strWHERE += " AND utenti.stato_id  = " + (int)filter.statoId;
                }

            }


            if (!String.IsNullOrEmpty(strWHERE))
            {
                mStrSQL += " WHERE (1=1) " + strWHERE;
            }


            mStrSQL += " ORDER BY " + sort + " " + sortOrder;


            command.CommandText = mStrSQL;

            mDt = mFillDataTable(command);

            totalRecords = mDt.Rows.Count;


            if (pageSize > 0 && pageIndex >= 0)
            {
                // apply paging
                IEnumerable<DataRow> rows = mDt.AsEnumerable().Skip((pageIndex - 1) * pageSize).Take(pageSize);
                foreach (DataRow row in rows)
                {
                    risultato.Add(new Models.MyUser(row));
                }
            }
            else
            {
                foreach (DataRow row in mDt.Rows)
                {
                    risultato.Add(new Models.MyUser(row));
                }
            }

            return risultato;
        }


        public Models.MyUser getUser(long id)
        {
            mStrSQL = "SELECT * FROM UTENTI LEFT  join source on utenti.source_id = source.source_id WHERE user_id = " + id;

            mDt = mFillDataTable(mStrSQL);

            if (mDt.Rows.Count > 1)
            {
                throw new MyManagerCSharp.MyException("id > 1");
            }

            Models.MyUser u;
            u = new Models.MyUser(mDt.Rows[0]);

            return u;

        }



        public Models.Immobile getLastImmobilePublisched(long userId)
        {
            //restoituisce l'ultimo annuncio pubblicato dall'utente
            //mi serve per capire da quanto tempo lavora....



            mStrSQL = "SELECT TOP 1 ANNUNCIO.*,  " + userId + " as user_id , 'dummy' as my_login "  +
                " , 'dummy' as categoria " +
                " ,ANNUNCIO.fk_categoria_id as categoria_id " +
                " , -1 as customer_id " +
                " from ANNUNCIO ";


            //mStrSQL = "select ANNUNCIO.* , 'dummy' as categoria , ANNUNCIO.fk_categoria_id as categoria_id , UTENTI.my_login AS my_login, UTENTI.user_id AS user_id from ANNUNCIO " +
            //" LEFT JOIN UTENTI ON ANNUNCIO.fk_user_id = UTENTI.user_id " +
            //" WHERE ANNUNCIO.fk_user_id = " + userId;
            //+ " AND ANNUNCIO.date_added is NULL" +
            //" ORDER BY ANNUNCIO.date_added desc " ;


            //  mStrSQL = _sqlElencoAnnunci.Replace("SELECT ", "SELECT TOP 1 ");
            //mStrSQL = _sqlElencoAnnunci;
            string strWHERE = " WHERE ANNUNCIO.fk_user_id = " + userId + " AND ANNUNCIO.date_deleted is NULL";
            mStrSQL = mStrSQL + strWHERE + " ORDER BY ANNUNCIO.date_added desc";
            mDt = mFillDataTable(mStrSQL);

            Models.Immobile lastAnnuncio;

            if (mDt.Rows.Count == 0)
            {
                return null;
            }

            lastAnnuncio = new Models.Immobile(mDt.Rows[0], Models.Immobile.SelectFileds.Full);

            return lastAnnuncio;


        }


    }
}

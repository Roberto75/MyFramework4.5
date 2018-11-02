using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MyUsers;
using System.Diagnostics;

namespace Annunci
{
    public class ImmobiliareManager : Annunci.AnnunciManager
    {


        //public enum Source
        //{
        //    Undefined = -1,
        //    RevoAgent
        //}

        //public enum Stato
        //{
        //    Undefined = -1,
        //    Active = 1,
        //    Test = 2,
        //    Disabled = 3
        //}


        private const string _sqlElencoImmobili = "SELECT UTENTI.my_login, UTENTI.user_id, UTENTI.customer_id, ANNUNCIO.annuncio_id, ANNUNCIO.regione, ANNUNCIO.provincia, ANNUNCIO.MQ, ANNUNCIO.piano, ANNUNCIO.stato, ANNUNCIO.CAP, ANNUNCIO.COMUNE, ANNUNCIO.terrazzi, ANNUNCIO.indirizzo, ANNUNCIO.posto_auto" +
            ", ANNUNCIO.cantina, ANNUNCIO.cucina, ANNUNCIO.box, ANNUNCIO.salone, ANNUNCIO.camere_da_letto, ANNUNCIO.camerette, ANNUNCIO.bagni, ANNUNCIO.classe_energetica, ANNUNCIO.quartiere, ANNUNCIO.latitude, ANNUNCIO.longitude " +
            ", FORMAT (ANNUNCIO.date_added,\"dd-MM-yyyy\") as date_added " +
            ", FORMAT (ANNUNCIO.date_added,\"yyyyMMdd\") as date_ordered " +
            ", ANNUNCIO.tipo, ANNUNCIO.prezzo, categorie.nome AS categoria, categorie.categoria_id" +
            " FROM categorie INNER JOIN (ANNUNCIO LEFT JOIN UTENTI ON ANNUNCIO.fk_user_id=UTENTI.user_id) ON categorie.categoria_id=ANNUNCIO.fk_categoria_id";
        //LA CONDIZIONE WHERE E' ESCLUSA


        public ImmobiliareManager(string connectionName)
            : base(connectionName)
        {

        }


/*
        public List<Models.Immobile> getList()
        {
            List<Models.Immobile> risultato;
            risultato = new List<Models.Immobile>();

            mStrSQL = _sqlElencoImmobili;
            mStrSQL += " WHERE ANNUNCIO.date_deleted Is Null ";

            mDt = mFillDataTable(mStrSQL);

            foreach (DataRow row in mDt.Rows)
            {
                risultato.Add(new Models.Immobile(row, Models.Immobile.SelectFileds.Lista));
            }

            return risultato;
        }
        */


        public void getList(Models.SearchImmobili model)
        {

            List<Models.Immobile> risultato;
            risultato = new List<Models.Immobile>();

            mStrSQL = _sqlElencoImmobili;
          
            System.Data.Common.DbCommand command;
            command = mConnection.CreateCommand();


            string strWHERE = " WHERE ANNUNCIO.date_deleted Is Null";

            if (model.filter != null)
            {

                switch (model.Tempo)
                {
                    case Models.SearchImmobili.EnumTempo.Oggi:
                        //strWHERE += "AND FORMAT (ANNUNCIO.date_added,\"yyyy-MM-dd\") = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
                        strWHERE += String.Format(" AND (DAY({0})={1} AND  MONTH({0})={2} AND YEAR({0})={3}) ", "ANNUNCIO.date_added", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year);
                        break;
                    case Models.SearchImmobili.EnumTempo.UltimaSettimana:
                        strWHERE += "AND ( FORMAT (ANNUNCIO.date_added,\"yyyy-MM-dd\") <= '" + DateTime.Now.ToString("yyyy-MM-dd") + "' AND  FORMAT (ANNUNCIO.date_added,\"yyyy-MM-dd\") >= '" + DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd") + "')";
                        break;
                    case Models.SearchImmobili.EnumTempo.UltimoMese:
                        strWHERE += "AND ( FORMAT (ANNUNCIO.date_added,\"yyyy-MM-dd\") <= '" + DateTime.Now.ToString("yyyy-MM-dd") + "' AND  FORMAT (ANNUNCIO.date_added,\"yyyy-MM-dd\") >= '" + DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd") + "')";
                        break;
                }


                if (model.filter.regioneId != null && model.filter.regioneId != -1 && model.filter.regioneId != 0)
                {
                    strWHERE += " AND regione_id = " + model.filter.regioneId;
                }

                if (!String.IsNullOrEmpty(model.filter.provinciaId) && model.filter.provinciaId != "-1" && model.filter.provinciaId != "---")
                {
                    strWHERE += " AND provincia_id = @PROVINCIA_ID ";
                    mAddParameter(command, "@PROVINCIA_ID", model.filter.provinciaId);
                }

                if (!String.IsNullOrEmpty(model.filter.comuneId) && model.filter.comuneId != "-1" && model.filter.comuneId != "---")
                {
                    strWHERE += " AND comune_id =  @COMUNE_ID ";
                    mAddParameter(command, "@COMUNE_ID", model.filter.comuneId);
                }


                if (model.filter.immobile != null && model.filter.immobile > 0)
                {
                    strWHERE += " AND tipo = '" + model.filter.immobile.ToString() + "'";
                }

                if (model.filter.categoria != null && model.filter.categoria > 0)
                {
                    strWHERE += " AND categoria_id = " + ((int)model.filter.categoria);
                }


                if (model.TipoAnnuncio != null && model.TipoAnnuncio.Count < 2)
                {

                    if (model.TipoAnnuncio.Contains(Models.SearchImmobili.EnumTipoAnnuncio.Agenzia))
                    {
                        strWHERE += " AND (UTENTI.customer_id IS NOT NULL ) ";
                    }

                    if (model.TipoAnnuncio.Contains(Models.SearchImmobili.EnumTipoAnnuncio.Privato))
                    {
                        strWHERE += " AND (UTENTI.customer_id IS NULL ) ";
                    }

                }

                
                if (model.filter.ascensore != null && model.filter.ascensore != -1)
                {
                    strWHERE += " AND ascensore = " + model.filter.ascensore;
                }

                //16/01/2014
                if (model.prezzoMax != null && model.prezzoMax != 0)
                {
                    strWHERE += " AND prezzo <= " + model.prezzoMax;
                }

                //16/01/2014
                if (model.mqMin != null && model.mqMin != 0)
                {
                    strWHERE += " AND mq >= " + model.mqMin;
                }


            }

            if (!String.IsNullOrEmpty(strWHERE))
            {
                mStrSQL += strWHERE;
            }




            if (!String.IsNullOrEmpty(model.Sort))
            {
                string sortField = getSortField(model.Sort);


                if (model.SortDir.ToUpper().Trim() != "ASC" && model.SortDir.ToUpper().Trim() != "DESC")
                {
                    model.SortDir = "ASC";
                }
                Debug.WriteLine("ORDER BY " + sortField + " " + model.SortDir);

                mStrSQL += " ORDER BY " + sortField + " " + model.SortDir;
            }


        


            command.CommandText = mStrSQL;

            mDt = mFillDataTable(command);

            model.TotalRows = mDt.Rows.Count;


            if (model.PageSize > 0 && model.PageNumber >= 0)
            {
                // apply paging
                IEnumerable<DataRow> rows = mDt.AsEnumerable().Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize);
                foreach (DataRow row in rows)
                {
                    risultato.Add(new Models.Immobile(row, Models.Immobile.SelectFileds.Lista));
                }
            }
            else
            {
                foreach (DataRow row in mDt.Rows)
                {
                    risultato.Add(new Models.Immobile(row, Models.Immobile.SelectFileds.Lista));
                }
            }
            model.Immobili= risultato;
        }


        private string getSortField(string modelSort)
        {
            string sortField = "";

            switch (modelSort)
            {
                
                case "dataInserimento":
                    sortField = "ANNUNCIO.DATE_ADDED";
                    break;
                
                default:
                    sortField = modelSort;
                    break;
            }

            return sortField;
        }

        public Models.Immobile getImmobile(long id)
        {
            mStrSQL = "SELECT UTENTI.my_login AS my_login, UTENTI.user_id AS user_id, UTENTI.isModeratore AS isModeratore " +
                     ", ANNUNCIO.*  " +
                     ", categorie.nome AS categoria " +
                     ", categorie.categoria_id as categoria_id " +
                     ", UTENTI.customer_id AS customer_id " +
                     " FROM categorie INNER JOIN (ANNUNCIO LEFT JOIN UTENTI ON ANNUNCIO.fk_user_id=UTENTI.user_id) ON categorie.categoria_id=ANNUNCIO.fk_categoria_id " +
                     " WHERE ( ANNUNCIO.date_deleted Is Null) And ANNUNCIO.ANNUNCIO_ID = " + id;

            mDt = mFillDataTable(mStrSQL);

            if (mDt.Rows.Count == 0)
            {
                return null;
            }

            if (mDt.Rows.Count > 1)
            {
                throw new MyManagerCSharp.MyException("id > 0");
            }

            Models.Immobile i;
            i = new Models.Immobile(mDt.Rows[0], Models.Immobile.SelectFileds.Full);

            return i;
        }

        public List<Models.Immobile> getMap()
        {
            List<Models.Immobile> risultato;
            risultato = new List<Models.Immobile>();

            mStrSQL = _sqlElencoImmobili;
            mStrSQL += " WHERE ANNUNCIO.date_deleted Is Null ";
            mStrSQL += " AND latitude > 0 AND longitude > 0";

            mDt = mFillDataTable(mStrSQL);

            foreach (DataRow row in mDt.Rows)
            {
                risultato.Add(new Models.Immobile(row, Models.Immobile.SelectFileds.Lista));
            }

            return risultato;


        }


              



        public System.Data.DataTable getPhoto(long annuncioId)
        {
            mStrSQL = "SELECT * FROM PHOTO WHERE  FK_EXTERNAL_ID= " + annuncioId;
            return mFillDataTable(mStrSQL);
        }

             



        public int countAnnunci(long userId)
        {
            mStrSQL = "SELECT count(*) from  ANNUNCIO where  ANNUNCIO.fk_user_id = " + userId;
            return int.Parse(mExecuteScalar(mStrSQL));
        }
    /*    public System.Collections.Hashtable countAnnunciByStato()
        {
            AnnunciManager m = new AnnunciManager (mConnection);
            return m.countAnnunciByStato();
        }

        public System.Collections.Hashtable countAnnunciByStato(long userId)
        {
            AnnunciManager m = new AnnunciManager(mConnection);
            return m.countAnnunciByStato(userId);
        }
        */



        public List<Models.Immobile> getListAnnunci(long userId)
        {



            List<Models.Immobile> risultato;
            risultato = new List<Models.Immobile>();

            //Debug
            //userId = 567809036;

            //            mStrSQL = "SELECT UTENTI.my_login, UTENTI.user_id, ANNUNCIO.annuncio_id, FORMAT(ANNUNCIO.date_added,\"dd-MM-yyyy\") AS date_added, ANNUNCIO.tipo, ANNUNCIO.prezzo,  ANNUNCIO.source_id, categorie.nome AS categoria, categorie.categoria_id " +


            //mStrSQL = "SELECT UTENTI.my_login, UTENTI.user_id, UTENTI.customer_id, ANNUNCIO.annuncio_id, ANNUNCIO.regione, ANNUNCIO.provincia, ANNUNCIO.MQ, ANNUNCIO.piano, ANNUNCIO.stato, ANNUNCIO.CAP, ANNUNCIO.COMUNE, ANNUNCIO.terrazzi, ANNUNCIO.indirizzo, ANNUNCIO.posto_auto" +
            //", ANNUNCIO.cantina, ANNUNCIO.cucina, ANNUNCIO.box, ANNUNCIO.salone, ANNUNCIO.camere_da_letto, ANNUNCIO.camerette, ANNUNCIO.bagni, ANNUNCIO.classe_energetica, ANNUNCIO.quartiere, ANNUNCIO.latitude, ANNUNCIO.longitude " +

            //", FORMAT (ANNUNCIO.date_added,\"dd-MM-yyyy\") as date_added " +
            //", FORMAT (ANNUNCIO.date_added,\"yyyyMMdd\") as date_ordered " +
            //            ", ANNUNCIO.tipo, ANNUNCIO.prezzo, categorie.nome AS categoria, categorie.categoria_id" +
            //        " FROM categorie INNER JOIN (ANNUNCIO LEFT JOIN UTENTI ON ANNUNCIO.fk_user_id=UTENTI.user_id) ON categorie.categoria_id=ANNUNCIO.fk_categoria_id " 

            mStrSQL = _sqlElencoImmobili;
            mStrSQL += " WHERE ANNUNCIO.date_deleted Is Null   And ANNUNCIO.fk_user_id= " + userId;

            //    if Me.chkAnnunciEsterni.Checked {
            //    mStrSQL+= " AND  ANNUNCIO.SOURCE_ID is null "
            //}


            mStrSQL += " ORDER BY ANNUNCIO.date_added DESC";

            mDt = mFillDataTable(mStrSQL);


            Models.Immobile immobile;

            foreach (DataRow row in mDt.Rows)
            {

                immobile = new Models.Immobile(row, Models.Immobile.SelectFileds.Lista);


                risultato.Add(immobile);
            }

            return risultato;


        }


        public long insertAnnuncio(Models.Immobile immobile, long userId)
        {
            return insertAnnuncio(immobile, userId, false, 0, DateTime.MinValue);
        }

        public long insertAnnuncioInTestMode(Models.Immobile immobile, long userId)
        {
            return insertAnnuncio(immobile, userId, true, 0, DateTime.MinValue);
        }


        public long insertAnnuncio(Models.Immobile immobile, long userId, bool test_mode, AnnunciManager.StatoAnnuncio myStato, DateTime dateAdded)
        {

            if (immobile.categoria == null || immobile.categoria == 0)
            {
                throw new MyManagerCSharp.MyException("La Categoria deve essere obbiligatoria");
            }

            string strSQLParametri;

            mStrSQL = "INSERT INTO ANNUNCIO ( FK_CATEGORIA_ID , MY_STATO";
            strSQLParametri = " VALUES ( " + (int)immobile.categoria;

            if (myStato == 0)
            {
                strSQLParametri += ", '" + AnnunciManager.StatoAnnuncio.Pubblicato.ToString() + "' ";
            }
            else
            {
                strSQLParametri += ", '" + myStato.ToString() + "' ";
            }


            System.Data.Common.DbCommand command;
            command = mConnection.CreateCommand();


            //'27/01/2012 iposto data di modifica =  data inserimento 
            DateTime dataCorrente = DateTime.Now;

            mStrSQL += ",DATE_ADDED, DATE_MODIFIED";
            strSQLParametri += ", @DATE_ADDED , @DATE_MODIFIED ";

            if (dateAdded != DateTime.MinValue)
            {
                mAddParameter(command, "@DATE_ADDED", dateAdded);
                mAddParameter(command, "@DATE_MODIFIED", dateAdded);
            }
            else
            {
                mAddParameter(command, "@DATE_ADDED", dataCorrente);
                mAddParameter(command, "@DATE_MODIFIED", dataCorrente);
            }

            if (userId != 0)
            {
                mStrSQL += ",FK_USER_ID ";
                strSQLParametri += ", @FK_USER_ID ";
                mAddParameter(command, "@FK_USER_ID", userId);
            }

            mStrSQL += ",TIPO ";
            strSQLParametri += ", @TIPO ";
            mAddParameter(command, "@TIPO", immobile.immobile.ToString());

            if (!String.IsNullOrEmpty(immobile.nota))
            {
                mStrSQL += ",DESCRIZIONE ";
                strSQLParametri += ", @DESCRIZIONE ";
                mAddParameter(command, "@DESCRIZIONE", immobile.nota);
            }

            if (immobile.MQ > 0)
            {
                mStrSQL += ",MQ ";
                strSQLParametri += ", @MQ ";
                mAddParameter(command, "@MQ", immobile.MQ);
            }

            if (immobile.piano != null && immobile.piano != int.MinValue)
            {
                mStrSQL += ",PIANO ";
                strSQLParametri += ", @PIANO ";
                mAddParameter(command, "@PIANO", immobile.piano);
            }

            if (immobile.pianiTotali != null && immobile.pianiTotali != int.MinValue)
            {
                mStrSQL += ",PIANI_TOTALI ";
                strSQLParametri += ", @PIANI_TOTALI ";
                mAddParameter(command, "@PIANI_TOTALI", immobile.pianiTotali);
            }

            if (immobile.anno != null && immobile.anno > 0)
            {
                mStrSQL += ",ANNO ";
                strSQLParametri += ", @ANNO ";
                mAddParameter(command, "@ANNO", immobile.anno);
            }

            if (immobile.cucina != null && immobile.cucina != Models.Immobile.TipoCucina.Undefined)
            {
                mStrSQL += ",CUCINA ";
                strSQLParametri += ", @CUCINA ";
                //'addParameter(command, "@CUCINA", immobile.cucina.ToString.Replace("_", " "))
                mAddParameter(command, "@CUCINA", immobile.cucina.ToString());
            }


            if (immobile.salone != null && immobile.salone != Models.Immobile.TipoSalone.Undefined)
            {
                mStrSQL += ",SALONE ";
                strSQLParametri += ", @SALONE ";
                mAddParameter(command, "@SALONE", immobile.salone.ToString());
            }


            if (immobile.statoImmobile != null && immobile.statoImmobile != Models.Immobile.TipoStatoImmobile.Undefined)
            {
                mStrSQL += ",STATO ";
                strSQLParametri += ", @STATO ";
                //'addParameter(command, "@STATO", immobile.statoImmobile.ToString.Replace("_", " "))
                mAddParameter(command, "@STATO", immobile.statoImmobile.ToString());
            }


            if (immobile.occupazione != null && immobile.occupazione != Models.Immobile.TipoOccupazione.Undefined)
            {
                mStrSQL += ",OCCUPAZIONE ";
                strSQLParametri += ", @OCCUPAZIONE ";
                //'addParameter(command, "@OCCUPAZIONE", immobile.occupazione.ToString.Replace("_", " "))
                mAddParameter(command, "@OCCUPAZIONE", immobile.occupazione.ToString());
            }


            if (immobile.box != null && immobile.box != Models.Immobile.TipoBoxAuto.Undefined)
            {
                mStrSQL += ",BOX ";
                strSQLParametri += ", @BOX ";
                mAddParameter(command, "@BOX", immobile.box.ToString());
            }

            if (immobile.postoAuto != null && immobile.postoAuto != Models.Immobile.TipoPostoAuto.Undefined)
            {
                mStrSQL += ",POSTO_AUTO ";
                strSQLParametri += ", @POSTO_AUTO ";
                //'addParameter(command, "@POSTO_AUTO", immobile.postoAuto.ToString.Replace("_", " "))
                mAddParameter(command, "@POSTO_AUTO", immobile.postoAuto.ToString());
            }


            if (immobile.prezzo > 0)
            {
                mStrSQL += ",PREZZO ";
                strSQLParametri += ", @PREZZO ";
                mAddParameter(command, "@PREZZO", immobile.prezzo);
            }

            if (!String.IsNullOrEmpty(immobile.indirizzo))
            {
                mStrSQL += ",INDIRIZZO ";
                strSQLParametri += ", @INDIRIZZO ";
                mAddParameter(command, "@INDIRIZZO", immobile.indirizzo);
            }

            if (!String.IsNullOrEmpty(immobile.regione))
            {
                mStrSQL += ",REGIONE ";
                strSQLParametri += ", @REGIONE ";
                mAddParameter(command, "@REGIONE", immobile.regione);
            }

            if (!String.IsNullOrEmpty(immobile.provincia))
            {
                mStrSQL += ",PROVINCIA ";
                strSQLParametri += ", @PROVINCIA ";
                mAddParameter(command, "@PROVINCIA", immobile.provincia);
            }

            if (!String.IsNullOrEmpty(immobile.comune))
            {
                mStrSQL += ", COMUNE ";
                strSQLParametri += ", @COMUNE ";
                mAddParameter(command, "@COMUNE", immobile.comune);
            }


            if (immobile.regioneId != -1)
            {
                mStrSQL += ",REGIONE_ID ";
                strSQLParametri += ", @REGIONE_ID ";
                mAddParameter(command, "@REGIONE_ID", immobile.regioneId);
            }

            //'if (immobile.provinciaId != -1 {
            if (!String.IsNullOrEmpty(immobile.provinciaId))
            {
                mStrSQL += ",PROVINCIA_ID ";
                strSQLParametri += ", @PROVINCIA_ID ";
                mAddParameter(command, "@PROVINCIA_ID", immobile.provinciaId);
            }

            //'if (immobile.comuneId != -1 {
            if (!String.IsNullOrEmpty(immobile.comuneId))
            {
                mStrSQL += ", COMUNE_ID ";
                strSQLParametri += ", @COMUNE_ID ";
                mAddParameter(command, "@COMUNE_ID", immobile.comuneId);
            }



            if (!String.IsNullOrEmpty(immobile.quartiere))
            {
                mStrSQL += ",QUARTIERE ";
                strSQLParametri += ", @QUARTIERE ";
                mAddParameter(command, "@QUARTIERE", immobile.quartiere);
            }

            if (!String.IsNullOrEmpty(immobile.cap))
            {
                mStrSQL += ",CAP ";
                strSQLParametri += ", @CAP ";
                mAddParameter(command, "@CAP", immobile.cap);
            }

            if (immobile.camereDaLetto != null && immobile.camereDaLetto != -1)
            {
                mStrSQL += ",CAMERE_DA_LETTO ";
                strSQLParametri += ", @CAMERE_DA_LETTO ";
                mAddParameter(command, "@CAMERE_DA_LETTO", immobile.camereDaLetto);
            }

            if (immobile.camerette != null && immobile.camerette != -1)
            {
                mStrSQL += ",CAMERETTE ";
                strSQLParametri += ", @CAMERETTE ";
                mAddParameter(command, "@CAMERETTE", immobile.camerette);
            }

            if (immobile.bagni != null && immobile.bagni != -1)
            {
                mStrSQL += ",BAGNI ";
                strSQLParametri += ", @BAGNI ";
                mAddParameter(command, "@BAGNI", immobile.bagni);
            }

            if (immobile.balconi != null && immobile.balconi != -1)
            {
                mStrSQL += ",BALCONI ";
                strSQLParametri += ", @BALCONI ";
                mAddParameter(command, "@BALCONI", immobile.balconi);
            }

            if (immobile.terrazzi != null && immobile.terrazzi != -1)
            {
                mStrSQL += ",TERRAZZI ";
                strSQLParametri += ", @TERRAZZI ";
                mAddParameter(command, "@TERRAZZI", immobile.terrazzi);
            }


            if (immobile.giardinoMq != null && immobile.giardinoMq != int.MinValue)
            {
                mStrSQL += ",GIARDINO_MQ ";
                strSQLParametri += ", @GIARDINO_MQ ";
                mAddParameter(command, "@GIARDINO_MQ", immobile.giardinoMq);
            }


            if (immobile.cantina != null && immobile.cantina != -1)
            {
                mStrSQL += ",CANTINA ";
                strSQLParametri += ", @CANTINA ";
                mAddParameter(command, "@CANTINA", immobile.cantina);
            }

            if (immobile.soffitta != null && immobile.soffitta != -1)
            {
                mStrSQL += ",SOFFITTA ";
                strSQLParametri += ", @SOFFITTA ";
                mAddParameter(command, "@SOFFITTA", immobile.soffitta);
            }


            if (immobile.climatizzato != null && immobile.climatizzato != -1)
            {
                mStrSQL += ",CLIMATIZZATO ";
                strSQLParametri += ", @CLIMATIZZATO ";
                mAddParameter(command, "@CLIMATIZZATO", immobile.climatizzato);
            }


            if (immobile.ascensore != null && immobile.ascensore != -1)
            {
                mStrSQL += ",ASCENSORE ";
                strSQLParametri += ", @ASCENSORE ";
                mAddParameter(command, "@ASCENSORE", immobile.ascensore);
            }

            if (immobile.portiere != null && immobile.portiere != -1)
            {
                mStrSQL += ",PORTIERE ";
                strSQLParametri += ", @PORTIERE ";
                mAddParameter(command, "@PORTIERE", immobile.portiere);
            }

            if (immobile.speseCondominiali != null && immobile.speseCondominiali > 0)
            {
                mStrSQL += ",SPESE_CONDOMINIALI ";
                strSQLParametri += ", @SPESE_CONDOMINIALI ";
                mAddParameter(command, "@SPESE_CONDOMINIALI", immobile.speseCondominiali);
            }

            //'REL. 1.0.0.7 del 05/01/2012
            //if ( !String.IsNullOrEmpty(immobile.classeEnergetica)) {
            if (immobile.classeEnergetica != null && immobile.classeEnergetica != Models.Immobile.TipoClasseEnergetica.Undefined)
            {
                mStrSQL += ",CLASSE_ENERGETICA ";
                strSQLParametri += ", @CLASSE_ENERGETICA ";
                mAddParameter(command, "@CLASSE_ENERGETICA", immobile.classeEnergetica.ToString());
            }



            if (immobile.riscaldamento != null && immobile.riscaldamento != Models.Immobile.TipoRiscaldamento.Undefined)
            {
                mStrSQL += ",RISCALDAMENTO ";
                strSQLParametri += ", @RISCALDAMENTO ";
                //'addParameter(command, "@RISCALDAMENTO", immobile.riscaldamento.ToString.Replace("_", " "))
                mAddParameter(command, "@RISCALDAMENTO", immobile.riscaldamento.ToString());
            }


            if (immobile.latitude != 0)
            {
                mStrSQL += ",LATITUDE ";
                strSQLParametri += "," + immobile.latitude.ToString("G18").Replace(",", ".");


                //'25/02/2011  utilizzando i parametri mi tronca i valori!!

                //'strSQLParametri+= ", @LATITUDE "
                //'addParameter(command, "@LATITUDE", immobile.latitude)
            }

            if (immobile.longitude != 0)
            {
                mStrSQL += ",LONGITUDE ";

                strSQLParametri += "," + immobile.longitude.ToString("G18").Replace(",", ".");
                //'strSQLParametri+= ", @LONGITUDE "
                //'addParameter(command, "@LONGITUDE", immobile.longitude)
            }



            //'24/01/2012
            if (immobile.sourceId != 0 && immobile.sourceId != Models.Immobile.TipoSourceId.Undefined)
            {
                mStrSQL += ",SOURCE_ID ";
                strSQLParametri += ", @SOURCE_ID ";
                mAddParameter(command, "@SOURCE_ID", (int)immobile.sourceId);
            }

            if (!String.IsNullOrEmpty(immobile.externalId))
            {
                mStrSQL += ",EXTERNAL_ID ";
                strSQLParametri += ", @EXTERNAL_ID ";
                mAddParameter(command, "@EXTERNAL_ID", immobile.externalId);
            }


            command.CommandText = mStrSQL + " ) " + strSQLParametri + " )";

            if (test_mode == true)
            {
                mTransactionBegin();
                mExecuteNoQuery(command);
                mTransactionRollback();
                return -1;
            }

            mExecuteNoQuery(command);

            return mGetIdentity();
        }




        #region "Trattative"



        
      
        #endregion
    }

}

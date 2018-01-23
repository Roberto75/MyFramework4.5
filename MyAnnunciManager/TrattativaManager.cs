using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Annunci
{
    public class TrattativaManager : MyManagerCSharp.ManagerDB
    {

        public enum StatoTrattativa
        {
            Attiva = 1,
            NonPiuDiInteresse,
            AnnuncioRimosso,
            TerminataConSuccesso,
            TerminataSenzaSuccesso,
            TerminataConFrode,
            Altro
        }
        public TrattativaManager(string connectionName)
            : base(connectionName)
        {

        }
        public TrattativaManager(System.Data.Common.DbConnection connection)
            : base(connection)
        {

        }


        public long countTrattativeByStato(StatoTrattativa stato)
        {
            return countTrattativeByStato(stato, -1);
        }


        public long countTrattativeByStato(StatoTrattativa stato, long userId)
        {
            mStrSQL = "SELECT COUNT(*) FROM TRATTATIVA WHERE STATO = '" + stato.ToString() + "'";

            if (userId != -1)
            {
                mStrSQL += " AND fk_user_id = " + userId;
            }
            return long.Parse(mExecuteScalar(mStrSQL));
        }

        public System.Collections.Hashtable countTrattativeByStato()
        {
            return countTrattativeByStato(-1);
        }

        public System.Collections.Hashtable countTrattativeByStato(long userId)
        {
            System.Collections.Hashtable risultato = new System.Collections.Hashtable();

            foreach (var value in Enum.GetValues(typeof(StatoTrattativa)))
            {
                risultato.Add((StatoTrattativa)value, countTrattativeByStato((StatoTrattativa)value, userId));
            }

            return risultato;
        }






        //public System.Collections.Hashtable countTrattativeByStato()
        //{
        //    return countTrattativeByStato(-1);
        //}

        //public System.Collections.Hashtable countTrattativeByStato(long userId)
        //{
        //    System.Collections.Hashtable risultato = new System.Collections.Hashtable();

        //    foreach (var value in Enum.GetValues(typeof(StatoTrattativa)))
        //    {
        //        risultato.Add((StatoTrattativa)value, countTrattativeByStato((StatoTrattativa)value, userId));
        //    }

        //    return risultato;
        //}




        public long rispondi(long trattativaId, long userId, string testo)
        {
            //una nuova risposta per l'annuncio da parte di un utente
            mStrSQL = "INSERT INTO RISPOSTA ( FK_USER_ID, FK_TRATTATIVA_ID, TESTO , FK_RISPOSTA_ID , OWNER) " +
                        " VALUES ( @FK_USER_ID ,  @FK_TRATTATIVA_ID , @TESTO , NULL, " + userId + " ) ";

            //'30/01/2011 VER. 1.0.0.5
            //'l'inserimento di una nuova risposta comporta la notifica del messaggio 


            System.Data.Common.DbCommand command;
            command = mConnection.CreateCommand();
            mAddParameter(command, "@FK_USER_ID", userId);
            mAddParameter(command, "@FK_TRATTATIVA_ID", trattativaId);
            mAddParameter(command, "@TESTO", testo);

            command.CommandText = mStrSQL;

            mExecuteNoQuery(command);
            //Return Me.mGetIdentity
            return 1;
        }

        public long rispondi(long trattativaId, long userId, string testo, long risposta_id)
        {
            //'una nuova risposta per l'annuncio da parte di un utente
            mStrSQL = "SELECT OWNER FROM RISPOSTA WHERE RISPOSTA_ID  = " + risposta_id;

            long owner;
            owner = long.Parse(mExecuteScalar(mStrSQL));

            mStrSQL = "INSERT INTO RISPOSTA ( FK_USER_ID, FK_TRATTATIVA_ID, TESTO , FK_RISPOSTA_ID , OWNER) " +
                        " VALUES ( @FK_USER_ID ,  @FK_TRATTATIVA_ID , @TESTO , " + risposta_id + ", " + owner + " ) ";


            System.Data.Common.DbCommand command;
            command = mConnection.CreateCommand();
            mAddParameter(command, "@FK_USER_ID", userId);
            mAddParameter(command, "@FK_TRATTATIVA_ID", trattativaId);
            mAddParameter(command, "@TESTO", testo);

            command.CommandText = mStrSQL;

            mExecuteNoQuery(command);

            //Return Me.mGetIdentity
            return 1;
        }




        public List<Models.Trattativa> getListTrattative(long userId, Models.Trattativa.TipoTrattativa tipo)
        {
            List<Models.Trattativa> risultato;
            risultato = new List<Models.Trattativa>();

            //Debug
            //userId = 567809036;


            if (tipo == Models.Trattativa.TipoTrattativa.Immobile)
            {
                //manca il campo Annuncio.nome
                mStrSQL = "SELECT DISTINCT TRATTATIVA.trattativa_id, TRATTATIVA.stato, UTENTI.my_login, UTENTI.user_id, ANNUNCIO.annuncio_id,    ANNUNCIO.date_added, ANNUNCIO.tipo, ANNUNCIO.prezzo, categorie.nome AS categoria, categorie.categoria_id " +
                                " FROM ((TRATTATIVA INNER JOIN ANNUNCIO ON ANNUNCIO.annuncio_id=TRATTATIVA.fk_annuncio_id) LEFT JOIN CATEGORIE ON categorie.categoria_id=ANNUNCIO.fk_categoria_id) LEFT JOIN UTENTI ON ANNUNCIO.fk_user_id=UTENTI.user_id " +
                                " WHERE TRATTATIVA.fk_user_id= " + userId + "  AND  TRATTATIVA.DATE_DELETED IS NULL ";
            }
            else if (tipo == Models.Trattativa.TipoTrattativa.Libro)
            {
                mStrSQL = "SELECT DISTINCT TRATTATIVA.trattativa_id, TRATTATIVA.stato, UTENTI.my_login, UTENTI.user_id, ANNUNCIO.annuncio_id,  ANNUNCIO.nome,   ANNUNCIO.date_added, ANNUNCIO.tipo, ANNUNCIO.prezzo, categorie.nome AS categoria, categorie.categoria_id " +
                " FROM ((TRATTATIVA INNER JOIN ANNUNCIO ON ANNUNCIO.annuncio_id=TRATTATIVA.fk_annuncio_id) LEFT JOIN CATEGORIE ON categorie.categoria_id=ANNUNCIO.fk_categoria_id) LEFT JOIN UTENTI ON ANNUNCIO.fk_user_id=UTENTI.user_id " +
                " WHERE TRATTATIVA.fk_user_id= " + userId + "  AND  TRATTATIVA.DATE_DELETED IS NULL ";
            }


            mDt = mFillDataTable(mStrSQL);

            Models.Trattativa trattativa;

            foreach (System.Data.DataRow row in mDt.Rows)
            {

                trattativa = new Models.Trattativa(row, tipo);

                //trattativa.userId = userId;
                //trattativa.login = row["my_login"].ToString();
                //trattativa.trattativaId = long.Parse(row["trattativa_id"].ToString());
                //trattativa.annuncioId = long.Parse(row["annuncio_id"].ToString());
                //trattativa.dateAdded = (row["date_added"] is DBNull) ? DateTime.MinValue : DateTime.Parse(row["date_added"].ToString());
                //trattativa.stato = (row["stato"] is DBNull) ? Models.Trattativa.Stato.Undefined : (Models.Trattativa.Stato)Enum.Parse(typeof(Models.Trattativa.Stato), row["stato"].ToString());
                //trattativa.prezzo = (row["prezzo"] is DBNull) ? 0 : Decimal.Parse(row["prezzo"].ToString());
                //trattativa.tipo = (Immobiliare.Models.Immobile.TipoImmobile)Enum.Parse(typeof(Immobiliare.Models.Immobile.TipoImmobile), row["tipo"].ToString());
                //trattativa.categoria = (Immobiliare.Models.Immobile.Categorie)int.Parse(row["categoria_id"].ToString());
                risultato.Add(trattativa);
            }

            return risultato;
        }


        public long insertTrattativa(long annuncioId, long userId)
        {
            mStrSQL = "INSERT INTO TRATTATIVA ( FK_USER_ID , FK_ANNUNCIO_ID, STATO )" + " VALUES ( @USER_ID , @ANNUNCIO_ID , @STATO )";

            System.Data.Common.DbCommand command;
            command = mConnection.CreateCommand();
            command.CommandText = mStrSQL;

            mAddParameter(command, "@USER_ID", userId);
            mAddParameter(command, "@ANNUNCIO_ID", annuncioId);
            mAddParameter(command, "@STATO", TrattativaManager.StatoTrattativa.Attiva.ToString());

            mExecuteNoQuery(command);
            return mGetIdentity();
        }


        public Models.Trattativa getTrattativa(long trattativaId)
        {

            mStrSQL = "SELECT DISTINCT TRATTATIVA.trattativa_id, TRATTATIVA.stato, UTENTI.my_login, UTENTI.user_id, ANNUNCIO.annuncio_id, ANNUNCIO.date_added, ANNUNCIO.tipo, ANNUNCIO.prezzo, categorie.nome AS categoria, categorie.categoria_id " +
                " FROM ((TRATTATIVA INNER JOIN ANNUNCIO ON ANNUNCIO.annuncio_id=TRATTATIVA.fk_annuncio_id) LEFT JOIN CATEGORIE ON categorie.categoria_id=ANNUNCIO.fk_categoria_id) LEFT JOIN UTENTI ON ANNUNCIO.fk_user_id=UTENTI.user_id " +
                " WHERE TRATTATIVA.trattativa_id= " + trattativaId + "  AND  TRATTATIVA.DATE_DELETED IS NULL ";


            mDt = mFillDataTable(mStrSQL);


            Models.Trattativa trattativa;

            DataRow row;
            row = mDt.Rows[0];
            trattativa = new Models.Trattativa(row, Models.Trattativa.TipoTrattativa.Immobile);

            return trattativa;
        }


        public void setRisposteFromTrattativa(Models.Trattativa trattativa)
        {
            mStrSQL = "SELECT  UTENTI.my_login as my_login , UTENTI.isModeratore as isModeratore, UTENTI.user_id as user_id, risposta_id, fk_risposta_id ,RISPOSTA.date_added, testo , TRATTATIVA.FK_ANNUNCIO_ID as annuncio_id, FK_TRATTATIVA_ID as FK_TRATTATIVA_ID, UTENTI.customer_id " +
                    " FROM TRATTATIVA INNER JOIN (RISPOSTA LEFT  JOIN UTENTI ON RISPOSTA.FK_user_ID = UTENTI.USER_ID ) ON RISPOSTA.FK_TRATTATIVA_ID = TRATTATIVA.TRATTATIVA_ID" +
                    " WHERE FK_TRATTATIVA_ID = " + trattativa.trattativaId + " AND TRATTATIVA.FK_ANNUNCIO_ID =" + trattativa.annuncioId;

            mStrSQL += " ORDER BY RISPOSTA.date_added ASC";

            mDt = mFillDataTable(mStrSQL);

            List<Models.Risposta> risposte = new List<Models.Risposta>();

            Models.Risposta risposta;

            foreach (DataRow row in mDt.Rows)
            {
                risposta = new Models.Risposta(row, trattativa.annuncioId);

                risposte.Add(risposta);
            }

            trattativa.risposte = risposte;
        }


        public List<Annunci.Models.Trattativa> getTrattativeOnMyAnnuncio(long userId, long annuncioId, Models.Trattativa.TipoTrattativa tipo)
        {


            mStrSQL = "SELECT TRATTATIVA.date_added as date_added , TRATTATIVA.trattativa_id , 'dummy' as categoria " +
                ",ANNUNCIO.annuncio_id, ANNUNCIO.prezzo, ANNUNCIO.tipo, ANNUNCIO.nome,  UTENTI.user_id,  UTENTI.my_login, TRATTATIVA.stato" +
       " FROM " +
        " (TRATTATIVA INNER JOIN UTENTI ON TRATTATIVA.fk_user_id=UTENTI.user_id) INNER JOIN ANNUNCIO ON ANNUNCIO.annuncio_id=TRATTATIVA.fk_annuncio_id" +
        " WHERE  TRATTATIVA.DATE_DELETED_OWNER IS NULL AND  ANNUNCIO.fk_user_id  = " + userId + " AND FK_ANNUNCIO_ID= " + annuncioId +
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





        public List<Models.Trattativa> getListMessaggi(long userId, Models.Trattativa.TipoTrattativa tipo)
        {
            List<Models.Trattativa> risultato;
            risultato = new List<Models.Trattativa>();

            //Debug
            //userId = 567809036;


            if (tipo == Models.Trattativa.TipoTrattativa.Immobile)
            {
                //manca il campo Annuncio.nome
                //mStrSQL = "SELECT DISTINCT TRATTATIVA.trattativa_id, TRATTATIVA.stato, UTENTI.my_login, UTENTI.user_id, ANNUNCIO.annuncio_id,    ANNUNCIO.date_added, ANNUNCIO.tipo, ANNUNCIO.prezzo, categorie.nome AS categoria, categorie.categoria_id " +
                  //              " FROM ((TRATTATIVA INNER JOIN ANNUNCIO ON ANNUNCIO.annuncio_id=TRATTATIVA.fk_annuncio_id) LEFT JOIN CATEGORIE ON categorie.categoria_id=ANNUNCIO.fk_categoria_id) LEFT JOIN UTENTI ON ANNUNCIO.fk_user_id=UTENTI.user_id " +
                    //            " WHERE TRATTATIVA.fk_user_id= " + userId + "  AND  TRATTATIVA.DATE_DELETED IS NULL ";
            }
            else if (tipo == Models.Trattativa.TipoTrattativa.Libro)
            {
                mStrSQL = "SELECT  '01/01/1900' AS date_added,  TRATTATIVA.trattativa_id, UTENTI.my_login, TRATTATIVA.stato, UTENTI.user_id, ANNUNCIO.tipo, ANNUNCIO.nome AS nome, ANNUNCIO.prezzo, categorie.nome AS categoria, Switch(tipo=1,'Vendo',tipo=2,'Compro',tipo=3,'Scambio') AS tipo_desc, categorie.categoria_id, ANNUNCIO.annuncio_id " +
            " FROM ((TRATTATIVA INNER JOIN ANNUNCIO ON ANNUNCIO.annuncio_id=TRATTATIVA.fk_annuncio_id) LEFT JOIN CATEGORIE ON categorie.categoria_id=ANNUNCIO.fk_categoria_id) LEFT JOIN UTENTI ON ANNUNCIO.fk_user_id=UTENTI.user_id " +
            " WHERE TRATTATIVA.DATE_DELETED IS NULL AND TRATTATIVA.notifica_user =" + userId;

                mStrSQL += " UNION ";

                mStrSQL += "SELECT '01/01/1900' AS date_added, TRATTATIVA.trattativa_id, UTENTI.my_login , TRATTATIVA.stato, UTENTI.user_id, ANNUNCIO.tipo, ANNUNCIO.nome AS nome, ANNUNCIO.prezzo, categorie.nome AS categoria, Switch(tipo=1,'Vendo',tipo=2,'Compro',tipo=3,'Scambio') AS tipo_desc, categorie.categoria_id, ANNUNCIO.annuncio_id " +
            " FROM ((TRATTATIVA INNER JOIN ANNUNCIO ON ANNUNCIO.annuncio_id=TRATTATIVA.fk_annuncio_id) LEFT JOIN CATEGORIE ON categorie.categoria_id=ANNUNCIO.fk_categoria_id) LEFT JOIN UTENTI ON TRATTATIVA.fk_user_id=UTENTI.user_id " +
            " WHERE TRATTATIVA.DATE_DELETED IS NULL AND TRATTATIVA.notifica_owner =" + userId;


                mStrSQL += " ORDER BY nome DESC";
            }


            mDt = mFillDataTable(mStrSQL);

            Models.Trattativa trattativa;

            foreach (System.Data.DataRow row in mDt.Rows)
            {

                trattativa = new Models.Trattativa(row, tipo);

                //trattativa.userId = userId;
                //trattativa.login = row["my_login"].ToString();
                //trattativa.trattativaId = long.Parse(row["trattativa_id"].ToString());
                //trattativa.annuncioId = long.Parse(row["annuncio_id"].ToString());
                //trattativa.dateAdded = (row["date_added"] is DBNull) ? DateTime.MinValue : DateTime.Parse(row["date_added"].ToString());
                //trattativa.stato = (row["stato"] is DBNull) ? Models.Trattativa.Stato.Undefined : (Models.Trattativa.Stato)Enum.Parse(typeof(Models.Trattativa.Stato), row["stato"].ToString());
                //trattativa.prezzo = (row["prezzo"] is DBNull) ? 0 : Decimal.Parse(row["prezzo"].ToString());
                //trattativa.tipo = (Immobiliare.Models.Immobile.TipoImmobile)Enum.Parse(typeof(Immobiliare.Models.Immobile.TipoImmobile), row["tipo"].ToString());
                //trattativa.categoria = (Immobiliare.Models.Immobile.Categorie)int.Parse(row["categoria_id"].ToString());
                risultato.Add(trattativa);
            }

            return risultato;
        }









    }

}
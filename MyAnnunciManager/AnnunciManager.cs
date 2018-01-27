using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Annunci
{

    public class AnnunciManager : MyManagerCSharp.ManagerDB
    {

        public enum StatoAnnuncio
        {
            Undefined = -1,
            Pubblicato = 1,
            Oggetto_non_piu_disponibile,
            Concluso_con_successo,
            Altro,
            OffLine,
            Da_cancellare
        }


        public enum TipoAnnuncio
        {
            Vendo = 1,
            Compro = 2,
            Scambio = 3
        }

        public AnnunciManager(string connectionName)
            : base(connectionName)
        {

        }

        public AnnunciManager(System.Data.Common.DbConnection connection)
            : base(connection)
        {

        }


        public bool deleteUser(long userId, string absoluteServerPath)
        {
            deleteAnnunciByUserId(userId, absoluteServerPath);

            //potrebbe aver scritto delle treatta senza aver creato annunci!
            deleteTrattativeByUserId(userId);


            mStrSQL = "DELETE * FROM UTENTI WHERE USER_ID = " + userId;
            return mExecuteNoQuery(mStrSQL) > 0;
        }



        public bool deleteTrattativeByUserId(long userId)
        {
            mStrSQL = "SELECT TRATTATIVA_ID FROM TRATTATIVA WHERE FK_USER_ID = " + userId;

            mDt = mFillDataTable(mStrSQL);

            foreach (System.Data.DataRow row in mDt.Rows)
            {
                deleteTrattativa(long.Parse(row["TRATTATIVA_ID"].ToString()));
            }


            return true;
        }



        public bool deleteTrattativa(long trattativaId)
        {
            mStrSQL = "UPDATE RISPOSTA SET FK_RISPOSTA_ID = NULL WHERE FK_TRATTATIVA_ID=" + trattativaId;
            mExecuteNoQuery(mStrSQL);

            mStrSQL = "DELETE * FROM RISPOSTA  WHERE FK_TRATTATIVA_ID = " + trattativaId;
            mExecuteNoQuery(mStrSQL);

            mStrSQL = "DELETE * FROM TRATTATIVA  WHERE TRATTATIVA_ID = " + trattativaId;
            mExecuteNoQuery(mStrSQL);

            return true;
        }

        public bool deleteAnnunciByUserId(long userId, string absoluteServerPath)
        {
            mStrSQL = "SELECT ANNUNCIO_ID FROM ANNUNCIO WHERE FK_USER_ID = " + userId;

            mDt = mFillDataTable(mStrSQL);

            foreach (System.Data.DataRow row in mDt.Rows)
            {
                deleteAnnuncio(long.Parse(row["ANNUNCIO_ID"].ToString()), absoluteServerPath);
            }


            return true;
        }



        public bool deleteAnnuncio(long annuncioId, string absoluteServerPath)
        {
            //Cancellazione fisica
            mStrSQL = "SELECT TRATTATIVA_ID FROM TRATTATIVA WHERE FK_ANNUNCIO_ID=" + annuncioId;

            System.Data.DataTable trattative;

            trattative = mFillDataTable(mStrSQL);

            foreach (System.Data.DataRow row in trattative.Rows)
            {
                mStrSQL = "UPDATE RISPOSTA SET FK_RISPOSTA_ID = NULL WHERE FK_TRATTATIVA_ID=" + row["trattativa_id"];
                mExecuteNoQuery(mStrSQL);

                mStrSQL = "DELETE * FROM RISPOSTA WHERE FK_TRATTATIVA_ID=" + row["trattativa_id"];
                mExecuteNoQuery(mStrSQL);

            }



            mStrSQL = "DELETE * FROM TRATTATIVA WHERE FK_ANNUNCIO_ID=" + annuncioId;
            mExecuteNoQuery(mStrSQL);


            if (!String.IsNullOrEmpty(absoluteServerPath))
            {

                PhotoManager photo = new PhotoManager(this.mConnection);


                mStrSQL = "SELECT PHOTO_ID FROM PHOTO WHERE FK_EXTERNAL_ID=" + annuncioId;
                System.Data.DataTable dt;
                dt = mFillDataTable(mStrSQL);

                foreach (System.Data.DataRow row in dt.Rows)
                {
                    photo.deletePhoto(long.Parse(row["PHOTO_ID"].ToString()), absoluteServerPath);
                }


                //cancello la directory!
                string folder;
                folder = System.Configuration.ConfigurationManager.AppSettings["mercatino.images.folder"];
                folder = absoluteServerPath + folder.Replace("~", "") + annuncioId + "/";
                if (System.IO.Directory.Exists(folder)) { }
                try
                {
                    System.IO.Directory.Delete(folder, true);
                }
                catch (Exception e)
                {
                    //'lo ignoro
                }

            }


            mStrSQL = "DELETE * FROM PHOTO WHERE FK_EXTERNAL_ID=" + annuncioId;
            mExecuteNoQuery(mStrSQL);

            mStrSQL = "DELETE * FROM ANNUNCIO WHERE ANNUNCIO_ID=" + annuncioId;
            mExecuteNoQuery(mStrSQL);
            return true;
        }



        public bool deleteAnnuncioLogic(long annuncioId, StatoAnnuncio causale, string absoluteServerPath)
        {
            {
                mStrSQL = "UPDATE ANNUNCIO SET DATE_DELETED = NOW() , MY_STATO = '" + causale.ToString() + "' WHERE ANNUNCIO_ID = " + annuncioId;
                mExecuteNoQuery(mStrSQL);

                //'Dim statoTrattativa As MyManager.MercatinoManager.StatoTrattativa

                //'If causale = StatoAnnuncio.ConclusoConSuccesso Then
                //'StatoTrattativa = MercatinoManager.StatoTrattativa.AnnuncioRimosso
                //'End If

                //'tutte le trattative collegate all'annuncio vengono notificate 
                mStrSQL = "UPDATE TRATTATIVA SET STATO = '" + TrattativaManager.StatoTrattativa.AnnuncioRimosso.ToString() + "' " +
                    " WHERE FK_ANNUNCIO_ID = " + annuncioId;
                mExecuteNoQuery(mStrSQL);


                if (!String.IsNullOrEmpty(absoluteServerPath))
                {

                    PhotoManager photo = new PhotoManager(mConnection);

                    mStrSQL = "SELECT PHOTO_ID FROM PHOTO WHERE FK_EXTERNAL_ID=" + annuncioId;
                    mDt = mFillDataTable(mStrSQL);


                    foreach (System.Data.DataRow row in mDt.Rows)
                    {
                        photo.deletePhoto(long.Parse(row["PHOTO_ID"].ToString()), absoluteServerPath);
                    }

                    //'cancello la directory!
                    string folder;
                    folder = System.Configuration.ConfigurationManager.AppSettings["mercatino.images.folder"];
                    folder = absoluteServerPath + folder.Replace("~/", "") + annuncioId + System.IO.Path.DirectorySeparatorChar;
                    if (System.IO.Directory.Exists(folder))
                    {
                        try
                        {
                            System.IO.Directory.Delete(folder, true);
                        }
                        catch (Exception ex)
                        {
                            //'lo ignoro
                        }
                    }

                }

                return true;
            }
        }



        public long getNumeroRisposteOfAnnuncio(long annuncioId, long userId)
        {
            //'nel conteggio delle risposte escludo le risposte scritte da se stesso
            mStrSQL = "SELECT count(*) as NUMERO_RISPOSTE " +
                        "FROM RISPOSTA  INNER JOIN  TRATTATIVA ON RISPOSTA.FK_TRATTATIVA_ID = TRATTATIVA.TRATTATIVA_ID" +
                        " WHERE FK_ANNUNCIO_ID =" + annuncioId + " AND RISPOSTA.FK_USER_ID <> " + userId;
            return long.Parse(mExecuteScalar(mStrSQL));
        }


        public long getNumeroRisposteOfTrattativa(long trattativaId, long userId)
        {
            //'nel conteggio delle risposte escludo le risposte scritte da se stesso
            mStrSQL = "SELECT count(*) as NUMERO_RISPOSTE " +
                        "FROM RISPOSTA  INNER JOIN  TRATTATIVA ON RISPOSTA.FK_TRATTATIVA_ID = TRATTATIVA.TRATTATIVA_ID" +
                        " WHERE TRATTATIVA.TRATTATIVA_ID =" + trattativaId + " AND RISPOSTA.FK_USER_ID <> " + userId;
            return long.Parse(mExecuteScalar(mStrSQL));
        }



        public int updateAnnuncioDescrizione(long annuncioId, string descrizione, bool test_mode)
        {
            mStrSQL = "UPDATE ANNUNCIO SET DATE_MODIFIED = GetDate()  ";

            System.Data.Common.DbCommand command;
            command = mConnection.CreateCommand();

            if (!String.IsNullOrEmpty(descrizione))
            {
                mStrSQL += " ,DESCRIZIONE = @DESCRIZIONE ";
                mAddParameter(command, "@DESCRIZIONE", descrizione);
            }

            mStrSQL += " WHERE ANNUNCIO_ID=" + annuncioId;

            command.CommandText = mStrSQL;


            if (test_mode)
            {
                mTransactionBegin();
                mExecuteNoQuery(command);
                mTransactionRollback();
                return -1;
            }

            return mExecuteNoQuery(command);
        }

        public int updateAnnuncioPrezzo(long annuncioId, decimal prezzo, bool test_mode)
        {
            mStrSQL = "UPDATE ANNUNCIO SET DATE_MODIFIED = GetDate()  ";

            System.Data.Common.DbCommand command;
            command = mConnection.CreateCommand();

            if (prezzo != Decimal.MinValue)
            {
                mStrSQL += " ,PREZZO = @PREZZO ";
                mAddParameter(command, "@PREZZO", prezzo);
            }

            mStrSQL += " WHERE ANNUNCIO_ID=" + annuncioId;

            command.CommandText = mStrSQL;


            if (test_mode)
            {
                mTransactionBegin();
                mExecuteNoQuery(command);
                mTransactionRollback();
                return -1;
            }

            return mExecuteNoQuery(command);
        }




        public void annuncioAddClick(long annuncioId)
        {
            mStrSQL = "UPDATE ANNUNCIO SET DATE_LAST_CLICK = NOW , COUNT_CLICK = COUNT_CLICK +1 , COUNT_CLICK_PARZIALE = COUNT_CLICK_PARZIALE + 1" +
                " WHERE ANNUNCIO_ID=" + annuncioId;
            mExecuteNoQuery(mStrSQL);
        }


        public void resetContatoreParziale(long annuncioId)
        {
            mStrSQL = "UPDATE ANNUNCIO SET date_start_click_parziale = NOW , COUNT_CLICK_PARZIALE = 0 " +
                                           " WHERE ANNUNCIO_ID=" + annuncioId;
            mExecuteNoQuery(mStrSQL);
        }





        public Models.Risposta getRisposta(long rispostaId)
        {
            mStrSQL = "select UTENTI.user_id, UTENTI.my_login, UTENTI.customer_id, RISPOSTA.fk_trattativa_id, RISPOSTA.testo, RISPOSTA.risposta_id, RISPOSTA.fk_risposta_id,   RISPOSTA.date_added " + " FROM RISPOSTA  LEFT JOIN UTENTI ON RISPOSTA.FK_user_ID = UTENTI.USER_ID WHERE RISPOSTA_ID =" + rispostaId;
            mDt = mFillDataTable(mStrSQL);

            return new Models.Risposta(mDt.Rows[0], -1);
        }

        public long insertTrattativa(long annuncioId, long userId)
        {
            TrattativaManager tManager = new TrattativaManager(mConnection);
            return tManager.insertTrattativa(annuncioId, userId);
        }

        public List<Models.Trattativa> getListTrattative(long userId, Models.Trattativa.TipoTrattativa tipo)
        {
            TrattativaManager tManager = new TrattativaManager(mConnection);
            return tManager.getListTrattative(userId, tipo);
        }


        public List<Models.Trattativa> getListMessaggi(long userId, Models.Trattativa.TipoTrattativa tipo)
        {
            TrattativaManager tManager = new TrattativaManager(mConnection);
            return tManager.getListMessaggi(userId, tipo);
        }



        public Models.Trattativa getTrattativa(long trattativaId)
        {
            TrattativaManager tManager = new Annunci.TrattativaManager(mConnection);
            return tManager.getTrattativa(trattativaId);
        }

        public void setRisposteFromTrattativa(Models.Trattativa trattativa)
        {
            TrattativaManager tManager = new Annunci.TrattativaManager(mConnection);
            tManager.setRisposteFromTrattativa(trattativa);
        }


        public System.Data.DataTable getEmailUtentiInTrattativa(long annuncio_id)
        {
            //prelevo gli indizzi email di tutti gli utenti che stanno in trattativa su un annuncio
            //per inviargli un'email
            mStrSQL = "SELECT utenti.my_login, utenti.email " +
                    " FROM utenti INNER JOIN trattativa ON utenti.user_id = trattativa.fk_user_id " +
                    " WHERE trattativa.fk_annuncio_id = " + annuncio_id;
            return mFillDataTable(mStrSQL);
        }



        public List<Annunci.Models.Trattativa> getTrattativeOnMyAnnuncio(long userId, long annuncioId)
        {
            TrattativaManager tManager = new TrattativaManager(mConnection);
            return tManager.getTrattativeOnMyAnnuncio(userId, annuncioId, Models.Trattativa.TipoTrattativa.Libro);
        }


        public long countPhoto(long annuncioId)
        {
            mStrSQL = "SELECT count(*) FROM PHOTO WHERE  FK_EXTERNAL_ID= " + annuncioId;
            return long.Parse(mExecuteScalar(mStrSQL));
        }

        public System.Xml.XmlDocument getCategorie_XML()
        {

            System.Xml.XmlDocument document = null;
            document = getCategorieRootLevel_XML();


            System.Xml.XmlNodeList nodeList;
            string temp;
            System.Xml.XmlNode nodeImported;
            nodeList = document.SelectNodes("/Categorie/Categoria[childnodecount > 0]");


            foreach (System.Xml.XmlNode node in nodeList)
            {
                temp = node.SelectSingleNode("categoria_id").InnerText;
                nodeImported = getCategorie_XML(long.Parse(temp));

                if (nodeImported != null)
                {
                    nodeImported = document.ImportNode(nodeImported.SelectSingleNode("/Categorie"), true);
                    node.AppendChild(nodeImported);
                }
            }

            return document;
        }


        public System.Xml.XmlDocument getCategorieRootLevel_XML()
        {
            System.Xml.XmlDocument document = new System.Xml.XmlDocument();
            System.Data.DataSet dataSet = new DataSet();

            mStrSQL = "select A.*,  (select count (*) from  CATEGORIE where FK_PADRE_ID = a.CATEGORIA_ID  ) as childnodecount  " +
                    ", (select count (*) from  ANNUNCIO where FK_CATEGORIA_ID = a.CATEGORIA_ID AND DATE_DELETED IS NULL  ) as COUNT_ANNUNCI " +
                    " from CATEGORIE A WHERE FK_PADRE_ID is NULL and HIDE = false" +
                    " ORDER by nome";

            dataSet = mFillDataSet(mStrSQL, "Categorie", "Categoria", -1);
            document.LoadXml(dataSet.GetXml());
            return document;
        }


        public System.Xml.XmlDocument getCategorie_XML(long categoria_id)
        {
            System.Xml.XmlDocument document = new System.Xml.XmlDocument();
            System.Data.DataSet dataSet = new DataSet();
            string sqlQuery;
            sqlQuery = "select A.* " +
         ",  (select count (*) from  CATEGORIE where FK_PADRE_ID = a.CATEGORIA_ID  ) as childnodecount  ";

            sqlQuery = sqlQuery + ", (select count (*) from  ANNUNCIO where FK_CATEGORIA_ID = a.CATEGORIA_ID AND DATE_DELETED IS NULL  ) as COUNT_ANNUNCI ";

            sqlQuery = sqlQuery + " from CATEGORIE A WHERE FK_PADRE_ID =" + categoria_id + " ORDER BY nome";
            dataSet = mFillDataSet(sqlQuery, "Categorie", "Categoria", -1);
            document.LoadXml(dataSet.GetXml());


            System.Xml.XmlNodeList nodeList;
            string temp;
            System.Xml.XmlNode nodeImported;
            nodeList = document.SelectNodes("/Categorie/Categoria[childnodecount > 0]");


            foreach (System.Xml.XmlNode node in nodeList)
            {
                temp = node.SelectSingleNode("categoria_id").InnerText;
                nodeImported = getCategorie_XML(long.Parse(temp));

                if (nodeImported != null)
                {
                    nodeImported = document.ImportNode(nodeImported.SelectSingleNode("/Categorie"), true);
                    node.AppendChild(nodeImported);
                }
            }



            return document;
        }



        public bool authorizeShowTrattativa(long userId, long trattativaId)
        {
            //verifico che user_id possa vedere la trattiva
            mStrSQL = "SELECT count(*) FROM TRATTATIVA INNER JOIN ANNUNCIO ON ANNUNCIO.annuncio_id=TRATTATIVA.fk_annuncio_id " +
                " WHERE ANNUNCIO.FK_USER_ID = " + userId + " OR TRATTATIVA.FK_USER_ID = " + userId +
                " AND TRATTATIVA.TRATTATIVA_ID =" + trattativaId;

            string temp;
            temp = mExecuteScalar(mStrSQL);

            return int.Parse(temp) > 0;
        }


        public long getAnnucioIdFromTrattativa(long trattativaId)
        {
            mStrSQL = "SELECT FK_ANNUNCIO_ID  FROM TRATTATIVA WHERE  TRATTATIVA_ID = " + trattativaId;

            return long.Parse(mExecuteScalar(mStrSQL));

        }

        public bool isOwner(long annuncioId, long userId)
        {
            mStrSQL = "SELECT COUNT(*) as TOT " +
                    " FROM ANNUNCIO " +
                    " WHERE ANNUNCIO.ANNUNCIO_ID = " + annuncioId +
                    " AND ANNUNCIO.FK_USER_ID = " + userId;
            return int.Parse(mExecuteScalar(mStrSQL)) == 1;
        }


        public bool updateNotificaLetturaRispostaOwner(long trattativaId)
        {
            //notifico la lettura di tutte le risposte che non sono state create da me ....
            mStrSQL = "UPDATE TRATTATIVA SET NOTIFICA_OWNER = 0  WHERE TRATTATIVA_ID = " + trattativaId;
            mExecuteNoQuery(mStrSQL);
            return true;
        }

        public bool updateNotificaLetturaRispostaUser(long trattativaId)
        {
            //notifico la lettura di tutte le risposte che non sono state create da me ....
            mStrSQL = "UPDATE TRATTATIVA SET NOTIFICA_USER = 0 WHERE TRATTATIVA_ID = " + trattativaId;
            mExecuteNoQuery(mStrSQL);
            return true;
        }

        public DataTable getEmailReplyAnnnuncio(long trattativaId)
        {
            //in teoria per ogni trattaviva sono in 2: owner + user
            mStrSQL = "SELECT utenti.my_login, utenti.email, utenti.user_id, trattativa.trattativa_id, utenti_1.my_login as login_owner , utenti_1.email as email_owner , utenti_1.user_id as user_id_owner " +
                    " FROM utenti AS utenti_1 INNER JOIN (utenti INNER JOIN (annuncio INNER JOIN trattativa ON annuncio.annuncio_id = trattativa.fk_annuncio_id) ON utenti.user_id = trattativa.fk_user_id) ON utenti_1.user_id = annuncio.fk_user_id " +
                       "  WHERE trattativa.trattativa_id = " + trattativaId;

            return mFillDataTable(mStrSQL);
        }



        public bool notificaOwner(long trattativaId, long userId)
        {
            mStrSQL = "UPDATE TRATTATIVA SET NOTIFICA_OWNER = " + userId + " WHERE trattativa_id = " + trattativaId;
            mExecuteNoQuery(mStrSQL);
            return true;
        }

        public bool notificaUser(long trattativaId, long userId)
        {
            mStrSQL = "UPDATE TRATTATIVA SET NOTIFICA_USER = " + userId + " WHERE trattativa_id = " + trattativaId;
            mExecuteNoQuery(mStrSQL);
            return true;
        }

        public long insertUser(long userId, string nome, string cognome, string email, string mylogin, long customerId)
        {
            //'in questo caso la userId non è un contatore in quanto il valore viene gestito da UserManager
            mStrSQL = "INSERT INTO UTENTI ( USER_ID,  NOME, COGNOME, MY_LOGIN, EMAIL , CUSTOMER_ID )" +
                " VALUES ( @USER_ID , @NOME , @COGNOME , @LOGIN , @EMAIL , @CUSTOMER_ID )";

            System.Data.Common.DbCommand command;
            command = mConnection.CreateCommand();
            command.CommandText = mStrSQL;

            mAddParameter(command, "@USER_ID", userId);
            mAddParameter(command, "@NOME", nome.Trim());
            mAddParameter(command, "@COGNOME", cognome.Trim());
            mAddParameter(command, "@LOGIN", mylogin.Trim());
            mAddParameter(command, "@EMAIL", email.Trim());

            if (customerId == -1)
            {
                mAddParameter(command, "@CUSTOMER_ID", DBNull.Value);
            }
            else
            {
                mAddParameter(command, "@CUSTOMER_ID", customerId);
            }

            mExecuteNoQuery(command);

            return mGetIdentity();
        }



        public long rispondi(long trattativaId, long userId, string testo)
        {
            TrattativaManager tManager = new TrattativaManager(mConnection);
            return tManager.rispondi(trattativaId, userId, testo);
        }

        public long rispondi(long trattativaId, long userId, string testo, long risposta_id)
        {
            TrattativaManager tManager = new TrattativaManager(mConnection);
            return tManager.rispondi(trattativaId, userId, testo, risposta_id);
        }


    }
}


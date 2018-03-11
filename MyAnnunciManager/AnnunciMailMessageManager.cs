using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annunci
{
    public class AnnunciMailMessageManager : MyManagerCSharp.MailMessageManager
    {
        /* public enum Lingua
         {
             IT = 0,
             EN = 1
         }

         protected string _http;
         protected string _applicationName;
 */

        public AnnunciMailMessageManager(string applicationName, string http)
             : base(applicationName, http)
        {

        }



        public string getBodyDonazione(string url)
        {
            string temp = "";
            temp = "<h1>" + _applicationName + "</h1>" +
        "Gentile utente,  " +
        "<br /> Sperando che il servizio offerto sia stato gradito, ti ricordiamo la possibilità di eseguire una donazione in modo sicuro e protetto, tramite il servizio di PAYPAL" +
        "<p>Fai una donazione, grande o piccola che sia.</p>" +
        "<p>Il tuo contributo ci aiuterà a mantenere il servizio gratuito e a ripagare le spese di gestione del dominio e dello spazio web.</p>" +
        "<br /> Clicca <a href=\"" + System.Configuration.ConfigurationManager.AppSettings["application.url"] + "{0}\">qui</a> per effettuare una donazione.<br /><br />";

            temp = String.Format(temp, url);

            temp += getFirma(Lingua.IT);

            return temp;
        }




        public string getBodyModificaTestoAnnuncio(long annuncio_id, string titoloAnnuncio, string url)
        {
            //una volta inserito un post invio un'email a chi ha aperto il Thread

            string temp = "";
            temp = "<h1>" + _applicationName + "</h1>" +
            " <p>Gentile utente,  " +
            "<br /> Il proprietartio dell'annuncio \"{0}\" ha modificato il testo della descrizione." +
            "<br /> " +
            "<br /> Clicca <a href=\"" + System.Configuration.ConfigurationManager.AppSettings["application.url"] + "{1}\">qui</a> per visualizzare le modiche apportate all'annuncio .<br /><br />";

            temp = String.Format(temp, titoloAnnuncio, url);

            temp += getFirma(Lingua.IT);

            return temp;
        }


        public string getBodyAggiornamentoPrezzoAnnuncio(long annuncio_id, string titoloAnnuncio, decimal vecchioPrezzo, decimal nuovoPrezzo, string url)
        {
            //una volta inserito un post invio un'email a chi ha aperto il Thread

            string temp = "";
            temp = "<h1>" + _applicationName + "</h1>" +
            " <p>Gentile utente,  " +
            "<br /> Il proprietartio dell'annuncio \"{0}\" ha modificato il prezzo dell'annuncio." +
            "<br /> " +
            "<p> " + String.Format("Vecchio prezzo: € {0:N2}", vecchioPrezzo) + "</p>" +
            "<p> " + String.Format("Nuovo prezzo: € {0:N2}", nuovoPrezzo) + "</p>" +
            "<br /> " +
            "<br /> Clicca <a href=\"" + System.Configuration.ConfigurationManager.AppSettings["application.url"] + "{1}\">qui</a> per visualizzare le modiche apportate all'annuncio .<br /><br />";

            temp = String.Format(temp, titoloAnnuncio, url);

            temp += getFirma(Lingua.IT);

            return temp;
        }




        public string getBodyCancellaAnnuncio(string titoloAnnuncio, string url)
        {
            string temp = "";
            temp = "<h1> " + _applicationName + "</h1>" +
            " <p>Gentile utente,  </p>" +
            "<br />ti segnaliamo la cancellazione dell'annuncio per: {0} " +
            "<br />" +
            "<p>Di conseguenza la compravendita è stata interrotta. </p> " +
            "<br /> Clicca <a href=\"" + System.Configuration.ConfigurationManager.AppSettings["application.url"] + "{1}\">qui</a> per eliminare la compravendita dalle tue trattative.<br /><br />";

            temp = String.Format(temp, titoloAnnuncio, url);

            temp += getFirma(Lingua.IT);

            return temp;
        }


        public string getBodyNuovoMessaggioReply(long trattativaId, long annuncioId, string titoloAnnuncio, string url)
        {
            //una volta inserita una risposta invio un'email a chi ha inserito l'annucio

            string temp;
            temp = "<h1>Nuovo messaggio</h1>" +
            " <p>Gentile utente,  " +
            "<br /> hai ricevuto un nuovo messaggio per l'annuncio: {1} " +
            "<br />" +
            "<br /> Clicca <a href=\"" + System.Configuration.ConfigurationManager.AppSettings["application.url"] + "{3}\" > qui </a> per visualizzare la risposta.<br /><br />";

            temp = String.Format(temp, trattativaId, titoloAnnuncio, annuncioId, url);

            temp += getFirma(Lingua.IT);

            return temp;
        }

        public string getBodyInserimentoNuovoAnnuncio(long annuncioId, string titoloAnnuncio, string url)
        {
            string temp = "";
            temp = "<h1>Nuovo annuncio</h1>" +
            " <p>Gentile utente,  " +
            "<br /> ti ricordiamo che il {1} hai inseriro l'annuncio per: {0} " +
            "<br />" +
            "<br /> Clicca <a href=\"{2}{3}\">qui</a> per visualizzare l'annuncio.<br /><br />";

            temp = String.Format(temp, titoloAnnuncio, DateTime.Now.ToShortDateString(), System.Configuration.ConfigurationManager.AppSettings["application.url"], url);

            temp += getFirma(Lingua.IT);

            return temp;
        }


    }
}

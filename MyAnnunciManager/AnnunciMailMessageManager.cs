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



        public string getBodyModificaTestoAnnuncio(long annuncio_id, string titoloAnnuncio)
        {
            //una volta inserito un post invio un'email a chi ha aperto il Thread

            string temp = "";
            temp = "<h1>" + _applicationName + "</h1>" +
            " <p>Gentile utente,  " +
            "<br /> Il proprietartio dell'annuncio \"{0}\" ha modificato il testo della descrizione." +
            "<br /> " +
            "<br /> Clicca <a href=\"" + System.Configuration.ConfigurationManager.AppSettings["application.url"] + "Immobiliare/Details/{1}\">qui</a> per visualizzare le modiche apportate all'annuncio .<br /><br />";

            temp = String.Format(temp, titoloAnnuncio, annuncio_id);

            temp += getFirma(Lingua.IT);

            return temp;
        }


        public string getBodyAggiornamentoPrezzoAnnuncio(long annuncio_id, string titoloAnnuncio, decimal vecchioPrezzo, decimal nuovoPrezzo)
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
            "<br /> Clicca <a href=\"" + System.Configuration.ConfigurationManager.AppSettings["application.url"] + "Immobiliare/Details/{1}\">qui</a> per visualizzare le modiche apportate all'annuncio .<br /><br />";

            temp = String.Format(temp, titoloAnnuncio, annuncio_id);

            temp += getFirma(Lingua.IT);

            return temp;
        }




        public string getBodyCancellaAnnuncio(string titoloAnnuncio)
        {
            string temp = "";
            temp = "<h1> " + _applicationName + "</h1>" +
            " <p>Gentile utente,  </p>" +
            "<br />ti segnaliamo la cancellazione dell'annuncio per: {0} " +
            "<br />" +
            "<p>Di conseguenza la compravendita è stata interrotta. </p> " +
            "<br /> Clicca <a href=\"" + System.Configuration.ConfigurationManager.AppSettings["application.url"] + "immobiliare/MyAnnunci\">qui</a> per eliminare la compravendita  dalle tue trattative.<br /><br />";

            temp = String.Format(temp, titoloAnnuncio);

            temp += getFirma(Lingua.IT);

            return temp;
        }


        public string getBodyNuovoMessaggioReply(long trattativaId, long annuncioId, string titoloAnnuncio)
        {
            //una volta inserita una risposta invio un'email a chi ha inserito l'annucio

            string temp;
            temp = "<h1>Nuovo messaggio</h1>" +
            " <p>Gentile utente,  " +
            "<br /> hai ricevuto un nuovo messaggio per l'annuncio: {1} " +
            "<br />" +
            "<br /> Clicca <a href=\"" + System.Configuration.ConfigurationManager.AppSettings["application.url"] + "Libri/trattativa.aspx?trattativaId={0}&annuncioId={2}\" > qui </a> per visualizzare la risposta.<br /><br />";

            temp = String.Format(temp, trattativaId, titoloAnnuncio, annuncioId);

            temp += getFirma(Lingua.IT);

            return temp;
        }


    }
}

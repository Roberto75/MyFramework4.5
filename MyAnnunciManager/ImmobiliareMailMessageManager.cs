using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Annunci
{
    public class ImmobiliareMailMessageManager : AnnunciMailMessageManager
    {

        public ImmobiliareMailMessageManager(string applicationName, string http)
              : base(applicationName, http)
        {
            mIV = Immobiliare.ImmobiliareSecurityManager.IV;
            mKey = Immobiliare.ImmobiliareSecurityManager.Key;
        }

        /*
                public string getFirma(Lingua lingua)
                {
                    string risulato = "";

                    switch (lingua)
                    {
                        case Lingua.IT:
                            risulato = "<br />Cordiali saluti dallo staff di " + _applicationName + "." + Environment.NewLine +
                              "<br><br><br> " + _applicationName + Environment.NewLine +
                              "<br><a href=\"" + _http + "\">" + _http + "</a>" + Environment.NewLine;
                            break;
                    }

                    return risulato;

                }



                public string getBodyImmobiliareModificaTestoAnnuncio(long annuncio_id, string titoloAnnuncio)
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
                }*/

    }
}

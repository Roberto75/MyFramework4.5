using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyManagerCSharp
{
    public class MailMessageManager : MyManagerCSharp.MailManager
    {
        public enum Lingua
        {
            IT = 0,
            EN = 1
        }

        protected string _http;
        protected string _applicationName;


        public MailMessageManager(string applicationName, string http)
        {
            _applicationName = applicationName;
            _http = http;
        }


        public string getFirma(Lingua lingua)
        {
            string risulato = "";

            switch (lingua)
            {
                case Lingua.IT:
                    risulato = "<br />Cordiali saluti dallo staff di " + _applicationName + "." + Environment.NewLine +
                                   "<br /><a href=\"" + _http + "\">" + _http + "</a>" + Environment.NewLine;
                    break;
            }

            return risulato;

        }


        #region "Utenti"


        public string getBodyAccessDenied(long userId, string login, string email, string ip)
        {
            string temp = "";

            Lingua lingua = Lingua.IT;

            switch (lingua)
            {
                case Lingua.IT:
                case Lingua.EN:

                    temp = "<h1>Access denied  in" + System.Net.Dns.GetHostName() + "</h1>" + Environment.NewLine +

                    "<br /> Si è verificato un tentativo di accesso ad una risorsa a cui non si posseggono le autorizzazioni:" +
                    "<br />" + Environment.NewLine +
                    "<br /> <b>Login</b>: " + login + Environment.NewLine +
                    "<br /> <b>User Id</b>: " + userId + Environment.NewLine +
                    "<br /> <b>Email</b>: " + email + Environment.NewLine +

                    "<br />" + Environment.NewLine;
                    break;
            }


            temp += getFirma(lingua);
            return temp;
        }


        public string getBodyRegistrazioneUtente(string nome, string cognome, string login, string email, Lingua lingua)
        {

            string temp = "";
            string nome_cognome = "";

            if (String.IsNullOrEmpty(cognome))
            {
                nome_cognome = nome;
            }
            else
            {
                nome_cognome = nome + " " + cognome;
            }

            switch (lingua)
            {
                case Lingua.IT:
                case Lingua.EN:

                    temp = "<h1>Registrazione al Portale di " + _applicationName + "</h1>" + Environment.NewLine +
                    "Gentile " + nome_cognome + ",  " + Environment.NewLine +
                    "<br /> benvenuto come utente registrato del Portale di " + _applicationName + ". Per accedere al Portale utilizza le seguenti credenziali, digitando correttamente lettere maiuscole e minuscole:" +
                    "<br />" + Environment.NewLine +
                    "<br /> <b>Login</b>: " + login + Environment.NewLine +
                    "<br />" + Environment.NewLine +
                    "<p>Per questione di sicurezza riceverai una <b>seconda email con una password</b> generata in automatico dal sistema.</p>" + Environment.NewLine;

                    temp += "<p>Se dopo qualche minuto non ricevi la seconda email con la password, controlla nella tua cartella della \"Posta indesiderata\". Abbiamo notato che alcuni servizi di antispam posso filtrare la nostra email.</p>" + Environment.NewLine;


                    if (bool.Parse(System.Configuration.ConfigurationManager.AppSettings["utenti.password.canSet"]))
                    {
                        temp += "<p>In seguito, se lo desideri, potrai effettuare il login e accedere al menu \"Utente\" per impostare una password in modo che sia più semplice ricordarla per te. </p>" + Environment.NewLine +
                                 "<br /> " + Environment.NewLine;
                    }

                    temp += "<p>In un secondo momento, dopo esserti loggato nel sistema, potrai sempre accedere al menu \"Utente\" per <b>modificare e aggiornare i tuoi dati anagrafici</b>.</p>" + Environment.NewLine;


                    temp += "<p>Ti rammentiamo che come utente registrato puoi accedere gratuitamente a tutti servizi on line individuali del Portale.</p>" + Environment.NewLine +
                    "<p>Conserva o stampa questa mail come promemoria. In caso di smarrimento della password potrai, comunque, utilizzare la funzione \"Reset password\" presente nel menu \"Utente\".</p>" + Environment.NewLine;
                    break;
            }


            temp += getFirma(lingua);
            return temp;
        }






        public string getBodyRegistrazioneUtente(string nome, string cognome, string login, string email, string telefono, string indirizzo, string numeroCivico, string cap, string provincia, string comune, Lingua lingua)
        {

            string temp = "";
            string nome_cognome = "";

            if (String.IsNullOrEmpty(cognome))
            {
                nome_cognome = nome;
            }
            else
            {
                nome_cognome = nome + " " + cognome;
            }

            switch (lingua)
            {
                case Lingua.IT:
                case Lingua.EN:

                    temp = "<h1>Registrazione al Portale di " + _applicationName + "</h1>" + Environment.NewLine +
                    "Gentile " + nome_cognome + ",  " + Environment.NewLine +
                    "<br /> benvenuto come utente registrato del Portale di " + _applicationName + ". Per accedere al Portale utilizza le seguenti credenziali, digitando correttamente lettere maiuscole e minuscole:" +
                    "<br />" + Environment.NewLine +
                    "<br /> <b>Login</b>: " + login + Environment.NewLine +
                    "<br />" + Environment.NewLine +
                    "<p>Per questione di sicurezza riceverai una <b>seconda email con una password</b> generata in automatico dal sistema.</p>" + Environment.NewLine;

                    if (bool.Parse(System.Configuration.ConfigurationManager.AppSettings["utenti.password.canSet"]))
                    {
                        temp += "<p>In seguito, se lo desideri, potrai effettuare il login e accedere al menu \"Utente\" per impostare una password in modo che sia più semplice ricordarla per te. </p>" + Environment.NewLine +
                                 "<br /> " + Environment.NewLine;
                    }

                    temp += "<p>In un secondo momento potrai sempre accedere al menu \"Utente\" per <b>modificare e aggiornare i tuoi dati anagrafici</b> dopo esserti loggato nel sistema.</p>" + Environment.NewLine +
                    "<br />" + Environment.NewLine +
                    "<p>Email: " + email + " </p>" + Environment.NewLine +
                    "<p>Telefono: " + telefono + " </p>" + Environment.NewLine +
                    "<br />" + Environment.NewLine +
                    "<p>Indirizzo: " + indirizzo + " n° " + numeroCivico + ", " + cap + " - " + comune + " (" + provincia + ") </p>" + Environment.NewLine +
                    "<br /> " + Environment.NewLine +
                    "<p>Ti rammentiamo che come utente registrato puoi accedere gratuitamente a tutti servizi on line individuali del Portale.</p>" + Environment.NewLine +
                    "<p>Conserva o stampa questa mail come promemoria. In caso di smarrimento della password potrai, comunque, utilizzare la funzione \"Genera password\" presente nel menu \"Utente\".</p>" + Environment.NewLine;
                    break;
            }


            temp += getFirma(lingua);
            return temp;
        }


        public virtual string getBodyResetPassword(string nome, string cognome, string passwordGenerata, Lingua lingua)
        {
            string temp = "";

            switch (lingua)
            {
                case Lingua.IT:
                case Lingua.EN:
                    temp = "<h1>Generazione nuova password</h1>" + Environment.NewLine +
                    "Gentile " + nome + " " + cognome + "," + Environment.NewLine +
                    "<br /> come richiesto dal servizio \"Genera Password\" ti inviamo questo messaggio con le tue credenziali." + Environment.NewLine +
                    "<p>Per accedere al Portale utilizza le seguenti credenziali, digitando correttamente lettere maiuscole e minuscole: </p>" + Environment.NewLine +
                    "<br />" + Environment.NewLine +
                    "<br/> Password: <b>" + passwordGenerata + "</b> <br /> " + Environment.NewLine +
                    "<p> In seguito potrai accedere al menu \"Utente\" e alla funzione \"Modifica password\" per impostare una password in modo che sia più semplice ricordarla per te.</p>" + Environment.NewLine +
                    "<p> Per questione di sicurezza le consigliamo di modificare periodicamnte la tua password tramite la funzione \"Modifica password\" del menu Utente. </p>" + Environment.NewLine +
                    "<p> Ti rammentiamo che come utente registrato puoi accedere gratuitamente a tutti servizi on line individuali del Portale. </p>" + Environment.NewLine +
                    "<p> Conserva o stampa questa mail come promemoria. In caso di smarrimento della password potrai, comunque, utilizzare la funzione \"Genera password\" presente nel menu \"Utente\". </p>" + Environment.NewLine;
                    break;
            }
            temp += getFirma(lingua);

            return temp;
        }


        public virtual string getBodyUpdatePassword(string nome, string cognome, Lingua lingua)
        {
            string temp = "";

            switch (lingua)
            {
                case Lingua.IT:
                case Lingua.EN:
                    temp = "<h1>Modifica della password</h1>" + Environment.NewLine +
                    "Gentile " + nome + " " + cognome + "," + Environment.NewLine +
                    "<br /> Ti comunichiamo che la password per del tuo account di " + _applicationName + " è stata modificata." + Environment.NewLine +
                    "<br /> Se sei stato tu a modificare la password non devi fare nulla." + Environment.NewLine +
                    "<br />" + Environment.NewLine +
                    "<br /> Se invece non sei stato tu a modificare la password utilizza la funzione di \"Reset password\" per creare una nuova password." + Environment.NewLine;

                    //temp += "<p>Clicca sul link per accedere al <a href=\"" + System.Configuration.ConfigurationManager.AppSettings["application.url"] + "admin/\" >Pannello di amministrazione<a></p>" + Environment.NewLine;

                    break;
            }
            temp += getFirma(lingua);

            return temp;
        }


        public string getBodyAlertNewAccount(string nome, string cognome, string login, long userId, string profilo, Lingua lingua)
        {
            string temp = "";

            switch (lingua)
            {
                case Lingua.IT:
                case Lingua.EN:

                    temp = "<h1>Attivazione nuovo account</h1>" + Environment.NewLine +
                    " Gentile amministratore," + Environment.NewLine +
                    "<br/>La informiamo che il " + DateTime.Now.ToShortDateString() + " un nuovo utente si è registrato al portale." + Environment.NewLine +
                    "<p>Per consentire l'accesso al nuovo utente occorre accedere al pannello di amministrazione degli utenti e abilitare il nuovo account.</p>" + Environment.NewLine +
                    "<p>Di seguito vengono riportati i dati nel nuovo utente per poterlo individuare e abilitare:</p>" + Environment.NewLine;


                    if (!String.IsNullOrEmpty(profilo))
                    {
                        temp = temp + "<br />Profilo : " + profilo + Environment.NewLine;
                    }

                    temp += "<br />Nome: " + nome + Environment.NewLine +
                    "<br />Cognome: " + cognome + Environment.NewLine +
                    "<br />Login: " + login + Environment.NewLine +
                    "<br />User ID: " + userId + Environment.NewLine +
                    "<p>Clicca sul link per accedere al <a href=\"" + System.Configuration.ConfigurationManager.AppSettings["application.url"] + "admin/\" >Pannello di amministrazione<a></p>" + Environment.NewLine;
                    break;
            }
            temp += getFirma(lingua);

            return temp;
        }

        #endregion

        #region "Ticket"

        public string getBodyTicketReply(string login, long ticketId, string title)
        {
            string temp = "";

            Lingua lingua = Lingua.IT;

            switch (lingua)
            {
                case Lingua.IT:
                case Lingua.EN:

                    temp = "<h1>" + login + " ha commentato il ticket: " + title + "</h1>" + Environment.NewLine +
                     "<p>" + login + " ha commentato il ticket: " + title + " </p>" + Environment.NewLine +
                    "<p>Clicca sul link per accedere al ticket <a href=\"" + System.Configuration.ConfigurationManager.AppSettings["application.url"] + "Ticket/Details/" + ticketId + "\" >Visualizza il ticket</a></p>" + Environment.NewLine;
                    break;
            }
            temp += getFirma(lingua);

            return temp;
        }

        #endregion

    }
}

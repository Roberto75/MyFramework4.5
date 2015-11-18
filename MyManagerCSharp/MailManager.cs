using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyManagerCSharp
{
    [CLSCompliant(true)]
    public class MailManager
    {
        //chiavi per cifrare e decifrare le credenziali persenti nel file di configurazione
        protected string _key;
        protected string _IV;



        //Roberto Rutigliano 26/12/2007
        //http://www.systemnetmail.com/
        //Invio email tramite autenticazone 
        //<add key="mail.server.userName" value=""/>
        //<add key="mail.server.password"  value=""/>  


        //Roberto Rutigliano 04/12/2007
        //Gestione degli Attachments
        //posso aggiungere delle Stringhe che rappresentano il path assoluto dell'allegato
        //oppure direttamente un memoryStram

        #region "TO"

        private List<System.Net.Mail.MailAddress> p_To = new List<System.Net.Mail.MailAddress>();


        public void _To(string address)
        {
            if (!String.IsNullOrEmpty(address))
            {
                //25/10/2010 Roberto Rutigliano ho più indirizzi separati da ;

                string[] myArray = address.Split(';');

                string temp;
                foreach (string value in myArray)
                {
                    if (!String.IsNullOrEmpty(value))
                    {
                        temp = value.Replace("\"", "");
                        p_To.Add(new System.Net.Mail.MailAddress(temp));
                    }

                }

            }

        }

        public void _To(string address, string displayName)
        {
            if (!String.IsNullOrEmpty(address))
            {
                p_To.Add(new System.Net.Mail.MailAddress(address, displayName));
            }

        }

        public void _To(System.Net.Mail.MailAddress item)
        {
            p_To.Add(item);
        }


        public void _ToClearField()
        {
            p_To.Clear();
        }
        #endregion



        #region "FROM"

        private System.Net.Mail.MailAddress p_From = null;

        public void _From(string address)
        {
            if (!String.IsNullOrEmpty(address))
            {
                p_From = new System.Net.Mail.MailAddress(address);
            }
        }


        public void _From(string address, string displayName)
        {
            if (!String.IsNullOrEmpty(address))
            {
                p_From = new System.Net.Mail.MailAddress(address, displayName);
            }

        }

        public void _From(System.Net.Mail.MailAddress item)
        {
            p_From = item;
        }



        #endregion



        #region "Cc"

        private List<System.Net.Mail.MailAddress> p_Cc = new List<System.Net.Mail.MailAddress>();

        public void _Cc(string address)
        {
            if (!String.IsNullOrEmpty(address))
            {
                //25/10/2010 Roberto Rutigliano ho più indirizzi separati da ;

                string[] myArray = address.Split(';');

                string temp;
                foreach (string value in myArray)
                {
                    if (!String.IsNullOrEmpty(value))
                    {
                        temp = value.Replace("\"", "");
                        p_Cc.Add(new System.Net.Mail.MailAddress(temp));
                    }

                }

            }

        }

        public void _Cc(string address, string displayName)
        {
            if (!String.IsNullOrEmpty(address))
            {
                p_Cc.Add(new System.Net.Mail.MailAddress(address, displayName));
            }

        }

        public void _Cc(System.Net.Mail.MailAddress item)
        {
            p_Cc.Add(item);
        }


        public void _CcClearField()
        {
            p_Cc.Clear();
        }

        #endregion



        #region "Bcc"

        private List<System.Net.Mail.MailAddress> p_Bcc = new List<System.Net.Mail.MailAddress>();


        public void _Bcc(string address)
        {
            if (!String.IsNullOrEmpty(address))
            {
                //25/10/2010 Roberto Rutigliano ho più indirizzi separati da ;

                string[] myArray = address.Split(';');

                string temp;
                foreach (string value in myArray)
                {
                    if (!String.IsNullOrEmpty(value))
                    {
                        temp = value.Replace("\"", "");
                        p_Bcc.Add(new System.Net.Mail.MailAddress(temp));
                    }

                }

            }

        }

        public void _Bcc(string address, string displayName)
        {
            if (!String.IsNullOrEmpty(address))
            {
                p_Bcc.Add(new System.Net.Mail.MailAddress(address, displayName));
            }

        }

        public void _Bcc(System.Net.Mail.MailAddress item)
        {
            p_Bcc.Add(item);
        }


        public void _BccClearField()
        {
            p_Bcc.Clear();
        }

        #endregion




        public string _Subject;

        public string _Body;

        public string _MailServer;

        public List<System.Net.Mail.Attachment> _Attachments;



        public string send()
        {

            if (!bool.Parse(System.Configuration.ConfigurationManager.AppSettings["mail.isEnabled"]))
            {
                return "mail.isEnabled == FALSE";
            }

            System.Net.Mail.MailMessage MyMail = new System.Net.Mail.MailMessage();


            if (p_From == null)
            {
                if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["mail.From.displayName"]))
                {
                    this._From(System.Configuration.ConfigurationManager.AppSettings["mail.From"], System.Configuration.ConfigurationManager.AppSettings["mail.From.displayName"]);
                }
                else
                {
                    this._From(System.Configuration.ConfigurationManager.AppSettings["mail.From"]);
                }
            }

            MyMail.From = p_From;


            foreach (System.Net.Mail.MailAddress item in p_To)
            {
                MyMail.To.Add(item);
            }


            foreach (System.Net.Mail.MailAddress item in p_Cc)
            {
                MyMail.CC.Add(item);
            }

            foreach (System.Net.Mail.MailAddress item in p_Bcc)
            {
                MyMail.Bcc.Add(item);
            }

            //*** SPAMMING: G.a.p.p.y - T.e.x.t
            MyMail.Subject = _Subject.Replace(".", "");

            if (_Body.ToLower().StartsWith("<html>"))
            {
                MyMail.Body = _Body;
            }
            else
            {
                MyMail.Body = "<html><body>" + _Body + "</body></html>";
            }

            MyMail.IsBodyHtml = true;

            //*** Attachment ***
            if (_Attachments != null)
            {
                foreach (System.Net.Mail.Attachment attachment in _Attachments)
                {
                    MyMail.Attachments.Add(attachment);
                }
            }

            if (String.IsNullOrEmpty(_MailServer))
            {
                //leggo il nome del mail server dal file Web.Config
                _MailServer = System.Configuration.ConfigurationManager.AppSettings["mail.server"];
            }


            int port = 25;
            if (System.Configuration.ConfigurationManager.AppSettings["mail.server.port"] != null && !String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["mail.server.port"]))
            {
                port = int.Parse(System.Configuration.ConfigurationManager.AppSettings["mail.server.port"]);
            }

            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(_MailServer, port);

            if (System.Configuration.ConfigurationManager.AppSettings["mail.server.enableSsl"] != null && !String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["mail.server.enableSsl"]))
            {
                smtp.EnableSsl = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["mail.server.enableSsl"]);
            }

            if (System.Configuration.ConfigurationManager.AppSettings["mail.server.enableTls"] != null && !String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["mail.server.enableTls"]))
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls;
            }


            if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["mail.server.userName"]))
            {
                //'invio email tramite autenticazione

                string username;
                string password;

                if (System.Configuration.ConfigurationManager.AppSettings["mail.credentials.encrypted"] != null && bool.Parse(System.Configuration.ConfigurationManager.AppSettings["mail.credentials.encrypted"]))
                {
                    username = decript(System.Configuration.ConfigurationManager.AppSettings["mail.server.username"]);
                    password = decript(System.Configuration.ConfigurationManager.AppSettings["mail.server.password"]);

                }
                else
                {
                    username = System.Configuration.ConfigurationManager.AppSettings["mail.server.username"];
                    password = System.Configuration.ConfigurationManager.AppSettings["mail.server.password"];
                }
                smtp.Credentials = new System.Net.NetworkCredential(username, password);
            }

            else
            {
                smtp.UseDefaultCredentials = true;
            }


            string esito = "";
            try
            {
                smtp.Send(MyMail);
            }
            catch (System.Net.WebException ex)
            {
                esito = ex.Source + Environment.NewLine + ex.Message + Environment.NewLine;
                if (ex.InnerException != null)
                {
                    esito += ex.InnerException.Message + Environment.NewLine;
                }
                if (ex.InnerException.InnerException != null)
                {
                    esito += ex.InnerException.InnerException.Message + Environment.NewLine;
                }
                esito += ex.StackTrace;
            }
            catch (Exception ex)
            {
                esito = ex.Source + Environment.NewLine + ex.Message + Environment.NewLine;
                if (ex.InnerException != null)
                {
                    esito += ex.InnerException.Message + Environment.NewLine;
                }
                if (ex.InnerException != null && ex.InnerException.InnerException != null)
                {
                    esito += ex.InnerException.InnerException.Message + Environment.NewLine;
                }
                esito += ex.StackTrace;
            }

            return esito;

        }



        public string  sendException(Exception ex)
        {
           return sendException(ex, "");
        }

        public string sendException(Exception ex, string messaggio)
        {
            string temp;

            //*** BODY ***
            temp = "<html><body>";
            if (!String.IsNullOrEmpty(ex.Message))
            {
                temp += "<h2>Exception  </h2> " + ex.Message;
            }

            if (ex.InnerException != null)
            {
                temp += "<br /> <br /> <h2>Inner Exception</h2> " + ex.InnerException.Message;


                if (ex.InnerException.StackTrace != null)
                {
                    temp += "<br /> <br /> <h2>Inner Exception - Stack Trace</h2> " + ex.InnerException.StackTrace.ToString();
                }
            }


            if (ex.StackTrace != null)
            {
                temp += "<br /> <br /><h2>Stack Trace</h2> " + ex.StackTrace.ToString();
            }


            if (!String.IsNullOrEmpty(messaggio))
            {
                temp += "<br /> <br />  <h2>Messaggio </h2> " + messaggio;
            }


            temp += "</body></html>";

            _Body = temp;


            //in caso di errore invio l'email a me stesso e anche a .... se è presente nel file di configurazione 
            _ToClearField();
            p_To.Add(new System.Net.Mail.MailAddress(System.Configuration.ConfigurationManager.AppSettings["mail.From"]));

            if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["mail.To.Ccn"]))
            {
                p_Bcc.Add(new System.Net.Mail.MailAddress(System.Configuration.ConfigurationManager.AppSettings["mail.To.Ccn"]));
            }


            if ((ex.Source == null) || String.IsNullOrEmpty(ex.Source))
            {
                _Subject = System.Net.Dns.GetHostName() + " - Exception ";
            }
            else
            {
                _Subject = System.Net.Dns.GetHostName() + " - Exception: " + ex.Source;
            }

           return send();
        }


        public static string send(Exception ex)
        {
            return MailManager.send(ex, "", "", "");
        }

        public static string send(Exception ex, string messaggio)
        {
            if (System.Configuration.ConfigurationManager.AppSettings["mail.credentials.encrypted"] != null && bool.Parse(System.Configuration.ConfigurationManager.AppSettings["mail.credentials.encrypted"]))
            {

                throw new ApplicationException("Utilizzare il metodo send passando le credenziali di accesso decifrate");
                //  username = decript(System.Configuration.ConfigurationManager.AppSettings["mail.server.username"]);
                //password = decript(System.Configuration.ConfigurationManager.AppSettings["mail.server.password"]);
            }

            string username;
            string password;
            username = System.Configuration.ConfigurationManager.AppSettings["mail.server.username"];
            password = System.Configuration.ConfigurationManager.AppSettings["mail.server.password"];

            return MailManager.send(ex, messaggio, username, password);
        }

        public static string send(Exception ex, string messaggio, string username, string password)
        {
            if (!bool.Parse(System.Configuration.ConfigurationManager.AppSettings["mail.isEnabled"]))
            {
                return "mail.isEnabled == FALSE";
            }

            string temp;

            //*** BODY ***
            temp = "<html><body>";

            if (!String.IsNullOrEmpty(ex.Message))
            {
                temp += "<h2>Exception  </h2> " + ex.Message;
            }

            if (ex.InnerException != null)
            {
                temp += "<br /> <br /> <h2>Inner Exception</h2> " + ex.InnerException.Message;


                if (ex.InnerException.StackTrace != null)
                {
                    temp += "<br /> <br /> <h2>Inner Exception - Stack Trace</h2> " + ex.InnerException.StackTrace.ToString();
                }
            }


            if (ex.StackTrace != null)
            {
                temp += "<br /> <br /><h2>Stack Trace</h2> " + ex.StackTrace.ToString();
            }


            if (!String.IsNullOrEmpty(messaggio))
            {
                temp += "<br /> <br />  <h2>Messaggio </h2> " + messaggio;
            }


            temp += "</body></html>";




            System.Net.Mail.MailMessage MyMail = new System.Net.Mail.MailMessage();


            if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["mail.From.displayName"]))
            {
                MyMail.From = new System.Net.Mail.MailAddress(System.Configuration.ConfigurationManager.AppSettings["mail.From"], System.Configuration.ConfigurationManager.AppSettings["mail.From.displayName"]);
            }
            else
            {
                MyMail.From = new System.Net.Mail.MailAddress(System.Configuration.ConfigurationManager.AppSettings["mail.From"]);
            }

            //!!!!!!!!!!!!!!!!!!!!
            //in caso di errore invio l'email a me stesso e anche a .... se è presente nel file di configurazione 
            MyMail.To.Add(new System.Net.Mail.MailAddress(System.Configuration.ConfigurationManager.AppSettings["mail.From"]));



            if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["mail.To.Ccn"]))
            {
                MyMail.Bcc.Add(new System.Net.Mail.MailAddress(System.Configuration.ConfigurationManager.AppSettings["mail.To.Ccn"]));
            }



            if ((ex.Source == null) || String.IsNullOrEmpty(ex.Source))
            {
                MyMail.Subject = System.Net.Dns.GetHostName() + " - Exception ";
            }
            else
            {
                MyMail.Subject = System.Net.Dns.GetHostName() + " - Exception: " + ex.Source;
            }


            //*** SPAMMING: G.a.p.p.y - T.e.x.t
            MyMail.Subject = MyMail.Subject.Replace(".", "");


            MyMail.Body = temp;


            MyMail.IsBodyHtml = true;




            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(System.Configuration.ConfigurationManager.AppSettings["mail.server"]);

            //if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["mail.server.userName"]))
            if (!String.IsNullOrEmpty(username))
            {
                //'invio email tramite autenticazione
                smtp.Credentials = new System.Net.NetworkCredential(username, password);
            }
            else
            {
                smtp.UseDefaultCredentials = true;
            }


            string esito = "";

            try
            {
                smtp.Send(MyMail);
            }
            catch (Exception exx)
            {
                esito = exx.Source + Environment.NewLine + exx.Message + Environment.NewLine + exx.StackTrace;
            }

            return esito;
        }


        private string decript(string cipherTextBase64)
        {
            if (String.IsNullOrEmpty(_key))
                throw new ArgumentNullException("Decript key is NULL, usare il metodo setKey");

            if (String.IsNullOrEmpty(_IV))
                throw new ArgumentNullException("Decript IV is NULL, usare il metodo setKey");

            return MyManagerCSharp.SecurityManager.AESDecryptSFromBase64String(cipherTextBase64, System.Text.UTF8Encoding.UTF8.GetBytes(_key), System.Text.UTF8Encoding.UTF8.GetBytes(_IV));
        }


        public void setKey(string key, string IV)
        {
            _key = key;
            _IV = IV;
        }




    }
}

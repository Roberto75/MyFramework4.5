using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MyManagerCSharp
{
    public class MailManager
    {
        //chiavi per cifrare e decifrare le credenziali persenti nel file di configurazione
        protected string mKey;
        protected string mIV;



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

        private List<System.Net.Mail.MailAddress> _to = new List<System.Net.Mail.MailAddress>();

        public void To(string address)
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
                        _to.Add(new System.Net.Mail.MailAddress(temp));
                    }

                }

            }

        }

        public void To(string address, string displayName)
        {
            if (!String.IsNullOrEmpty(address))
            {
                _to.Add(new System.Net.Mail.MailAddress(address, displayName));
            }

        }

        public void To(System.Net.Mail.MailAddress item)
        {
            _to.Add(item);
        }

        public void ToClearField()
        {
            _to.Clear();
        }
        #endregion



        #region "FROM"

        private System.Net.Mail.MailAddress _from = null;

        public void From(string address)
        {
            if (!String.IsNullOrEmpty(address))
            {
                _from = new System.Net.Mail.MailAddress(address);
            }
        }


        public void From(string address, string displayName)
        {
            if (!String.IsNullOrEmpty(address))
            {
                _from = new System.Net.Mail.MailAddress(address, displayName);
            }

        }

        public void From(System.Net.Mail.MailAddress item)
        {
            _from = item;
        }



        #endregion



        #region "Cc"

        private List<System.Net.Mail.MailAddress> _cc = new List<System.Net.Mail.MailAddress>();

        public void Cc(string address)
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
                        _cc.Add(new System.Net.Mail.MailAddress(temp));
                    }

                }

            }

        }

        public void Cc(string address, string displayName)
        {
            if (!String.IsNullOrEmpty(address))
            {
                _cc.Add(new System.Net.Mail.MailAddress(address, displayName));
            }

        }

        public void Cc(System.Net.Mail.MailAddress item)
        {
            _cc.Add(item);
        }


        public void CcClearField()
        {
            _cc.Clear();
        }

        #endregion



        #region "Bcc"

        private List<System.Net.Mail.MailAddress> _bcc = new List<System.Net.Mail.MailAddress>();


        public void Bcc(string address)
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
                        _bcc.Add(new System.Net.Mail.MailAddress(temp));
                    }

                }

            }

        }

        public void Bcc(string address, string displayName)
        {
            if (!String.IsNullOrEmpty(address))
            {
                _bcc.Add(new System.Net.Mail.MailAddress(address, displayName));
            }

        }

        public void Bcc(System.Net.Mail.MailAddress item)
        {
            _bcc.Add(item);
        }


        public void BccClearField()
        {
            _bcc.Clear();
        }

        #endregion




        public string Subject { get; set; }

        public string Body { get; set; }

        public string MailServer { get; set; }

        public List<System.Net.Mail.Attachment> Attachments { get; set; }

        public int? port { get; set; }

        public bool? enableSsl { get; set; }

        public bool? enableTls { get; set; }



        public string send()
        {

            if (!bool.Parse(System.Configuration.ConfigurationManager.AppSettings["mail.isEnabled"]))
            {
                return "mail.isEnabled == FALSE";
            }

            System.Net.Mail.MailMessage MyMail = new System.Net.Mail.MailMessage();


            if (_from == null)
            {
                if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["mail.From.displayName"]))
                {
                    this.From(System.Configuration.ConfigurationManager.AppSettings["mail.From"], System.Configuration.ConfigurationManager.AppSettings["mail.From.displayName"]);
                }
                else
                {
                    this.From(System.Configuration.ConfigurationManager.AppSettings["mail.From"]);
                }
            }

            MyMail.From = _from;


            foreach (System.Net.Mail.MailAddress item in _to)
            {
                MyMail.To.Add(item);
            }


            foreach (System.Net.Mail.MailAddress item in _cc)
            {
                MyMail.CC.Add(item);
            }

            foreach (System.Net.Mail.MailAddress item in _bcc)
            {
                MyMail.Bcc.Add(item);
            }

            //*** SPAMMING: G.a.p.p.y - T.e.x.t
            MyMail.Subject = Subject.Replace(".", "");

            if (Body.ToLower().StartsWith("<html>"))
            {
                MyMail.Body = Body;
            }
            else
            {
                MyMail.Body = "<html><body>" + Body + "</body></html>";
            }

            MyMail.IsBodyHtml = true;

            //*** Attachment ***
            if (Attachments != null)
            {
                foreach (System.Net.Mail.Attachment attachment in Attachments)
                {
                    MyMail.Attachments.Add(attachment);
                }
            }

            if (String.IsNullOrEmpty(MailServer))
            {
                //leggo il nome del mail server dal file Web.Config
                MailServer = System.Configuration.ConfigurationManager.AppSettings["mail.server"];
            }



            if (port == null)
            {
                if (System.Configuration.ConfigurationManager.AppSettings["mail.server.port"] != null && !String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["mail.server.port"]))
                {
                    port = int.Parse(System.Configuration.ConfigurationManager.AppSettings["mail.server.port"]);
                }
                else
                {
                    port = 25; //valore di default
                }
            }

            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(MailServer, (int)port);


            if (enableSsl == null)
            {
                if (System.Configuration.ConfigurationManager.AppSettings["mail.server.enableSsl"] != null && !String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["mail.server.enableSsl"]))
                {
                    enableSsl = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["mail.server.enableSsl"]);
                }
                else
                {
                    enableSsl = false; //valore di default
                }
            }
            smtp.EnableSsl = (bool)enableSsl;



            if (enableTls == null)
            {
                if (System.Configuration.ConfigurationManager.AppSettings["mail.server.enableTls"] != null && !String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["mail.server.enableTls"]))
                {
                    enableTls = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["mail.server.enableTls"]);
                }
            }

            if (enableTls == true)
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

                smtp.UseDefaultCredentials = false;
            }

            else
            {
                smtp.UseDefaultCredentials = true;
            }


            string esito = "";
            try
            {
                //"The operation has timed out."

                //100000
                //smtp.Timeout = 500000;
                Debug.WriteLine("smtp.Timeout = " + smtp.Timeout);
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
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


        public string sendException(Exception ex)
        {
            return sendException(ex, "");
        }

        public string sendException(Exception ex, string messaggio)
        {
            string temp;

            //*** BODY ***
            temp = "<html><body>";

            if (!String.IsNullOrEmpty(messaggio))
            {
                temp += "<br />  <h2>Messaggio </h2> " + messaggio;
            }


            temp += "<br />  <h2>" + ex.GetType().Name + "</h2> ";
            if (!String.IsNullOrEmpty(ex.Message))
            {
                temp += "<p>" + ex.Message + "</p>";
            }


            if (ex.StackTrace != null)
            {
                temp += "<br /> <br /><h2>Stack Trace</h2> " + ex.StackTrace.ToString();
            }


            if (ex.InnerException != null)
            {
                temp += "<br /> <br /> <h2>Inner Exception: " + ex.InnerException.GetType().Name + "</h2>";
                temp += "<p>" + ex.InnerException.Message + "</p>";

                if (ex.InnerException.StackTrace != null)
                {
                    temp += "<br /> <br /> <h2>Inner Exception - Stack Trace</h2> " + ex.InnerException.StackTrace.ToString();
                }
            }





            temp += "</body></html>";

            Body = temp;


            //in caso di errore invio l'email a me stesso e anche a .... se è presente nel file di configurazione 
            ToClearField();
            _to.Add(new System.Net.Mail.MailAddress(System.Configuration.ConfigurationManager.AppSettings["mail.From"]));

            if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["mail.To.Ccn"]))
            {
                _bcc.Add(new System.Net.Mail.MailAddress(System.Configuration.ConfigurationManager.AppSettings["mail.To.Ccn"]));
            }


            if ((ex.Source == null) || String.IsNullOrEmpty(ex.Source))
            {
                Subject = System.Net.Dns.GetHostName() + " - Exception ";
            }
            else
            {
                Subject = System.Net.Dns.GetHostName() + " - Exception: " + ex.Source;
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


            Exception t = ex;
            while (t != null)
            {
                if (!String.IsNullOrEmpty(ex.Message))
                {
                    temp += "<h2>Exception</h2> " + ex.Message + "<br />";
                }
                t = t.InnerException;
            }



            //if (ex.InnerException != null)
            //{
            //    temp += "<br /> <br /> <h2>Inner Exception</h2> " + ex.InnerException.Message;


            //    if (ex.InnerException.StackTrace != null)
            //    {
            //        temp += "<br /> <br /> <h2>Inner Exception - Stack Trace</h2> " + ex.InnerException.StackTrace.ToString();
            //    }
            //}





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
            if (String.IsNullOrEmpty(mKey))
                throw new ArgumentNullException("Decript key is NULL, usare il metodo setKey");

            if (String.IsNullOrEmpty(mIV))
                throw new ArgumentNullException("Decript IV is NULL, usare il metodo setKey");

            return MyManagerCSharp.SecurityManager.AESDecryptSFromBase64String(cipherTextBase64, System.Text.UTF8Encoding.UTF8.GetBytes(mKey), System.Text.UTF8Encoding.UTF8.GetBytes(mIV));
        }


        public void setKey(string key, string IV)
        {
            mKey = key;
            mIV = IV;
        }




    }
}

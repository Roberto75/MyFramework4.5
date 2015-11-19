using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyManagerCSharp;
using System.Diagnostics;
using System.IO.Compression;
using System.IO;

namespace My.Shared.Tasks
{
    public class MyTaskBase
    {
        protected MyManagerCSharp.Log.LogManager _log;
        protected string _taskName;
        protected Guid _uid;

        protected string _connectionName;

        protected bool _DebugMoveErrorFileInFolder;

        protected bool TEST_MODE = false;

        protected System.IO.DirectoryInfo folderAttachments;
        protected System.IO.DirectoryInfo folderArchive;

        protected long contaRecordLetti = 0;
        protected long contaRecordInseriti = 0;
        protected long contaRecordInErrore = 0;
        protected long contaRecordAggiornati = 0;
        protected long contaRecordGiaPresenti = 0;

        protected long contaAllegatiScaricati = 0;
        protected long contaAllegatiLetti = 0;
        protected long contaAllegatiInseriti = 0;
        protected long contaAllegatiInErrore = 0;

        //chiavi per cifrare e decifrare le credenziali di accesso al proxy, persenti nel file di configurazione
        private string _key;
        private string _IV;

        private System.IO.FileInfo _fileDownloaded;

        public MyTaskBase(string connectionName, string taskName)
        {
            _log = new MyManagerCSharp.Log.LogManager(connectionName);
            _taskName = taskName;
            _uid = Guid.NewGuid();

            _connectionName = connectionName;

            _DebugMoveErrorFileInFolder = true;
        }

        protected bool init()
        {

            string messaggio;

#if DEBUG
            messaggio = String.Format("Start task {0} ver. {1}  [DEBUG]", _taskName, this.GetType().Assembly.GetName().Version.ToString());
#endif

#if !DEBUG
            messaggio = String.Format("Start task {0} ver. {1}", _taskName, this.GetType().Assembly.GetName().Version.ToString());
#endif

            _printMessage(messaggio);

            try
            {
                _log.openConnection();
            }
            catch (Exception e)
            {
                Console.WriteLine("Errore di connessione verso il database: " + e.Message);
                throw new MyException(MyException.ErrorNumber.Errore_connessione_verso_il_database, e.Message);
            }

            _log.info(messaggio, _uid.ToString(), "", _taskName);


            if (System.Configuration.ConfigurationManager.AppSettings["task.mode.test"] != null && bool.Parse(System.Configuration.ConfigurationManager.AppSettings["task.mode.test"]) == true)
            {
                TEST_MODE = true;
                messaggio = "task.mode.test == TRUE";

                _printMessage(messaggio);
                _log.warning(messaggio, _uid.ToString(), "", _taskName);
            }


            if (System.Configuration.ConfigurationManager.AppSettings["task.folder.attachments"] == null || String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["task.folder.attachments"]))
            {
                folderAttachments = null;
            }
            else
            {
                folderAttachments = new System.IO.DirectoryInfo(System.Configuration.ConfigurationManager.AppSettings["task.folder.attachments"]);
                if (folderAttachments.Exists == false)
                {
                    messaggio = String.Format("[task.folder.attachments] not exists: {0}", folderAttachments.FullName);
                    _log.error(messaggio, _uid.ToString(), "", _taskName);
                    Console.WriteLine(messaggio);
                    return false;
                }

            }


            if (System.Configuration.ConfigurationManager.AppSettings["task.folder.archive"] == null || String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["task.folder.archive"]))
            {
                folderArchive = null;
            }
            else
            {
                folderArchive = new System.IO.DirectoryInfo(System.Configuration.ConfigurationManager.AppSettings["task.folder.archive"]);
                if (folderArchive.Exists == false)
                {
                    messaggio = String.Format("[task.folder.archive] not exists: {0}", folderArchive.FullName);
                    _log.error(messaggio, _uid.ToString(), "", _taskName);
                    Console.WriteLine(messaggio);
                    return false;
                }

                if (!System.IO.Directory.Exists(folderArchive.FullName + "Error\\"))
                {
                    System.IO.Directory.CreateDirectory(folderArchive.FullName + "Error\\");
                }
            }




            if (System.Configuration.ConfigurationManager.AppSettings["debug.delay.milliseconds"] != null && !String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["debug.delay.milliseconds"]))
            {
                int delay = int.Parse(System.Configuration.ConfigurationManager.AppSettings["debug.delay.milliseconds"]);

                if (delay > 0)
                {
                    messaggio = String.Format("[debug.delay.milliseconds]: {0}", delay);
                    _log.error(messaggio, _uid.ToString(), "", _taskName);
                    Console.WriteLine(messaggio);

                    System.Threading.Thread.Sleep(delay);
                }

            }



            return true;
        }

        private string decript(string cipherTextBase64)
        {
            if (String.IsNullOrEmpty(_key))
                throw new ArgumentNullException("Decript key is NULL, usare il metodo setKey");

            if (String.IsNullOrEmpty(_IV))
                throw new ArgumentNullException("Decript IV is NULL, usare il metodo setKey");

            return MyManagerCSharp.SecurityManager.AESDecryptSFromBase64String(cipherTextBase64, System.Text.UTF8Encoding.UTF8.GetBytes(_key), System.Text.UTF8Encoding.UTF8.GetBytes(_IV));
        }



        private string downloadXML(Uri sourceFile, System.IO.FileInfo destinationFile)
        {
            string extraMessage = "";

            System.Net.WebRequest myRequest;
            myRequest = System.Net.WebRequest.Create(sourceFile);

            if (System.Configuration.ConfigurationManager.AppSettings["proxy.isEnabled"] != null && bool.Parse(System.Configuration.ConfigurationManager.AppSettings["proxy.isEnabled"]))
            {
                extraMessage += " - Proxy abilitato";
                _printMessage("Proxy abilitato");

                System.Net.NetworkCredential credential = null;

                if (bool.Parse(System.Configuration.ConfigurationManager.AppSettings["proxy.credentials.encrypted"]))
                {

                    extraMessage += " - Le credenziali sono cifrate";
                    _printMessage("Le credenziali del proxy sono cifrate");

                    string username = decript(System.Configuration.ConfigurationManager.AppSettings["proxy.username"]);
                    string password = decript(System.Configuration.ConfigurationManager.AppSettings["proxy.password"]);

                    credential = new System.Net.NetworkCredential(username, password);
                }
                else
                {
                    extraMessage += " - Le credenziali sono in chiaro";
                    credential = new System.Net.NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["proxy.username"], System.Configuration.ConfigurationManager.AppSettings["proxy.password"]);
                }

                myRequest.Credentials = credential;

                System.Net.WebProxy webProxy = new System.Net.WebProxy(System.Configuration.ConfigurationManager.AppSettings["proxy.url"], bool.Parse(System.Configuration.ConfigurationManager.AppSettings["proxy.bypassOnLocal"]));
                webProxy.UseDefaultCredentials = false;
                webProxy.Credentials = credential;

                myRequest.Proxy = webProxy;
            }


            myRequest.Timeout = 30000;

            System.Net.WebResponse myResponse = null;
            System.IO.StreamReader reader = null;
            System.IO.FileStream destStream = null;
            try
            {
                _printMessage("Downloading ...");
                myResponse = myRequest.GetResponse();

                if (destinationFile.Exists)
                {
                    destinationFile.Delete();
                }

                destStream = destinationFile.Create();

                reader = new System.IO.StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
                reader.BaseStream.CopyTo(destStream);

                //html = reader.ReadToEnd();
            }
            catch (System.Net.WebException ex)
            {
                return ex.Message + extraMessage;
            }
            finally
            {
                if (myResponse != null)
                {
                    myResponse.Close();
                }
                if (reader != null)
                {
                    reader.Close();
                }

                if (destStream != null)
                {
                    destStream.Close();
                }
            }
            return "";
        }

        private bool Download()
        {
            string messaggio;

            if (!bool.Parse(System.Configuration.ConfigurationManager.AppSettings["download.isEnabled"]))
            {

                _printMessage("[App.config] download.isEnabled = false");
                _log.warning("[App.config] download.isEnabled = false", _uid.ToString(), "", _taskName);
                _fileDownloaded = null;
                return true;
            }

            if (System.Configuration.ConfigurationManager.AppSettings["download.url"] == null)
            {
                _printMessage("[App.config] download.url is NULL");
                _log.warning("[App.config] download.url is NULL", _uid.ToString(), "", _taskName);
                _fileDownloaded = null;
                return false;
            }

            try
            {
                Uri sourceFile = new Uri(System.Configuration.ConfigurationManager.AppSettings["download.url"]);

                messaggio = String.Format("Inizio download file XML: {0}", sourceFile.AbsoluteUri);
                _log.info(messaggio, _uid.ToString(), "", _taskName);
                Console.WriteLine(messaggio);

                string esito;

                Debug.WriteLine("BaseDirectory: " + AppDomain.CurrentDomain.BaseDirectory);


                System.IO.FileInfo destinationFile = new System.IO.FileInfo(folderAttachments + sourceFile.Segments[sourceFile.Segments.Length - 1]);
                esito = downloadXML(sourceFile, destinationFile);

                if (!String.IsNullOrEmpty(esito))
                {

                    _log.error("Download XML: " + esito, _uid.ToString(), "", _taskName);
                    Console.WriteLine("Download XML fallito: " + esito);

                    if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["mail.support"]))
                    {
                        // DataModel.EarlyWarningEmailMessage mail = new DataModel.EarlyWarningEmailMessage(System.Configuration.ConfigurationManager.AppSettings["application.name"], System.Configuration.ConfigurationManager.AppSettings["application.url"]);

                        MyManagerCSharp.MailMessageManager mail = new MailMessageManager(System.Configuration.ConfigurationManager.AppSettings["application.name"], System.Configuration.ConfigurationManager.AppSettings["application.url"]);
                        mail._Subject = System.Net.Dns.GetHostName() + " - " + _taskName + " - Errore durante il download";

                        mail._Body = String.Format("Errore durante il download file XML {0} " + Environment.NewLine + "{1}", sourceFile, esito);
                        try
                        {
                            mail._To(System.Configuration.ConfigurationManager.AppSettings["mail.support"]);
                            mail.send();
                        }
                        catch (Exception ex)
                        {
                            MyManagerCSharp.MailManager.send(ex);
                        }
                    }

                    _fileDownloaded = null;
                    return false;
                }

                messaggio = "Download XML completato con successo";
                _log.info(messaggio, _uid.ToString(), "", _taskName);
                Console.WriteLine(messaggio);

                //Rutigliano 19/11/2014   UNZIP
                if (destinationFile.Extension == ".zip")
                {
                    // System.IO.Compression.ZipFile.ExtractToDirectory(destinationFile.FullName, folderAttachments.FullName);



                    using (System.IO.Compression.ZipArchive archive = System.IO.Compression.ZipFile.OpenRead(destinationFile.FullName))
                    {
                        foreach (System.IO.Compression.ZipArchiveEntry entry in archive.Entries)
                        {
                            entry.ExtractToFile(System.IO.Path.Combine(folderAttachments.FullName, entry.Name), true);
                        }
                    }

                    messaggio = "UNZIP completato con successo";
                    _log.info(messaggio, _uid.ToString(), "", _taskName);
                    Console.WriteLine(messaggio);
                }

                _fileDownloaded = destinationFile;
                return true;
            }
            catch (Exception ex)
            {
                _log.error(ex.Message, _uid.ToString(), "", _taskName);
                Console.WriteLine("Exception: " + ex.Message);
                Debug.WriteLine("Exception: " + ex.Message);
                return false;
            }

        }

        private bool Pop3()
        {
            string messaggio;
            if (!bool.Parse(System.Configuration.ConfigurationManager.AppSettings["pop3.isEnabled"]))
            {
                messaggio = String.Format("Pop3 disabilitato. Non è stato fatto il download degli allegati");
                _log.warning(messaggio, _uid.ToString(), "", _taskName);
                Console.WriteLine(messaggio);
                return true;
            }

            ActiveUp.Net.Mail.Pop3Client client = new ActiveUp.Net.Mail.Pop3Client();
            long countMessages;


            try
            {



                string username;
                string password;

                if (bool.Parse(System.Configuration.ConfigurationManager.AppSettings["pop3.credentials.encrypted"]))
                {
                    username = decript(System.Configuration.ConfigurationManager.AppSettings["pop3.server.username"]);
                    password = decript(System.Configuration.ConfigurationManager.AppSettings["pop3.server.password"]);
                    _printMessage("Le credenziali sono cifrate");
                }
                else
                {
                    username = System.Configuration.ConfigurationManager.AppSettings["pop3.server.username"];
                    password = System.Configuration.ConfigurationManager.AppSettings["pop3.server.password"];
                }


                if (bool.Parse(System.Configuration.ConfigurationManager.AppSettings["pop3.server.isSSL"]))
                {
                    _printMessage("Connessione SSL veso il server POP3: " + System.Configuration.ConfigurationManager.AppSettings["pop3.server"]);

                    client.ConnectSsl(System.Configuration.ConfigurationManager.AppSettings["pop3.server"],
                    int.Parse(System.Configuration.ConfigurationManager.AppSettings["pop3.server.port"]),
                    username, password);
                }
                else
                {
                    _printMessage("Connessione veso il server POP3: " + System.Configuration.ConfigurationManager.AppSettings["pop3.server"]);

                    client.Connect(System.Configuration.ConfigurationManager.AppSettings["pop3.server"],
                    int.Parse(System.Configuration.ConfigurationManager.AppSettings["pop3.server.port"]),
                    username, password);
                }

                bool deleteMessage = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["pop3.message.delete"]);
                //int deleteHours = int.Parse(System.Configuration.ConfigurationManager.AppSettings["pop3.message.delete.hours"]);

                countMessages = client.MessageCount;

                messaggio = String.Format("Connessione al server pop3 completato con successo. Trovati {0:N0} messaggi", countMessages);
                _log.info(messaggio, _uid.ToString(), "", _taskName);
                Console.WriteLine(messaggio);

                string temp;
                string[] alertsFrom;

                temp = System.Configuration.ConfigurationManager.AppSettings["pop3.download.email.from"];
                if (temp.EndsWith(";"))
                {
                    temp = temp.Substring(0, temp.Length - 1);
                }

                alertsFrom = temp.Split(';');

                ActiveUp.Net.Mail.Header header;
                ActiveUp.Net.Mail.Message message;



                for (int i = 1; i <= countMessages; i++)
                {
                    Console.WriteLine(String.Format("Messsage Header {0:N0}/{1:N0}", i, countMessages));

                    header = client.RetrieveHeaderObject(i);

                    //"centraleallarmi@posteitaliane.it"

                    //if (header.From.Email != "alerts@deepsight.symantec.com")
                    if (!alertsFrom.Contains(header.From.Email))
                    {
                        continue;
                    }

                    message = client.RetrieveMessageObject(i, deleteMessage);

                    Console.WriteLine("Message from: " + message.From.Email);

                    saveAttachments(message);


                    message = null;
                }

                if (client.IsConnected)
                {
                    client.Disconnect();
                }

                messaggio = String.Format("Sono stati scaricati {0} allegati XML", contaAllegatiScaricati);
                _log.info(messaggio, _uid.ToString(), "", _taskName);
                Console.WriteLine(messaggio);


                return true;
            }
            catch (Exception ex)
            {
                _log.error(ex.Message, _uid.ToString(), "", _taskName);
                Console.WriteLine("Exception: " + ex.Message);
                Debug.WriteLine("Exception: " + ex.Message);
                return false;
            }
            finally
            {
                if (client.IsConnected)
                {
                    try
                    {
                        client.Disconnect();
                    }
                    catch (Exception ex)
                    {
                        //ignoro
                    }

                }
            }
        }

        public bool start()
        {
            try
            {
                if (init() == false)
                {
                    return false;
                }

                string messaggio;
                bool esito = true;
                DateTime startDateTime = DateTime.Now;

                if (esito && System.Configuration.ConfigurationManager.AppSettings["pop3.isEnabled"] != null)
                {
                    esito = Pop3();
                }

                if (esito && System.Configuration.ConfigurationManager.AppSettings["imap.isEnabled"] != null)
                {
                    esito = Imap();
                }

                if (esito && System.Configuration.ConfigurationManager.AppSettings["download.isEnabled"] != null)
                {
                    esito = Download();
                }


                if (esito == false)
                {
                    return false;
                }


                if (System.Configuration.ConfigurationManager.AppSettings["download.isEnabled"] != null
                    && bool.Parse(System.Configuration.ConfigurationManager.AppSettings["download.isEnabled"]) == true
                    && System.Configuration.ConfigurationManager.AppSettings["download.check.MD5"] != null)
                {
                    esito = checkMD5();

                    if (esito == true)
                    {
                        return true;
                    }
                }





                if (folderAttachments != null)
                {
                    processFolder(folderAttachments);
                }
                else
                {
                    process();
                }




                afterProcess();


                /*
                                if (esito)
                                {
                                    messaggio = String.Format("Elaborazione conclusa con successo");
                                    _log.info(messaggio, _uid.ToString(), "", _taskName);
                                    Console.WriteLine(messaggio);
                                }
                                else
                                {
                                    messaggio = String.Format("Elaborazione conclusa con errori");
                                    _log.error(messaggio, _uid.ToString(), "", _taskName);
                                    Console.WriteLine(messaggio);
                                }
                                */

                messaggio = String.Format("Elaborazione completata in " + MyManagerCSharp.ConsoleManager.calcolaTempoDiElaborazione(startDateTime, DateTime.Now));
                _log.info(messaggio, _uid.ToString(), "", _taskName);
                Console.WriteLine(messaggio);

                return true;
            }
            catch (Exception ex)
            {
                _log.error(ex.Message, _uid.ToString(), "", _taskName);
                Console.WriteLine("Exception: " + ex.Message);
                return false;
            }
            finally
            {
                _log.closeConnection();
            }
        }



        private bool checkMD5()
        {
            if (!bool.Parse(System.Configuration.ConfigurationManager.AppSettings["download.check.MD5"]))
            {

                _printMessage("[App.config] download.check.MD5 = false");
                // _log.warning("[App.config] download.isEnabled = false", _uid.ToString(), "", _taskName);
                return false;
            }

            if (_fileDownloaded == null)
            {
                _printMessage("File downloaded == NULL");
                return false;
            }

            Debug.WriteLine("BaseDirectory: " + AppDomain.CurrentDomain.BaseDirectory);
            //Calcolo MD5 del file appena scaricato
            string MD5 = MyManagerCSharp.SecurityManager.getMD5Hash(_fileDownloaded);


            System.IO.FileInfo compareMD5 = new System.IO.FileInfo(AppDomain.CurrentDomain.BaseDirectory + "MD5.txt");
            bool esito = false;

            if (compareMD5.Exists)
            {
                //leggo il contenuto del file 
                string compareValue = System.IO.File.ReadAllText(compareMD5.FullName);

                if (compareValue == MD5)
                {
                    esito = true;

                    string messaggio = String.Format("File con lo stesso MD5: " + MD5);
                    _log.info(messaggio, _uid.ToString(), "", _taskName);
                    _printMessage(messaggio);
                }
                else
                {
                    _printMessage("MD5 diverso");
                }
            }

            //sostituisco il vecchio valore
            System.IO.File.WriteAllText(compareMD5.FullName, MD5);

            return esito;
        }



        private string processFolder(System.IO.DirectoryInfo folderXML)
        {
            string messaggio;
            bool esitoSingoloFileXML;
            bool esitoArchivioZip;




            messaggio = String.Format("Inizio elaborazione della folder: {0}", folderXML.FullName);
            _log.info(messaggio, _uid.ToString(), "", _taskName);
            Console.WriteLine(messaggio);

            contaAllegatiLetti = 0;
            contaAllegatiInseriti = 0;
            contaAllegatiInErrore = 0;

            foreach (System.IO.FileInfo fi in folderXML.GetFiles("*.*", SearchOption.AllDirectories))
            {
                contaAllegatiLetti++;

                messaggio = String.Format("Inizio elaborazione del file: {0}", fi.Name);
                _log.info(messaggio, _uid.ToString(), fi.Name, _taskName);
                Console.WriteLine(messaggio);

                List<FileInfo> filesXML = new List<FileInfo>();
                esitoArchivioZip = true;

                switch (fi.Extension)
                {
                    case ".zip":
                        FileInfo fileZip = fi;
                        using (System.IO.Compression.ZipArchive archive = System.IO.Compression.ZipFile.OpenRead(fileZip.FullName))
                        {
                            foreach (System.IO.Compression.ZipArchiveEntry entry in archive.Entries)
                            {
                                entry.ExtractToFile(System.IO.Path.Combine(fileZip.Directory.FullName, entry.Name), true);

                                filesXML.Add(new FileInfo(System.IO.Path.Combine(fileZip.Directory.FullName, entry.Name)));
                            }
                        }

                        messaggio = "UNZIP completato con successo: " + fileZip.Name;
                        _log.info(messaggio, _uid.ToString(), fi.Name, _taskName);
                        Console.WriteLine(messaggio);


                        break;
                    case ".xml":
                        filesXML.Add(fi);
                        break;
                    case "":
                        //nel caso di CVS ho dei files senza estenzione ... non li sposto!!!
                        contaAllegatiLetti--;
                        continue;
                    default:
                        //throw new ApplicationException("Task Base: tipo di file non supportato: " + fi.Extension);
                        messaggio = "Task Base: tipo di file non supportato: " + fi.FullName;
                        _log.error(messaggio, _uid.ToString(), fi.Name, _taskName);
                        Console.WriteLine(messaggio);

                        if (System.IO.File.Exists(folderArchive.FullName + fi.Name))
                        {
                            System.IO.File.Delete(folderArchive.FullName + fi.Name);
                        }
                        contaAllegatiInErrore++;
                        fi.MoveTo(folderArchive.FullName + "Error\\" + fi.Name);
                        continue;
                        
                }



                foreach (FileInfo file in filesXML)
                {
                    contaRecordLetti = 0;
                    contaRecordInseriti = 0;
                    contaRecordInErrore = 0;
                    contaRecordGiaPresenti = 0;

                    esitoSingoloFileXML = processXML(file);

                    if (esitoSingoloFileXML)
                    {
                        if (System.IO.File.Exists(folderArchive.FullName + file.Name))
                        {
                            System.IO.File.Delete(folderArchive.FullName + file.Name);
                        }

                        file.MoveTo(folderArchive.FullName + file.Name);

                        messaggio = String.Format("Elaborazione e archiviazione del file conclusa con successo: {0}", file.Name);
                        _log.info(messaggio, _uid.ToString(), file.Name, _taskName);
                        Console.WriteLine(messaggio);

                        contaAllegatiInseriti++;
                    }
                    else
                    {
                        esitoArchivioZip = false;

                        if (_DebugMoveErrorFileInFolder)
                        {
                            if (System.IO.File.Exists(folderArchive.FullName + "Error\\" + file.Name))
                            {
                                System.IO.File.Delete(folderArchive.FullName + "Error\\" + file.Name);
                            }

                            file.MoveTo(folderArchive.FullName + "Error\\" + file.Name);
                        }
                        messaggio = String.Format("Elaborazione del file conclusa con errori: {0}", file.Name);
                        _log.error(messaggio, _uid.ToString(), file.Name, _taskName);
                        Console.WriteLine(messaggio);

                        contaAllegatiInErrore++;
                    }
                }

                if (fi.Extension == ".zip")
                {
                    if (esitoArchivioZip == false)
                    {
                        if (System.IO.File.Exists(folderArchive.FullName + "Error\\" + fi.Name))
                        {
                            System.IO.File.Delete(folderArchive.FullName + "Error\\" + fi.Name);
                        }

                        fi.MoveTo(folderArchive.FullName + "Error\\" + fi.Name);

                        //messaggio = String.Format("Elaborazione del file conclusa con errori: {0}", fi.Name);
                        //_log.error(messaggio, _uid.ToString(), fi.Name, _taskName);
                        //Console.WriteLine(messaggio);
                    }
                    else
                    {
                        fi.MoveTo(folderArchive.FullName + fi.Name);

                    }
                }
            }


            if (contaAllegatiInErrore == 0)
            {
                messaggio = String.Format("Elaborazione della folder conclusa con successo. Letti {0} allegati e inseriti {1} ", contaAllegatiLetti, contaAllegatiInseriti);
                _log.info(messaggio, _uid.ToString(), "", _taskName);
                Console.WriteLine(messaggio);
            }
            else
            {
                messaggio = String.Format("Elaborazione della folder conclusa con ERRORI. Letti {0} allegati e inseriti {1} allegati errori {2}", contaAllegatiLetti, contaAllegatiInseriti, contaAllegatiInErrore);
                _log.error(messaggio, _uid.ToString(), "", _taskName);
                Console.WriteLine(messaggio);
            }

            return "";
        }

        protected virtual bool processXML(System.IO.FileInfo fi)
        {
            //return "Not implemented";
            return false;
        }

        protected virtual bool process()
        {
            //return "Not implemented";
            return false;
        }


        protected virtual bool afterProcess()
        {
            return true;
        }



        protected void printSummary()
        {
            string messaggio;

            if (contaRecordInErrore > 0)
            {
                messaggio = String.Format("Elaborazione conclusa con ERRORI. Letti {0:N0} record, inseriti {1:N0}, già presenti {2:N0}, record con errori {3:N0}", contaRecordLetti, contaRecordInseriti, contaRecordGiaPresenti, contaRecordInErrore);
                messaggio += String.Format(", aggiornati {0:N0}", contaRecordAggiornati);

                //Rutigliano 05/06/2015 lascio le info 
                //  _log.error(messaggio, _uid.ToString(), "", _taskName);
                _log.warning(messaggio, _uid.ToString(), "", _taskName);

                Console.WriteLine(messaggio);
                //messaggio = String.Format("Letti {0:N0} record, inseriti {1:N0}, già presenti {2:N0}, record con errori {3:N0}", contaRecordLetti, contaRecordInseriti, contaRecordGiaPresenti, contaRecordInErrore);
            }
            else
            {
                messaggio = String.Format("Elaborazione conclusa con successo. Letti {0:N0} record, inseriti {1:N0}, già presenti {2:N0} ", contaRecordLetti, contaRecordInseriti, contaRecordGiaPresenti);
                messaggio += String.Format(", aggiornati {0:N0}", contaRecordAggiornati);

                _log.info(messaggio, _uid.ToString(), "", _taskName);
                Console.WriteLine(messaggio);
                //messaggio = String.Format("Elaborazione conclusa con successo. Letti {0:N0} record, inseriti {1:N0}, già presenti {2:N0} ", contaRecordLetti, contaRecordInseriti, contaRecordGiaPresenti);
            }
        }

        public void setKey(string key, string IV)
        {
            _key = key;
            _IV = IV;
        }

        private void _printMessage(string message)
        {
            Debug.WriteLine(message);
            Console.WriteLine(message);
        }

        private bool Imap()
        {

            string messaggio;
            if (!bool.Parse(System.Configuration.ConfigurationManager.AppSettings["imap.isEnabled"]))
            {
                messaggio = String.Format("IMAP disabilitato. Non è stato fatto il download degli allegati");
                _log.warning(messaggio, _uid.ToString(), "", _taskName);
                Console.WriteLine(messaggio);
                return true;
            }


            ActiveUp.Net.Mail.Imap4Client client = new ActiveUp.Net.Mail.Imap4Client();
            long countMessages;

            try
            {
                string username;
                string password;

                if (bool.Parse(System.Configuration.ConfigurationManager.AppSettings["imap.credentials.encrypted"]))
                {
                    username = decript(System.Configuration.ConfigurationManager.AppSettings["imap.server.username"]);
                    password = decript(System.Configuration.ConfigurationManager.AppSettings["imap.server.password"]);
                    _printMessage("Le credenziali sono cifrate");
                }
                else
                {
                    username = System.Configuration.ConfigurationManager.AppSettings["imap.server.username"];
                    password = System.Configuration.ConfigurationManager.AppSettings["imap.server.password"];
                }

                _printMessage("Connessione veso il server IMAP: " + System.Configuration.ConfigurationManager.AppSettings["imap.server"]);

                client.Connect(System.Configuration.ConfigurationManager.AppSettings["imap.server"], int.Parse(System.Configuration.ConfigurationManager.AppSettings["imap.server.port"]), username, password);

                ActiveUp.Net.Mail.Mailbox inbox = client.SelectMailbox("inbox");

                int[] ids = inbox.Search("ALL");
                countMessages = ids.Length;

                bool deleteMessage = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["imap.message.delete"]);
                int deleteHours = int.Parse(System.Configuration.ConfigurationManager.AppSettings["imap.message.delete.hours"]);


                messaggio = String.Format("Connessione al server IMAP completato con successo. Trovati {0:N0} messaggi", countMessages);
                _log.info(messaggio, _uid.ToString(), "", _taskName);
                Console.WriteLine(messaggio);

                string temp;
                string[] alertsFrom;

                temp = System.Configuration.ConfigurationManager.AppSettings["imap.download.email.from"];
                if (temp.EndsWith(";"))
                {
                    temp = temp.Substring(0, temp.Length - 1);
                }

                alertsFrom = temp.Split(';');

                ActiveUp.Net.Mail.Message message = null;

                int i = 1;
                foreach (int id in ids)
                {
                    Console.WriteLine(String.Format("Messsage {0:N0}/{1:N0}", i, countMessages));
                    message = inbox.Fetch.MessageObject(id);

                    //"centraleallarmi@posteitaliane.it"
                    //if (header.From.Email != "alerts@deepsight.symantec.com")
                    if (!alertsFrom.Contains(message.From.Email))
                    {
                        Console.WriteLine("Skip message from: " + message.From.Email);
                        continue;
                    }

                    saveAttachments(message);

                    if (deleteMessage)
                    {
                        TimeSpan ts = DateTime.Now.Subtract(message.Date);
                        //_printMessage("TotalHours: " + ts.TotalHours + " deleteHours: " + deleteHours);

                        if (deleteHours == 0 || ts.TotalHours > deleteHours)
                        {
                            inbox.DeleteMessage(id, true);
                            _printMessage(String.Format("Message deleted, {0:N0} hours old", ts.TotalHours));
                        }
                    }


                    message = null;
                    i++;
                }


                if (client.IsConnected)
                {
                    client.Disconnect();
                }

                messaggio = String.Format("Sono stati scaricati {0} allegati XML", contaAllegatiScaricati);
                _log.info(messaggio, _uid.ToString(), "", _taskName);
                Console.WriteLine(messaggio);

                return true;

            }
            catch (Exception ex)
            {
                _log.error(ex.Message, _uid.ToString(), "", _taskName);
                Console.WriteLine("Exception: " + ex.Message);
                Debug.WriteLine("Exception: " + ex.Message);
                return false;
            }
            finally
            {
                if (client.IsConnected)
                {
                    try
                    {
                        client.Disconnect();
                    }
                    catch (Exception ex)
                    {
                        //ignoro
                    }

                }
            }
        }

        private void saveAttachments(ActiveUp.Net.Mail.Message message)
        {
            string messaggio;

            if (message.Attachments.Count == 0)
            {
                messaggio = String.Format("Messaggio senza allegati");
                _log.warning(messaggio, _uid.ToString(), "", _taskName);
                Console.WriteLine(messaggio);
                return;
            }

            string attachFileName;
            foreach (ActiveUp.Net.Mail.MimePart attach in message.Attachments)
            {
                Console.WriteLine("attach: " + attach.Filename);

                contaAllegatiScaricati++;

                attachFileName = folderAttachments.FullName + attach.Filename;

                //Rel. 1.0.1.10
                attach.StoreToFile(attachFileName);

            }
        }

    }
}

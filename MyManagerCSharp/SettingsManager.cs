using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyManagerCSharp
{
    class SettingsManager
    {

        // http://msdn.microsoft.com/en-us/library/system.environment.specialfolder.aspx

        // LocalApplicationData	The directory that serves as a common repository for application-specific data that is used by the current, non-roaming user.
        // ES:  C:\Users\Roberto\AppData\Local
        //Console.WriteLine (System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)) ;

        // ApplicationData	The directory that serves as a common repository for application-specific data for the current roaming user.
        //A roaming user works on more than one computer on a network. A roaming user's profile is kept on a server on the network and is loaded onto a system when the user logs on.
        //ES: C:\Users\Roberto\AppData\Roaming
        //Console.WriteLine(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));

        //private string _pathFileConfigXml = Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%") + "\\AppData\\Roaming\\ConnectDataSpace\\settings.xml";

        private string _pathFileConfigXml = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\ConnectDataSpace\\settings.xml";


        private System.Xml.XmlDocument _document;

        public enum DefaultDriveParameter
        {
            mode,
            delay_time_sync,
            encoding,
            compression,
            max_number_versions,
            lock_expiration_time,
            enforce_max_number_versions
        }

        public enum ApplicationParameter
        {
            cache_size,
            restub_days,
            upload_processes,
            download_processes,
            upload_bandwidth,
            download_bandwidth
        }



       
         //Classe per la gestione del file di configurazione settings.xml
        public SettingsManager()
        {
            _document = new System.Xml.XmlDocument();
            _document.Load(_pathFileConfigXml);
        }

        public bool _reloadXML()
        {
            _document = null;
            _document = new System.Xml.XmlDocument();
            _document.Load(_pathFileConfigXml);
            return true;
        }

        public bool _saveXML()
        {
            _document.Save(_pathFileConfigXml);
            return true;
        }




        public string getApplicationParameter(ApplicationParameter parameter)
        {
            System.Xml.XmlNode n;
            n = _document.SelectSingleNode("//root/settings/" + parameter.ToString().Replace("_", "-"));
            if (n == null)
            {
                return "";
            }
            return n.InnerText;
        }


        public bool setApplicationParameter(ApplicationParameter parameter, string valore)
        {
            System.Xml.XmlNode n;
            n = _document.SelectSingleNode("//root/settings/" + parameter.ToString().Replace("_", "-"));
            if (n == null)
            {
                return false;
            }

            n.InnerText = valore;

            return true;
        }



        public bool _checkVersionNumbert(string versione)
        {
            System.Xml.XmlNode n;
            n = _document.SelectSingleNode("//root[@version ='" + versione + "']");
            if (n == null)
            {
                return false;
            }
            return true;
        }



    }
}

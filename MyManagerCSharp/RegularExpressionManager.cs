using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace MyManagerCSharp
{
    public class RegularExpressionManager
    {
        //Appllicazione di test on line: http://gskinner.com/RegExr/
        //http://www.regular-expressions.info
        //http://regexlib.com/Search.aspx?k=URL



        public static bool isValidEmail(string value)
        {
            //his expression matches email addresses, and checks that they are of the proper form. It checks to ensure the top level domain is between 2 and 4 characters long, but does not check the specific domain against a list (especially since there are so many of them now).
            //Matches	        joe@aol.com | joe@wrox.co.uk | joe@domain.info
            //Non-Matches	    a@b | notanemail | joe@@.

            //http://regexlib.com/REDetails.aspx?regexp_id=21

            System.Text.RegularExpressions.Regex pattern = new System.Text.RegularExpressions.Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            return pattern.IsMatch(value);
        }

        public static bool isMobileDevice(string UserAgent)
        {
            //https://gist.github.com/dalethedeveloper/1503252
            // Mozilla/5.0 (Linux; Android 4.4.4; Nexus 10 Build/KTU84P) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/37.0.2062.117 Safari/537.36
            Debug.WriteLine("UserAgent: " + UserAgent);

            System.Text.RegularExpressions.Regex pattern = new System.Text.RegularExpressions.Regex("Mobile|iP(hone|od|ad)|Android|BlackBerry|IEMobile|Kindle|NetFront|Silk-Accelerated|(hpw|web)OS|Fennec|Minimo|Opera M(obi|ini)|Blazer|Dolfin|Dolphin|Skyfire|Zune");
            return pattern.IsMatch(UserAgent);
        }



        public static bool isOldBrowser(string UserAgent)
        {
            //MSIE 9.0	Internet Explorer 9
            //MSIE 8.0	Internet Explorer 8 or IE8 Compatibility View/Browser Mode
            //MSIE 7.0	Windows Internet Explorer 7 or IE7 Compatibility View/Browser Mode
            //MSIE 6.0	Microsoft Internet Explorer 6

            if (UserAgent.IndexOf("MSIE 8.0") != -1 || UserAgent.IndexOf("MSIE 7.0") != -1 || UserAgent.IndexOf("MSIE 6.0") != -1)
            {
                return true;
            }

            return false;
        }




        public static bool isStrongPassword(string value)
        {
            //http://msdn.microsoft.com/library/ITA/jscript7/html/jsreconIntroductionToRegularExpressions.asp

            //At least 8 charcaters long
            //Includes at least one numeric digit
            //Includes at least one lowercase alpha character
            //Includes at least one uppercase alpha character
            //Includes at least one special character of PASSWORD_CHARS_SPECIAL

            string passwordStrengthRegularExpression;


            //'^           # anchor at the start
            //'               (?=.*\d)     # must contain at least one numeric character
            //'              (?=.*[a-z])  # must contain one lowercase character
            //'             (?=.*[A-Z])  # must contain one uppercase character
            //'            .{8,10}      # From 8 to 10 characters in length
            //'           \s           # allows a space 
            //'          $            # anchor at the end", 
            //'           (?!.*\s)   #non sono ammessi spazi
            //'        (?!.*\W)       #escludo tutti i caratteri non alfanumerico. Equivale a "[^A-Za-z0-9_]". 



            //'passwordStrengthRegularExpression = "^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[ ! "" @\$%&/\(\)=\?'`\*\+~#\-_\.,;:\{\[\]\}\\< >\|]).{8,}$"
            //'passwordStrengthRegularExpression = "^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[" & PASSWORD_CHARS_SPECIAL & "]).{8,}$"


            //'ci sono dei caratteri spciali usati nelle espressioni regolari che devono essere sostitui

            PasswordManager manager = new PasswordManager();

            passwordStrengthRegularExpression = "(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[" + replaceCharactersInRegularExplression(PasswordManager.PASSWORD_CHARS_SPECIAL) + "])" +
                     "(?!.*\\s)(?!.*[" + replaceCharactersInRegularExplression(PasswordManager.PASSWORD_CHARS_SPECIAL_DENY) + "]).{8,}$";


            passwordStrengthRegularExpression = "(.{8,})"; //'almeno 8 caratteri
            passwordStrengthRegularExpression = "(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\\s)(?=.*[" + replaceCharactersInRegularExplression(PasswordManager.PASSWORD_CHARS_SPECIAL) + "])(?!.*[" + replaceCharactersInRegularExplression(PasswordManager.PASSWORD_CHARS_SPECIAL_DENY) + "])(.{8,})";



            return System.Text.RegularExpressions.Regex.IsMatch(value, passwordStrengthRegularExpression);
        }

        public static string replaceCharactersInRegularExplression(string value)
        {
            string temp;

            //i caratteri spciali per le espressioni regolari sono: http://msdn.microsoft.com/library/ita/default.asp?url=/library/ITA/jscript7/html/jsjsgrpregexpsyntax.asp
            //prima di tutto sostiuisco l'evenuale \
            // temp = value.Replace("\\", "\\\\");
            temp = value.Replace("$", "\\$").Replace("(", "\\(").Replace(")", "\\)").Replace("*", "\\*").Replace("+", "\\+").Replace("-", "\\-").Replace("[", "\\[").Replace("]", "\\]").Replace("?", "\\?").Replace("/", "\\/").Replace("^", "\\^").Replace("{", "\\{").Replace("}", "\\}").Replace("|", "\\|");

            return temp;
        }



        public static bool isValidIpAddress(string value)
        {
            //http://channel9.msdn.com/wiki/default.aspx/SecurityWiki.RegExInputValCode2

            //IP address  
            //Matches 0.0.0.0 through 255.255.255.255  
            //Use this regex to match IP numbers with accurracy, without access to the individual IP numbers.  

            System.Text.RegularExpressions.Regex pattern = new System.Text.RegularExpressions.Regex("^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$");
            return pattern.IsMatch(value);
        }




        public static string isPartitaIva(string value)
        {
            string esito = "";
            value = value.ToUpper().Trim();

            if (value.Length != 11)
            {
                esito = String.Format("La partita iva inserita è lunga {0:NO} caratteri invece di 11", value.Length);
                return esito;
            }
            System.Text.RegularExpressions.Regex pattern = new System.Text.RegularExpressions.Regex(@"[0-9]");

            if (!pattern.IsMatch(value))
            {
                esito = "La partita iva digitata non è sintatticamente corretta";
                return esito;
            }
            
            return "";
        }

        public static string isValidCodiceFiscale(string value)
        {
            //'Return System.Text.RegularExpressions.Regex.IsMatch(value, "[A-Za-z]{6}[0-9]{2}[A-Za-z]{1}[0-9]{2}[A-Za-z]{1}[0-9]{3}[A-Za-z]{1}$")

            value = value.ToUpper().Trim();
            string esito = "";

            if (value.Length != 16)
            {
                esito = String.Format("Il codice fiscale inserito è lungo {0:NO} caratteri invece di 16", value.Length);
                return esito;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(value, "[A-Z][A-Z][A-Z][A-Z][A-Z][A-Z][0-9][0-9][A-Z][0-9][0-9][A-Z][0-9][0-9][0-9][A-Z]"))
            {
                esito = "Il codice fiscale digitato non è sintatticamente corretto";
                return esito;
            }

            string mese = value.ToUpper().Substring(8, 1);
            if (mese != "A" &&
              mese != "B" &&
               mese != "C" &&
               mese != "D" &&
               mese != "E" &&
               mese != "F" &&
               mese != "G" &&
               mese != "H" &&
               mese != "L" &&
               mese != "M" &&
               mese != "P" &&
               mese != "R" &&
               mese != "S" &&
               mese != "T")
            {
                esito = "Inserire un codice fiscale corretto,  il carattere in posizione 9 deve essere compreso da A-T";
                return esito;
            }

            int giorno = int.Parse(value.Substring(9, 2));

            if ((giorno > 31 && giorno < 41) || (giorno <= 0) || (giorno > 71))
            {
                esito = "Inserire un codice fiscale corretto il carattere in posizione 10 e 11 deve essere compreso tra 1 e 71";
                return esito;
            }

            return "";
        }
    }
}

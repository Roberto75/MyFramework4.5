using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyManagerCSharp
{
    public class MyException : Exception
    {
        public enum ErrorNumber
        {
            NotDefined = 0,
            PaginaNonTrovata = 1,
            UtenteNonAutorizzato = 2,
            UtenteNonAutenticato = 3,
            UtenteDisabilitato = 4,
            LoginDuplicata = 5,
            LoginInesistente = 6,
            LoginPasswordErrati = 7,
            FunzioneNonImplementata = 8,
            Record_duplicato = 9,
            Email_duplicata = 10,
            UtenteLoggato = 12,
            Passwor_expired = 13,
            ParametroNull,
            Parametro_non_valido,
            Codice_id_non_valido,
            Errore_connessione_verso_il_database
            //Constraint_violation 
        }

        public ErrorNumber ErrorCode;


        public MyException() : base()
        {

        }

        public MyException(string message)
            : base(message)
        {
            ErrorCode = ErrorNumber.NotDefined;
        }

        public MyException(string message, Exception ex)
            : base(message, ex)
        {
            ErrorCode = ErrorNumber.NotDefined;
        }


        public MyException(ErrorNumber errorNumber)
            : base("")
        {
            ErrorCode = errorNumber;
        }


        public MyException(ErrorNumber errorNumber, string message)
            : base(message)
        {
            ErrorCode = errorNumber;
        }


        public override string Message
        {
            get
            {
                string messaggio;

                switch (ErrorCode)
                {
                    case ErrorNumber.NotDefined:
                        messaggio = "";
                        break;
                    case ErrorNumber.LoginPasswordErrati:
                        messaggio = "Login utente e/o password errati. Verificare e ripetere l'operazione.";
                        break;
                    case ErrorNumber.UtenteDisabilitato:
                        messaggio = "Utente disabilitato. Inviare una mail all'amministatore del sistema.";
                        break;
                    case ErrorNumber.ParametroNull:
                        messaggio = "Parametro non valorizzato = NULL";
                        break;
                    case ErrorNumber.Parametro_non_valido:
                        messaggio = "Parametro non valido";
                        break;
                    case ErrorNumber.UtenteNonAutorizzato:
                        messaggio = "Utente non autorizzato";
                        break;
                    //case ErrorNumber.Constraint_violation:
                    //    messaggio = "Il database ha individuato una violazione del vincolo di integrità referenziale";
                    //    break;
                    default:
                        throw new ApplicationException("Eccezione non gestita: " + ErrorCode.ToString());
                }


                if (!String.IsNullOrEmpty(base.Message))
                {
                    if (String.IsNullOrEmpty(messaggio))
                    {
                        messaggio = base.Message;
                    }
                    else
                    {
                        messaggio += Environment.NewLine + base.Message;
                    }
                }


                if (base.InnerException != null)
                {
                    messaggio += Environment.NewLine + base.InnerException.Message;
                }

                return messaggio;
            }
        }


    }
}

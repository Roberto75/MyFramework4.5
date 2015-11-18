using My.MessageQueue.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.MessageQueue
{
    public class MailManager : MyManagerCSharp.ManagerDB
    {
        protected LogManager _log;

        public MailManager(string connectionName)
            : base(connectionName)
        {
            _log = new LogManager(_connection);
        }

        public MailManager(System.Data.Common.DbConnection connection)
            : base(connection)
        {
            _log = new LogManager(_connection);
        }

        public bool send(long emailId)
        {
            Email email = getEmail(emailId);

            if (email == null)
            {
                return false;
            }

            return send(email);
        }

        public bool send(Email model)
        {

            _log.info(model.id, "Inizio trasmissione");

            MyManagerCSharp.MailManager mail = new MyManagerCSharp.MailManager();
            // DataModel.EarlyWarningEmailMessage mail = new DataModel.EarlyWarningEmailMessage(System.Configuration.ConfigurationManager.AppSettings["application.name"], System.Configuration.ConfigurationManager.AppSettings["application.url"]);

            mail._Subject = model.Subject;
            mail._Body = model.Body;

            if (!String.IsNullOrEmpty(model.To))
            {
                mail._To(model.To);
                _log.info(model.id, String.Format("To: {0}", model.To));
            }

            if (!String.IsNullOrEmpty(model.Cc))
            {
                mail._Cc(model.Cc);
                _log.info(model.id, String.Format("Cc: {0}", model.Cc));
            }

            if (!String.IsNullOrEmpty(model.Bcc))
            {
                mail._Bcc(model.Bcc);
                _log.info(model.id, String.Format("Bcc: {0}", model.Bcc));
            }

            //Attachment 
            if (model.Attachments.Count > 0)
            {
                mail._Attachments = new List<System.Net.Mail.Attachment>();

                System.Net.Mail.Attachment att;
                foreach (Attachment attachment in model.Attachments)
                {
                    att = new System.Net.Mail.Attachment(new System.IO.MemoryStream(attachment.fileStream), attachment.name);

                    mail._Attachments.Add(att);
                }
            }

            //Distribution list
            if (model.distributionListId != null)
            {
                DistributionList list = getDistributionList((long)model.distributionListId);

                if (list != null)
                {
                    foreach (Member member in list.Members)
                    {
                        mail._To(member.email, member.name);
                        Debug.WriteLine(String.Format("Member {0}-{1}", member.email, member.name));
                    }

                    _log.info(model.id, String.Format("Distribution list '{0}' con {1:N0} utenti", list.name, list.Members.Count));
                }
            }

            // INVIO tramite SMTP
            string esito;
            esito = mail.send();

            //UPDATE STATUS
            MessaggeQueueManager manager = new MessaggeQueueManager(_connection);
            if (String.IsNullOrEmpty(esito))
            {
                //OK
                manager.updateStatus(model.id, MessageBase.EnumMessageStatus.Trasmissione_completata_con_successo, "");
                _log.info(model.id, MessageBase.EnumMessageStatus.Trasmissione_completata_con_successo.ToString().Replace("_", " "));
                return true;
            }

            //Errore di trasmissione
            manager.updateStatus(model.id, MessageBase.EnumMessageStatus.Errore_di_trasmissione, esito);
            _log.error(model.id, "Errore durante la trasmissione: " + esito);
            return false;
        }


        public Email getEmail(long emailId)
        {
            _strSQL = "SELECT t1.* , t2.distribution_list_id " +
                       " FROM mmq.Email as t1 " +
                       " join mmq.Message as t2 on t1.id = t2.id " +
                       " WHERE t1.id = " + emailId;

            _dt = _fillDataTable(_strSQL);

            if (_dt.Rows.Count == 0)
            {
                return null;
            }

            if (_dt.Rows.Count > 1)
            {
                throw new MyManagerCSharp.MyException(MyManagerCSharp.MyException.ErrorNumber.Record_duplicato);
            }

            Email email;
            email = new Email(_dt.Rows[0]);

            setAttachments(email);

            return email;
        }

        public void setAttachments(Email email)
        {
            _strSQL = "SELECT * FROM mmq.Attachment WHERE message_id = " + email.id;
            _dt = _fillDataTable(_strSQL);

            if (_dt.Rows.Count == 0)
            {
                return;
            }

            Attachment attachment;
            foreach (DataRow row in _dt.Rows)
            {
                attachment = new Attachment(row);
                email.Attachments.Add(attachment);
            }
        }

        public DistributionList getDistributionList(long id)
        {
            DistributionListManager manager = new DistributionListManager(_connection);
            return manager.getDistributionList(id);
        }

    }
}

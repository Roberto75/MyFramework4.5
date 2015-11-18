using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.MessageQueue.Models
{
    public class MessageBase
    {

        public enum EnumSendType
        {
            Sync = 1,
            Async = 2
        }

        public enum EnumMessageStatus
        {
            Trasmissione_completata_con_successo = 1,
            Trasmissione_in_corso = 2,
            Errore_di_trasmissione = -1
        }

        public enum EnumMessageType
        {
            Email,
            SMS,
            FAX,
            PUSH
        }

        public long id { get; set; }

        public DateTime dateAdded { get; set; }
        public DateTime? dateTransmission { get; set; }

        public EnumMessageStatus? MessageStatus { get; set; }
        public EnumMessageType MessageType { get; set; }
        public EnumSendType SendType { get; set; }

        public string errorMessage { get; set; }
        public Guid? comunicationId { get; set; }


        public long? distributionListId { get; set; }
        public DistributionList distributionList { get; set; }


        public List<Models.Log> Logs { get; set; }

        public MessageBase()
        {

        }

        public MessageBase(System.Data.DataRow row)
        {
            id = long.Parse(row["id"].ToString());
            errorMessage = row["error_message"].ToString();

            if (row["comunication_id"] is DBNull)
            {
                comunicationId = null;
            }
            else
            {
                comunicationId = Guid.Parse(row["comunication_id"].ToString());
            }


            if (row["date_added"] is DBNull)
            {
                dateAdded = DateTime.MinValue;
            }
            else
            {
                dateAdded = DateTime.Parse(row["date_added"].ToString());
            }

            if (row["date_transmission"] is DBNull)
            {
                dateTransmission = null;
            }
            else
            {
                dateTransmission = DateTime.Parse(row["date_transmission"].ToString());
            }

            if (row["distribution_list_id"] is DBNull)
            {
                distributionListId = null;
            }
            else
            {
                distributionListId = long.Parse(row["distribution_list_id"].ToString());
            }


            if (row["message_status"] is DBNull)
            {
                MessageStatus = null;
            }
            else
            {
                MessageStatus = (EnumMessageStatus)Enum.Parse(typeof(EnumMessageStatus), row["message_status"].ToString());
            }

            MessageType = (EnumMessageType)Enum.Parse(typeof(EnumMessageType), row["message_type"].ToString());
            SendType = (EnumSendType)Enum.Parse(typeof(EnumSendType), row["send_type"].ToString());
        }

    }
}

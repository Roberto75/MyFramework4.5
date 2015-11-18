using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.MessageQueue.Models
{
    public class DistributionList
    {
        public enum EnumDistributionType
        {
            email = 1,
            number = 2,
            device = 3
        }

        public long id { get; set; }
        public string name { get; set; }
        public string nota { get; set; }

        public DateTime dateAdded { get; set; }
        public DateTime dateModified { get; set; }

        public EnumDistributionType distributionType { get; set; }


        public List<Member> Members { get; set; }


        public DistributionList()
        {
            Members = new List<Member>();
        }


        public DistributionList(System.Data.DataRow row)
        {
            id = long.Parse(row["id"].ToString());
            name = row["name"].ToString();
            nota = row["nota"].ToString();
            dateAdded = (row["date_added"] is DBNull) ? DateTime.MinValue : DateTime.Parse(row["date_added"].ToString());
            dateModified = (row["date_modified"] is DBNull) ? DateTime.MinValue : DateTime.Parse(row["date_modified"].ToString());

            distributionType = (EnumDistributionType)Enum.Parse(typeof(EnumDistributionType), row["distribution_type"].ToString());
        }



    }
}

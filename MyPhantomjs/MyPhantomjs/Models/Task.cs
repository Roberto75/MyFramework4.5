using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Phantomjs.Models
{
    public class Task
    {
        public enum EnumEsito
        {
            Success,
            Failed
        }

        public My.Phantomjs.Phantomjs.EnumTaskType TaskType { get; set; }
        public System.Uri Url { get; set; }
        public string fileName { get; set; }
        public bool cacheEnabled { get; set; }
        
        public EnumEsito Esito { get; set; }
        public string Messaggio { get; set; }

        private System.IO.FileInfo _outputFile;
        public System.IO.FileInfo OutputFile
        {
            set
            {
                _outputFile = value;
            }

            get
            {
                return _outputFile;
            }
        }

    }
}

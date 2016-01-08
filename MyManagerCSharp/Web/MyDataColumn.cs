using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Shared.Web
{
    public class MyDataColumn
    {
        public bool CanSort { get; set; }

        public string PropertyName { get; set; }
        public string ColumnName { get; set; }
        // public Func<dynamic, object> Format { get; set; }

        public string Header { get; set; }

        //public string Style { get; set; }

        public MyDataColumn(string propertyName, string header, bool canSort)
        {
            this.PropertyName = propertyName;
            this.ColumnName = propertyName;
            this.Header = header;
            this.CanSort = canSort;
        }

        public MyDataColumn(string propertyName, string columnName , string header, bool canSort)
        {
            this.PropertyName = propertyName;
            this.ColumnName = columnName;
            this.Header = header;
            this.CanSort = canSort;
        }
    }
}

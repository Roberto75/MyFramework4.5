using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Shared.Web
{
    public class MyDataColumn
    {
        public enum ButtonType
        {
            Details,
            Delete,
            Edit
        }

        public bool CanSort { get; set; }
        public string PropertyName { get; set; }
        public string ColumnName { get; set; }
        public Func<dynamic, object> Format { get; set; }
        public string Header { get; set; }


        public ButtonType? Button { get; set; }

        //public string Style { get; set; }

        public MyDataColumn(string propertyName, string header, bool canSort)
        {
            this.PropertyName = propertyName;
            this.ColumnName = propertyName;
            this.Header = header;
            this.CanSort = canSort;
            this.Format = null;
            this.Button = null;
        }

        public MyDataColumn(string propertyName, string columnName, string header, bool canSort)
        {
            this.PropertyName = propertyName;
            this.ColumnName = columnName;
            this.Header = header;
            this.CanSort = canSort;
            this.Format = null;
            this.Button = null;
        }

        public MyDataColumn(string propertyName, string columnName, string header, bool canSort, Func<dynamic, object> format)
        {
            this.PropertyName = propertyName;
            this.ColumnName = columnName;
            this.Header = header;
            this.CanSort = canSort;
            this.Format = format;
            this.Button = null;
        }

        public MyDataColumn(ButtonType button, Func<dynamic, object> url )
        {
            this.Button = button;
            this.CanSort = false;
            this.Format = url;
            if (Button == ButtonType.Details)
            {
                Header = "Info";
            }
        }

    }
}

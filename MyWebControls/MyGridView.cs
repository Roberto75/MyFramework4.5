//*** http://aspalliance.com/666

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

#endregion

namespace MyWebControls
{

    public class MyGridView : GridView
    {

#region Data Set

        //protected override void OnRowDeleting(GridViewDeleteEventArgs e)
        //{
        //    //Lo ignoro...   
        //    //base.OnRowDeleting(e);
        //}

        //protected override void OnRowDeleted(GridViewDeletedEventArgs e)
        //{
        //    base.DataBind ();
        //}

       

#endregion

#region Logging

        private bool _enableLoggingCommand;

        [
        Description("Attiva il Logging dei comandi del GridView"),
        Category("Behavior"),
        DefaultValue("false"),
        ]
        public bool EnableLoggingCommand
        {
            get { return _enableLoggingCommand; }
            set { _enableLoggingCommand = value; }
        }


        protected override void OnRowCommand(GridViewCommandEventArgs e)
        {
            if (_enableLoggingCommand)
            {
                string  strSQL ;
                string accountNT = this.Page.User.Identity.Name ;
                strSQL = "INSERT INTO LOG_UTENTE_NT ( account, type, azione, nota )" + 
                             " VALUES ( '" + accountNT + "','GridView Command', '" + e.CommandName + "', '" + e.CommandArgument  +  "')";

                string provider;
                provider = System.Configuration.ConfigurationManager.ConnectionStrings["LogGridViewCommand"].ProviderName;
        
                System.Data.Common.DbProviderFactory factory;
                factory = System.Data.Common.DbProviderFactories.GetFactory(provider);

                System.Data.Common.DbConnection connection;
                connection = factory.CreateConnection();
                connection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["LogGridViewCommand"].ConnectionString;


                System.Data.Common.DbCommand command;
                command = factory.CreateCommand();
                command.CommandText = strSQL;
                command.Connection = connection;
                              
                connection.Open();

                try
                {
                    command.ExecuteNonQuery(); 
                } finally  {
                    command.Dispose();

                    connection.Close();
                    connection.Dispose();
                }
            }

            base.OnRowCommand(e);
        }

#endregion

#region SQL Data Souce

        //public void setSqlDataSource(SqlDataSource sqlDataSource)
        //{
        //    //Controllo che l'oggetto sqlDatasource non sia già presente nella pagina altrimenti si avrebbe un errore due oggetti con lo stesso id 
        //    //if (Page.FindControl(sqlDataSource.ID) == null)
        //    //{
        //      //  Page.Controls.Add(sqlDataSource);
        //    //}
        //    //Associo l'oggetto sqldataSource al gridView
        //    if (this.DataSourceID != sqlDataSource.ID)
        //    {
        //        this.DataSourceID = sqlDataSource.ID;
        //    }
        //    // intercetto l'evento di fine DataSource per ottenere il numero totale di records..
        //    sqlDataSource.Selected += new SqlDataSourceStatusEventHandler(dataSource_Selected);
             
        //}

        
        ///* Questa soluzione potrebbe creare problemi se ci sono 2 GridView nella stessa pagina
        // in quanto mi troverei con diversi datasource con lo stesso ID*/
        //public void setSqlDataSource(string selectCommand, string connectionName)
        //{
        //    string provider = System.Configuration.ConfigurationManager.ConnectionStrings[connectionName].ProviderName;
        //    string connectionString =  System.Configuration.ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
        //    // per le connessioni ai file Access cerco il Path Assoluto
        //    if (provider == "System.Data.OleDb")
        //    {
        //        connectionString = connectionString.Replace("~", System.AppDomain.CurrentDomain.BaseDirectory); 
        //    }

        //   System.Web.UI.WebControls.SqlDataSource dataSource = new SqlDataSource(provider, connectionString, selectCommand);
        //   dataSource.ID = base.ID + "_DataSourceID";
        //   this.setSqlDataSource(dataSource);
        //}


        //05/05/2010 
        protected override void OnInit(EventArgs e)
        {
            
            if ((this.DataSourceID != "") && (this.DataSourceObject != null))
            {
               // throw new ApplicationException("MyGridView with DataSource NULL");
                ((SqlDataSource)DataSourceObject).Selected += new SqlDataSourceStatusEventHandler(dataSource_Selected);
            }
 
            base.OnInit(e);
        }
    


        // intercetto l'evento di fine DataSource per ottenere il numero totale di records..
        void dataSource_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            _numeroDiRecordTotali = e.AffectedRows ;
        }

#endregion

#region Paginazione

        protected MyGridViewPagerTemplate _myGridViewPagerTemplate;
        private  int _numeroDiRecordTotali;

        public int numeroDiRecordTotali
        {
            get { return _numeroDiRecordTotali; }
        }



        //***  se abilito la paginazione utilizzo il template  **
        public override bool AllowPaging
        {
            get
            {
                return base.AllowPaging;
            }
            set
            {
                // se true abilito la paginazione con il nuovo template
                if (value)
                {
                    _myGridViewPagerTemplate = new MyGridViewPagerTemplate();
                    // intercetto e gestico l'vento per paginazione...
                    _myGridViewPagerTemplate.OnPageSizeChanged += new MyGridViewPagerTemplate.PageSizeEventHandler(PagerTemplate_OnPageSizeChanged);
                   
                     base.PagerTemplate = _myGridViewPagerTemplate;
                   //base.GridView1_PageIndexChanging  += new   (
                }
                else
                {
                    base.PagerTemplate = null;
                }
                base.AllowPaging = value;
            }
        }


        protected override void OnPageIndexChanging(GridViewPageEventArgs e)
        {
            base.PageIndex = e.NewPageIndex;
           // base.OnPageIndexChanging(e);
        }
       
 
        protected void PagerTemplate_OnPageSizeChanged(object sender, MyGridViewPagerTemplateEventArgs e)
        {

            base.PageSize = e.pageSize;
            
           //base.DataBind();
        }
      
      

        
#endregion

#region Properties
        /// <summary>
        /// Enable/Disable MultiColumn Sorting.
        /// </summary>
        [
        Description("Whether Sorting On more than one column is enabled"),
        Category("Behavior"),
        DefaultValue("false"),
        ]
        public bool AllowMultiColumnSorting
        {
            get
            {
                object o = ViewState["EnableMultiColumnSorting"];
                return (o != null ? (bool)o : false);
            }
            set
            {
                AllowSorting = true;
                ViewState["EnableMultiColumnSorting"] = value;
            }
        }
        /// <summary>
        /// Get or Set Image location to be used to display Ascending Sort order.
        /// </summary>
        [
        Description("Image to display for Ascending Sort"),
        Category("Misc"),
        Editor("System.Web.UI.Design.UrlEditor", typeof(System.Drawing.Design.UITypeEditor)),
        DefaultValue(""),

        ]
        public string SortAscImageUrl
        {
            get
            {
                object o = ViewState["SortImageAsc"];
                return (o != null ? o.ToString() : "");
            }
            set
            {
                ViewState["SortImageAsc"] = value;
            }
        }
        /// <summary>
        /// Get or Set Image location to be used to display Ascending Sort order.
        /// </summary>
        [
        Description("Image to display for Descending Sort"),
        Category("Misc"),
        Editor("System.Web.UI.Design.UrlEditor", typeof(System.Drawing.Design.UITypeEditor)),
        DefaultValue(""),
        ]
        public string SortDescImageUrl
        {
            get
            {
                object o = ViewState["SortImageDesc"];
                return (o != null ? o.ToString() : "");
            }
            set
            {
                ViewState["SortImageDesc"] = value;
            }
        }
#endregion

#region Ordinamento
        
        protected override void OnSorting(GridViewSortEventArgs e)
        {
            if (AllowMultiColumnSorting)
                e.SortExpression = GetSortExpression(e); 
            base.OnSorting(e);
        }


        protected override void OnRowCreated(GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (SortExpression != String.Empty)
                    DisplaySortOrderImages(SortExpression, e.Row);
            }
            base.OnRowCreated(e);
        }


        /// <summary>
        ///  Get Sort Expression by Looking up the existing Grid View Sort Expression 
        protected string GetSortExpression1(GridViewSortEventArgs e)
        {

            String order = (String)ViewState["order"];

            if (order == null)
            {
                order = "DESC";
                ViewState.Add("order", order);

            }

            else
            {

                if (order.Equals("ASC"))
                {

                    order = "DESC";

                }
                else
                {

                    order = "ASC";

                }
                ViewState["order"] = order;
            }

            return e.SortExpression + " " + order;

        }
        /// </summary>
      
        protected string GetSortExpression(GridViewSortEventArgs e)
        {
            string[] sortColumns = null;
            string sortAttribute = SortExpression;

            //Check to See if we have an existing Sort Order already in the Grid View.	
            //If so get the Sort Columns into an array
            if (sortAttribute != String.Empty)
            {
                sortColumns = sortAttribute.Split(",".ToCharArray());
            }

            //if User clicked on the columns in the existing sort sequence.
            //Toggle the sort order or remove the column from sort appropriately
            bool colonnaTrovata = false;

            if (sortColumns != null)
            {

                for (int i = 0; i < sortColumns.Length; i++)
                {
                    String colonna = sortColumns[i];
                    colonna = colonna.Replace("ASC", "");
                    colonna = colonna.Replace("DESC", "");
                    colonna = colonna.Trim();

                    if (colonna == e.SortExpression)
                    {
                sortAttribute = ModifySortExpression(sortColumns, e.SortExpression);
                        colonnaTrovata = true;
                        break;
                    }


                }
            }
            if (colonnaTrovata == false)
            {
                sortAttribute += String.Concat(",", e.SortExpression, " ASC ");
            }


            return sortAttribute.TrimStart(",".ToCharArray()).TrimEnd(",".ToCharArray());

        }
        /// <summary>
        ///  Toggle the sort order or remove the column from sort appropriately
        /// </summary>
        protected string ModifySortExpression(string[] sortColumns, string sortExpression)
        {

            string ascSortExpression = String.Concat(sortExpression, " ASC ");
            string descSortExpression = String.Concat(sortExpression, " DESC ");

            for (int i = 0; i < sortColumns.Length; i++)
            {

                if (ascSortExpression.Equals(sortColumns[i]))
                {
                    sortColumns[i] = descSortExpression;
                }

                else if (descSortExpression.Equals(sortColumns[i]))
                {
                    Array.Clear(sortColumns, i, 1);
                }
            }

            return String.Join(",", sortColumns).Replace(",,", ",").TrimStart(",".ToCharArray());

        }
        /// <summary>
        ///  Lookup the Current Sort Expression to determine the Order of a specific item.
        /// </summary>
        protected void SearchSortExpression(string[] sortColumns, string sortColumn, out string sortOrder, out int sortOrderNo)
        {

            //sortColumns sono la lista delle colonne cliccate
            //sortColumn è l'ultima colonna cliccata

            sortOrder = "";
            sortOrderNo = -1;
            for (int i = 0; i < sortColumns.Length; i++)
            {
                String colonna = sortColumns[i];
                colonna = colonna.Replace("ASC", "");
                colonna = colonna.Replace("DESC", "");
                colonna = colonna.Trim();

                if (colonna.Equals(sortColumn))
                {
                    sortOrderNo = i + 1;
                    if (AllowMultiColumnSorting)
                        sortOrder = sortColumns[i].Substring(sortColumn.Length).Trim();
                    else
                        sortOrder = ((SortDirection == SortDirection.Ascending) ? "ASC" : "DESC");
                }
            }
        }
        /// <summary>
        ///  Display a graphic image for the Sort Order along with the sort sequence no.
        /// </summary>
        protected void DisplaySortOrderImages(string sortExpression, GridViewRow dgItem)
        {
            string[] sortColumns = sortExpression.Split(",".ToCharArray());


            //Scorre tutte le colonne  della griglia verificando che la colonna sia  un LinkButton
            for (int i = 0; i < dgItem.Cells.Count; i++)
            {
                if (dgItem.Cells[i].Controls.Count > 0 && dgItem.Cells[i].Controls[0] is LinkButton)
                {
                    string sortOrder;
                    int sortOrderNo;

                    //Recupero la colonna corrente della grid
                    string column = ((LinkButton)dgItem.Cells[i].Controls[0]).CommandArgument;
                    SearchSortExpression(sortColumns, column, out sortOrder, out sortOrderNo);
                    if (sortOrderNo > 0)
                    {
                        string sortImgLoc = (sortOrder.Equals("ASC") ? SortAscImageUrl : SortDescImageUrl);

                        if (sortImgLoc != String.Empty)
                        {
                            Image imgSortDirection = new Image();
                            imgSortDirection.ImageUrl = sortImgLoc;
                            dgItem.Cells[i].Controls.Add(imgSortDirection);
                            Label lblSortOrder = new Label();
                            lblSortOrder.Font.Size = FontUnit.Small;
                            lblSortOrder.Text = sortOrderNo.ToString();
                            dgItem.Cells[i].Controls.Add(lblSortOrder);

                        }
                        else
                        {

                            Label lblSortDirection = new Label();
                            lblSortDirection.Font.Size = FontUnit.XSmall;
                            lblSortDirection.Font.Name = "webdings";
                            lblSortDirection.EnableTheming = false;
                            lblSortDirection.Text = (sortOrder.Equals("ASC") ? "5" : "6");
                            dgItem.Cells[i].Controls.Add(lblSortDirection);

                            if (AllowMultiColumnSorting)
                            {
                                Literal litSortSeq = new Literal();
                                litSortSeq.Text = sortOrderNo.ToString();
                                dgItem.Cells[i].Controls.Add(litSortSeq);

                            }


                        }

                    }

                }
            }

        }
        #endregion

    }
}
























































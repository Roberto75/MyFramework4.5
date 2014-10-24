namespace MyWebControls
{

    /*
     * http://www.visual-basic.it/articoli/eventnet.htm 
     * http://www.microsoft.com/italy/msdn/library/default.asp?url=/italy/msdn/library/vstudio/net/vb_programmatically.asp?frame=true
     * http://www.telerik.com/help/radgrid/v3_NET2/grdSettingPagerTemplate.html
     */

    public class MyGridViewPagerTemplate : System.Web.UI.ITemplate
    {
        //protected  solo da una classe estende la classe
        //friend limita il metodo nell'assembly


        //Ci serve un gestore di eventi 'nostro', che ricalchi la firma dei 'normali' gestori di eventi, e cioè
        public delegate void PageSizeEventHandler(object sender, MyGridViewPagerTemplateEventArgs  e);

        /*dichiaro un evento di nome "OnPageSizeChanged" di tipo "PageSizeEventHandler" il Delegate appena dichiarato
         * esempio di dichiarazione esplicita di un evento nella quale è compito
         * del programmatore la creazione delle classi relative all'evento e non in modo automatico 
         *
         * In questo modo l'evento viene definito del TIPO del gestore degli eventi delegato
         */
        public event PageSizeEventHandler OnPageSizeChanged;
        /* Catturo l'evento generato dalla combo e ne genero uno nostro che 
      * verrà catturato dal MyGrid
      */
        
        private void MyPager_PageSizeOnChange(object sender, System.EventArgs e)
        {
            System.Web.UI.WebControls.DropDownList combo;
            combo = (System.Web.UI.WebControls.DropDownList)sender;
            if (OnPageSizeChanged != null)
            {
                //genero l'evento
                OnPageSizeChanged(this, new MyGridViewPagerTemplateEventArgs(int.Parse(combo.SelectedValue)));
            }
        }

        //private void MyPager_PageNumberOnChange(object sender, System.Web.UI.ImageClickEventArgs e)
        //{

        //    System.Web.UI.WebControls.ImageButton imageButton;
        //    imageButton = (System.Web.UI.WebControls.ImageButton)sender;
        //    //devo risalire al numero della pagina inserito nella textBox

        //    System.Web.UI.WebControls.TextBox textBox;
        //    textBox = (System.Web.UI.WebControls.TextBox)imageButton.NamingContainer.FindControl("numeroPaginaCorrente");

        //    imageButton.CommandArgument = (int.Parse(textBox.Text)).ToString();          
        //}



        /* Evento generato durante il Binding del Pager.
         * Solo in questo momento posso risalire ai dati 
         */
        private void MyPager_DataBinding(object sender, System.EventArgs e)
        {
            System.Web.UI.WebControls.PlaceHolder ph = (System.Web.UI.WebControls.PlaceHolder)sender;
            MyWebControls.MyGridView  myGridView;

            myGridView = (MyWebControls.MyGridView)ph.NamingContainer.NamingContainer;
            long numeroPaginaCorrente = myGridView.PageIndex + 1;
            long numeroPagineTotali = myGridView.PageCount;

            ((System.Web.UI.WebControls.Literal)ph.FindControl("numeroPagineTotali")).Text = " of " + numeroPagineTotali;

            long numeroRecodTotali = 0;
            // dal SQLDataSource ricavo il dataSet per ottenere il numero totale di record....
            // questo tipo di soluzione mi comporta un'accesso al DB!!
            // System.Web.UI.WebControls.SqlDataSource dataSource;
            //dataSource = (System.Web.UI.WebControls.SqlDataSource) ph.Page.FindControl (myGridView.DataSourceID  );
            //System.Data.DataView dataView = ((System.Data.DataView) dataSource.Select(System.Web.UI.DataSourceSelectArguments.Empty));
            //numeroRecodTotali = dataView.Table.Rows.Count;
            numeroRecodTotali = myGridView.numeroDiRecordTotali;
            
            ((System.Web.UI.WebControls.Literal)ph.FindControl("record")).Text = "Record " + (myGridView.Rows[0].DataItemIndex + 1) + " to " + (myGridView.Rows[myGridView.Rows.Count - 1].DataItemIndex + 1) + " of " + numeroRecodTotali;


            string javascript = "if ((this.value > " + numeroPagineTotali + ") || (this.value < 1)) { alert(\"Il valore deve essere compreso tra 1 -" + numeroPagineTotali + "\");return false;}";


            System.Web.UI.WebControls.TextBox textBox;
            textBox = (System.Web.UI.WebControls.TextBox)ph.FindControl("numeroPaginaCorrente");
            textBox.Text = numeroPaginaCorrente.ToString();
            textBox.Attributes.Add("onBlur", "javascript:if (checkIsNumber2(this)) {" + javascript + " }");


            System.Web.UI.WebControls.DropDownList combo;
            combo = (System.Web.UI.WebControls.DropDownList)ph.FindControl("comboPageSize");
            combo.Items.FindByValue(myGridView.PageSize + "").Selected = true;

        }




        
       
        public void InstantiateIn(System.Web.UI.Control container)
        {

            /* Risalgo al GridView per catturare il suo evento di Binding
             * così mi leggo i suoi dati sul numero di record, numero di pagine, ecc...
             */


            System.Web.UI.WebControls.PlaceHolder ph = new System.Web.UI.WebControls.PlaceHolder();
            ph.DataBinding += new System.EventHandler (this.MyPager_DataBinding );

            System.Web.UI.WebControls.ImageButton button;
            System.Web.UI.WebControls.Literal literal;
            System.Web.UI.WebControls.Table table = new System.Web.UI.WebControls.Table() ;
            System.Web.UI.WebControls.TableRow  tr = new System.Web.UI.WebControls.TableRow ();
            System.Web.UI.WebControls.TableCell td = new System.Web.UI.WebControls.TableCell ();


            // *** TD 1 ***
            literal = new System.Web.UI.WebControls.Literal();
            literal.ID = "record";
            

            td = new System.Web.UI.WebControls.TableCell();
            td.CssClass = "td1";
            td.Controls.Add(literal);
            tr.Controls.Add(td);

           
            // *** TD 2 ***
            td = new System.Web.UI.WebControls.TableCell();
            td.CssClass = "td2";
            
            button = new System.Web.UI.WebControls.ImageButton();
            button.ImageUrl = "~/admin/images/first_16x16.gif";
            button.CssClass = "btnFirst";
            button.CommandName = "Page";
            button.CommandArgument = "First";
            button.ID = "btnFirst";
            td.Controls.Add(button);


            button = new System.Web.UI.WebControls.ImageButton();
            button.ImageUrl = "~/admin/images/back_16x16.gif";
            button.CssClass = "btnBack";
            button.CommandName = "Page";
            button.CommandArgument = "Prev";
            button.ID = "btnBack";
            td.Controls.Add(button);

            literal = new System.Web.UI.WebControls.Literal ();
            literal.Text = "Page ";
            td.Controls.Add(literal);

            System.Web.UI.WebControls.TextBox textBox = new System.Web.UI.WebControls.TextBox();
            textBox.ID = "numeroPaginaCorrente";
            textBox.MaxLength = 4;
            textBox.Width = 20;
            td.Controls.Add(textBox);


            // bottone con immagine...
            //button = new System.Web.UI.WebControls.ImageButton();
            //button.ImageUrl = "~/images/btnApri.gif";
            //button.CssClass = "btnGo";
            //button.CommandName = "Page";
            //button.ID = "btnGo";
            //button.Click += new System.Web.UI.ImageClickEventHandler(this.MyPager_PageNumberOnChange);
            //td.Controls.Add(button);

            
            //OPPURE un semplice link
            System.Web.UI.WebControls.LinkButton  link = new System.Web.UI.WebControls.LinkButton ();
            link.Text = "Go";
            link.CommandName = "Page";
            link.ID = "btnGo";
            link.Click += new System.EventHandler(link_Click);
            td.Controls.Add(link);

        
          

            literal = new System.Web.UI.WebControls.Literal();
            literal.ID = "numeroPagineTotali";
            td.Controls.Add(literal);

            button = new System.Web.UI.WebControls.ImageButton();
            button.ImageUrl = "~/admin/images/next_16x16.gif";
            button.CssClass = "btnNext";
            button.CommandName = "Page";
            button.CommandArgument = "Next";
            button.ID = "btnNext";
            td.Controls.Add(button);


            button = new System.Web.UI.WebControls.ImageButton();
            button.ImageUrl = "~/admin/images/last_16x16.gif";
            button.CssClass = "btnLast";
            button.CommandName = "Page";
            button.CommandArgument = "Last";
            button.ID = "btnLast";
            td.Controls.Add(button);
            tr.Controls.Add(td);


            // *** TD 3 ***
            td = new System.Web.UI.WebControls.TableCell();
            td.CssClass = "td3";

            literal = new System.Web.UI.WebControls.Literal();
            literal.Text  = "Page size: ";
            td.Controls.Add(literal);


            System.Web.UI.WebControls.DropDownList combo = new System.Web.UI.WebControls.DropDownList ();
            combo.ID = "comboPageSize";
            combo.CssClass = "combo";
            combo.AutoPostBack = true ;
            //combo.Items.Add(new System.Web.UI.WebControls.ListItem("1", "1"));
            //combo.Items.Add(new System.Web.UI.WebControls.ListItem("2", "2"));
            combo.Items.Add(new System.Web.UI.WebControls.ListItem("5", "5"));
            combo.Items.Add(new System.Web.UI.WebControls.ListItem("10", "10"));
            combo.Items.Add(new System.Web.UI.WebControls.ListItem("20", "20"));
            combo.Items.Add(new System.Web.UI.WebControls.ListItem("50", "50"));
            combo.SelectedIndexChanged += new System.EventHandler (this.MyPager_PageSizeOnChange);

            //combo.SelectedIndexChanged += new S ); 
            //combo.SelectedIndexChanged += new System.EventHandler(this.PageSizeEventHandler); 


            td.Controls.Add(combo);


            tr.Controls.Add(td);


            table.Rows.Add(tr);
            ph.Controls.Add(table);


            container.Controls.Add(ph);

        }







        void link_Click(object sender, System.EventArgs e)
        {
            System.Web.UI.WebControls.LinkButton link = (System.Web.UI.WebControls.LinkButton)sender;
            //devo risalire al numero della pagina inserito nella textBox

            System.Web.UI.WebControls.TextBox textBox;
            textBox = (System.Web.UI.WebControls.TextBox)link.NamingContainer.FindControl("numeroPaginaCorrente");

            link.CommandArgument = (int.Parse(textBox.Text)).ToString();          
        }
           


    }







    public class MyGridViewPagerTemplateEventArgs : System.EventArgs
    {
        private int _pageSize;
        private System.Web.UI.WebControls.GridView _gridView;

        public MyGridViewPagerTemplateEventArgs(int pageSize)
        {
            this._pageSize = pageSize;

        }

        public MyGridViewPagerTemplateEventArgs(System.Web.UI.WebControls.GridView gridView, int pageSize)
        {
            this._pageSize = pageSize;
            this._gridView = gridView;

        }

        public int pageSize
        {
            get { return _pageSize; }
        }

        public System.Web.UI.WebControls.GridView gridView
        {
            get { return _gridView; }
        }

    }












}










// public event System.EventHandler MyPager_PageSizeOnChange ;



// public event MyPager_PageSizeOnChange = new System.EventHandler (MyPager_PageSizeOnChange );


// public delegate void MyPager_PageSizeOnChange(object sender, System.EventArgs e);


//public void MyPager_PageSizeOnChange(object sender, System.EventArgs e)
//{
//    System.Web.UI.WebControls.DropDownList combo;
//    combo = (System.Web.UI.WebControls.DropDownList)sender;
//    System.Web.UI.WebControls.GridView myGridView;

//    myGridView = (System.Web.UI.WebControls.GridView)combo.NamingContainer.NamingContainer;

//    myGridView.PageSize = int.Parse(combo.SelectedValue);
//    myGridView.DataBind();
//}







//public int pageSize
//{
//    get {




//        return _pageSize; 


//    }
//}





//  private int _pageSize = 2;



//public enum MyPageSize : int
//{
//    uno = 1,
//    due = 2,
//    cinque = 5,
//    dieci = 10,
//    venti = 20,
//    cinquanda = 50
//}



//MyPageSize _myPageSize;


//public MyGridViewPagerTemplate()
//{
//    //Valore di default per la combo
//    this._myPageSize = MyPageSize.dieci ; 
//}


//public MyGridViewPagerTemplate(MyPageSize value)
//{
//    this._myPageSize = value;
//}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace My.Angular.WebApplication.Controllers
{
    public class AngularController : Controller
    {

        //http://angularjsaz.blogspot.it/2015/09/tutorial-32-angularjs-works-with-web-api.html
        //http://www.infragistics.com/community/blogs/dhananjay_kumar/archive/2015/05/13/how-to-use-angularjs-in-asp-net-mvc-and-entity-framework-4.aspx

        public ActionResult Index()
        {
            return View();
        }
    }
}
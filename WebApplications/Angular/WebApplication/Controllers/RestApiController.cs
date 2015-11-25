using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace My.Angular.WebApplication.Controllers
{
    public class RestApiController : ApiController
    {
        [AllowAnonymous]
        public MyUsers.Models.MyUserSmall[] Get()
        {
           My


            return cusDLL.GetAll();
        }   

    }
}

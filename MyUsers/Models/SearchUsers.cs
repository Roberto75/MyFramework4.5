using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyUsers;

namespace MyUsers.Models
{
    public class SearchUsers : PagedUsers 
    {
        public MyUser filter = new MyUser();

        public SearchUsers()
        {
            this.Sort = "my_login";
            this.SortDir = "ASC";
        }
    }
}
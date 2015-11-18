using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyManagerCSharp.Alert.Models
{
    public class MyAlertsModel
    {
        //public System.Web.Mvc.MultiSelectList Alerts;

        public List<MyManagerCSharp.Alert.Models.MyAlert> Alerts;
        public bool? EsitoSalvataggio;

        private System.Net.Mail.MailAddress _email;
        public string Email
        {
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    _email = null;
                }
                else
                {
                    _email = new System.Net.Mail.MailAddress(value);
                }
            }

            get
            {
                if (_email == null)
                {
                    return "";
                }
                return _email.ToString();
            }
        }




    }
}

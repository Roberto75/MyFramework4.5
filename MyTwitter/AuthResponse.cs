using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTwitter
{
    public class AuthResponse
    {

        [Newtonsoft.Json.JsonProperty("token_type")]
        public string TokenType { get; set; }

        [Newtonsoft.Json.JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MyTwitter
{
    public class TwitterManager
    {

        string oAuthConsumerKey;
        string oAuthConsumerSecret;

        public TwitterManager(string consumerKey, string consumerSecret)
        {
            oAuthConsumerKey = consumerKey;
            oAuthConsumerSecret = consumerSecret;
        }


        public JsonTypes.Search search(string query)
        {
            // Do the Authenticate
            string authHeader = string.Format("Basic {0}",
                Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(Uri.EscapeDataString(oAuthConsumerKey) + ":" +
                Uri.EscapeDataString((oAuthConsumerSecret)))
            ));


            string postBody = "grant_type=client_credentials";

            System.Net.HttpWebRequest authRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create("https://api.twitter.com/oauth2/token");
            authRequest.Headers.Add("Authorization", authHeader);
            authRequest.Method = "POST";
            authRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            authRequest.AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate;



                using (System.IO.Stream stream = authRequest.GetRequestStream())
                {
                    byte[] content = System.Text.ASCIIEncoding.ASCII.GetBytes(postBody);
                    stream.Write(content, 0, content.Length);
                }

           


            authRequest.Headers.Add("Accept-Encoding", "gzip");
            System.Net.WebResponse authResponse = authRequest.GetResponse();

            AuthResponse twitAuthResponse = null;

            using (authResponse)
            {
                using (var reader = new System.IO.StreamReader(authResponse.GetResponseStream()))
                {
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var objectText = reader.ReadToEnd();
                    twitAuthResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<AuthResponse>(objectText);
                }
            }

            //https://dev.twitter.com/docs/using-search
            //query = "CVE 2014 3155";

           // string apiUrl = String.Format("https://api.twitter.com/1.1/search/tweets.json?q={0}&result_type=popular&count=10&src=typd", query);

            string apiUrl = String.Format("https://api.twitter.com/1.1/search/tweets.json?q={0}", query);
            var json = string.Empty;
            System.Net.HttpWebRequest apiRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(apiUrl);
        
            apiRequest.Headers.Add("Authorization", string.Format("{0} {1}", twitAuthResponse.TokenType, twitAuthResponse.AccessToken));
            apiRequest.Method = "Get";
            System.Net.WebResponse timeLineResponse = apiRequest.GetResponse();

            JsonTypes.Search result;
            using (timeLineResponse)
            {
                using (var reader = new System.IO.StreamReader(timeLineResponse.GetResponseStream()))
                {
                    json = reader.ReadToEnd();
                    // The below can be used to deserialize into a c# object
                    // result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TimeLine>>(json);
                    result = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonTypes.Search>(json);
                }
            }

            Debug.WriteLine("Json: " + json);
            Debug.WriteLine("Twitter search count: " + result.Results.Count ); 
            return result;
        }
    }
}

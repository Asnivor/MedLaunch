using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.IO;

namespace MedLaunch.Classes
{
    public class WebOps
    {
        public string BaseUrl { get; set; }
        public string Params { get; set; }
        public string BodyAsString { get; set; }
        public int Timeout { get; set; }

        public WebOps()
        {
            BaseUrl = "http://thegamesdb.net/api";
            Timeout = 10000; // start at 1 seconds
            BodyAsString = null;
        }

        public string GetResponseText(string address, int timeout)
        {
            var request = (HttpWebRequest)WebRequest.Create(address);
            request.Proxy = null;
            string responseStr = "";
            try
            {
                request.Timeout = timeout;
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    var encoding = Encoding.GetEncoding(response.CharacterSet);

                    using (var responseStream = response.GetResponseStream())
                    using (var reader = new StreamReader(responseStream, encoding))
                        responseStr = reader.ReadToEnd();
                }
            }
            catch (System.Net.WebException wex)
            {
                // error or timeout - run again but change the timeout value
                Timeout += 2000;
                responseStr = GetResponseText(address, Timeout);
                if (Timeout > 29000)
                {
                    return responseStr;
                }
            }
            return responseStr;
            
        }

        public string ApiCall()
        { 
            // run task
            string s = GetResponseText(BaseUrl + Params, Timeout);
            BodyAsString = s.ToString();
            if (Timeout > 20000)
            {
                return s;
            }

            if (s == null || s == "")
            {
                Timeout += 3001;
                s = ApiCall();
            }
            return s;
        }
    }
}

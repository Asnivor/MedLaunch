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
            Timeout = 20000; // start at 1 seconds
            BodyAsString = null;
        }

        public string GetResponseText(string address, int timeout)
        {
            var request = (HttpWebRequest)WebRequest.Create(address) as HttpWebRequest;
            request.Timeout = 30000;
            request.Proxy = null;
            request.KeepAlive = false;
            request.ServicePoint.ConnectionLeaseTimeout = 30000;
            request.ServicePoint.MaxIdleTime = 30000;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2228.0 Safari/537.36";


            string responseStr = "";
            try
            {
                request.Timeout = timeout;
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    var encoding = Encoding.GetEncoding(response.CharacterSet);

                    using (Stream objStream = response.GetResponseStream())
                    {
                        using (StreamReader objReader = new StreamReader(objStream))
                        {
                            responseStr = objReader.ReadToEnd();
                            objReader.Close();
                        }
                        objStream.Flush();
                        objStream.Close();
                    }
                    response.Close();

                    /*
                    using (var responseStream = response.GetResponseStream())
                    using (var reader = new StreamReader(responseStream, encoding))
                        responseStr = reader.ReadToEnd();
                        */
                }
            }
            catch (System.Net.WebException wex)
            {
                // error or timeout - run again but change the timeout value
                Console.WriteLine(wex);
                request.Abort();
                
                Timeout += 5000;
                responseStr = GetResponseText(address, Timeout);
                if (Timeout > 60000)
                {
                    return responseStr;
                }
            }
            finally
            {
                request.Abort();
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

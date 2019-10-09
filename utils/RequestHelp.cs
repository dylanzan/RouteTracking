using System;
using System.Net.Http;

namespace HelloWorld.utils
{
    class RequestHelp
    {
        public string GetAsync(string httpUrl)
        {
            HttpClient hc = null;

            string responseData = String.Empty;
            try
            {
                hc = new HttpClient();
                hc.DefaultRequestHeaders.Add("UserAgent", "curl");
                HttpResponseMessage hrm = hc.GetAsync(httpUrl).Result;

                if (hrm.IsSuccessStatusCode)
                {
                    responseData = hrm.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return responseData;
        }
    }
}

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
                hc.DefaultRequestHeaders.Add("User-Agent", "Chrome");
                hc.DefaultRequestHeaders.Add("Authorization", "10001 qwertyuiop123456asdfghjkl");
                HttpResponseMessage hrm = hc.GetAsync(httpUrl).Result;

                if (hrm.IsSuccessStatusCode)
                {
                    responseData = hrm.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return responseData;
        }
    }
}

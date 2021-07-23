using RouteTracking.model;
using System;
using System.Net.Http;
using RouteTracking.utils;

namespace HelloWorld.utils
{
    class RequestHelp
    {
        private string TOKEN_KEY = ConfigUtils.ConfigDict[ConstModel.KEY_SERVER_TOKEN];
        private string IPV4_REQUEST_URL = ConfigUtils.ConfigDict[ConstModel.KEY_IPV4_ADDRESS];
        private string IPV6_REQUEST_URL = ConfigUtils.ConfigDict[ConstModel.KEY_IPV6_ADDRESS];
        private string USER_AGENT_VALUE = ConfigUtils.ConfigDict[ConstModel.KEY_CLIENT_VERSION];
        private string GetAsync(string httpUrl)
        {
            HttpClient hc = null;

            string responseData = String.Empty;
            try
            {
                hc = new HttpClient();
                hc.DefaultRequestHeaders.Add("User-Agent", USER_AGENT_VALUE);
                hc.DefaultRequestHeaders.Add("Authorization", TOKEN_KEY);
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

        public string InquireIpInfo(string ipAddress)
        {

            RegexUtils reu = new RegexUtils();
            JsonParseUtils jsu = null;

            string ipZone = "";

            //判断ip 类型
            switch (reu.IPCheckForS(ipAddress))
            {
                case ConstModel.IPV4:
                    if (reu.IPCheck(ipAddress))
                    {
                        jsu = new JsonParseUtils();
                        string ipv4JsonResponse = this.GetAsync(IPV4_REQUEST_URL + ipAddress);
                        ipZone = jsu.JsonParse(ipv4JsonResponse);
                    }
                    break;
                case ConstModel.IPV6:
                    jsu = new JsonParseUtils();
                    ipZone = this.GetAsync(IPV6_REQUEST_URL + ipAddress);
                    break;
                case ConstModel.NOTHING:
                    break;
                default:
                    ipZone = ConstModel.NO_VALUE;
                    break;
            }
            return ipZone;
        }

        //暂时仅支持IPV4
        public string InquireLocalIp() 
        {
            string localIpZone = "";
            try
            {
                localIpZone=this.GetAsync(IPV4_REQUEST_URL);
                return localIpZone; 
            }
            catch
            {
                throw;
            }
            return "";
        }

    }
}

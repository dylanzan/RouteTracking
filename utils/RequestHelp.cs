﻿using System;
using System.Net.Http;

namespace HelloWorld.utils
{
    class RequestHelp
    {
        private string GetAsync(string httpUrl)
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

        public string InquireIpInfo(string ipAddress)
        {

            RegexUtils reu = new RegexUtils();
            JsonParseUtils jsu = null;

            string ipZone = "";

            //判断ip 类型
            switch (reu.IPCheckForS(ipAddress))
            {
                case "ipv4":
                    if (reu.IPCheck(ipAddress))
                    {
                        jsu = new JsonParseUtils();
                        string ipv4JsonResponse = this.GetAsync("http://39.96.177.233/" + ipAddress);
                        /*if (!String.IsNullOrEmpty(ipv4JsonResponse)) //server端问题，有时需要请求两次，概率很小，以防万一
                        {
                            ipZone = jsu.JsonParse(ipv4JsonResponse);
                        }
                        else
                        {
                            ipv4JsonResponse = this.GetAsync("http://39.96.177.233/" + ipAddress);
                            ipZone = jsu.JsonParse(ipv4JsonResponse);
                        }*/

                        if (String.IsNullOrEmpty(ipv4JsonResponse))
                        {
                            ipv4JsonResponse = this.GetAsync("http://39.96.177.233/" + ipAddress);
                            ipZone = jsu.JsonParse(ipv4JsonResponse);
                            break;
                        }
                        ipZone = jsu.JsonParse(ipv4JsonResponse);
                    }
                    break;
                case "ipv6":
                    jsu = new JsonParseUtils();
                    ipZone = this.GetAsync("http://freeapi.ipip.net/" + ipAddress);
                    break;
                case "nothing":
                    break;
                default:
                    ipZone = "No Value!";
                    break;
            }
            return ipZone;
        }

       public string InquireLocalIp()
        {
            string localIpZone = "";
            try
            {
                localIpZone=this.GetAsync("http://39.96.177.233");
                if (!String.IsNullOrEmpty(localIpZone))
                {
                    return localIpZone;
                }
                else //server端问题，有时需要请求两次，才能获取结果，出现此现象概率很低
                {
                    localIpZone= this.GetAsync("http://39.96.177.233");
                    return localIpZone;
                }
            }
            catch
            {
                throw;
            }
            return "";
        }

    }
}

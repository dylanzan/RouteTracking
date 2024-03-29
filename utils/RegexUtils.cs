﻿using RouteTracking.model;
using System.Net;
using System.Text.RegularExpressions;

namespace HelloWorld.utils
{
    class RegexUtils
    {
        public bool IPCheck(string IP)
        {
            return Regex.IsMatch(IP, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        //检测ip类型
        public string IPCheckForS(string ip)
        {
            IPAddress address;
            if (IPAddress.TryParse(ip, out address))
            {
                switch (address.AddressFamily)
                {
                    case System.Net.Sockets.AddressFamily.InterNetwork:
                        return ConstModel.IPV4;
                    case System.Net.Sockets.AddressFamily.InterNetworkV6:
                        return ConstModel.IPV6;
                    default:
                        return ConstModel.NOTHING;
                }
            }
            return ConstModel.NOTHING;
        }
    }
}

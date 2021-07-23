using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteTracking.model
{
    public class ConstModel
    {
        public const string VOID_PARAMS = "Please check if the parameter format you entered is correct.";
        public const string VOID_PORT_NUM = "Incorrect port number format";

        public const string VOID_VALUE = "Invalid value";
        public const string NO_VALUE = "Value is empty !";

        public const string REQUEST_ERROR = "Request exception"; 
        public const string NETWORK_ERROR = "Network connection is abnormal";

        //提示
        public const string PROMPT_RETRY = "Please try again";

        public const string TOOLS_ERROR_NMAP = "The namp tool does not exist or the first position";

        //Params
        public const string IPV4 = "ipv4";
        public const string IPV6 = "ipv6";
        public const string NOTHING = "nothing";

        public const string KEY_CLIENT_VERSION = "ClientVersion";
        public const string KEY_IPV4_ADDRESS = "Ipv4InfoAddress";
        public const string KEY_IPV6_ADDRESS = "Ipv6InfoAddress";
        public const string KEY_SERVER_TOKEN = "ServerTokenKeys";
        public const string KEY_ISDEBUG = "isDebug";

        //Port params
        public const int MAXIMUM_PORT_NUMBER = 65535;
        public const int MINIMUM_PORT_NUMBER = 0;


        //异常常量
        public const string DATA_FORMAT_ERROR = "System.FormatException";
    }
}

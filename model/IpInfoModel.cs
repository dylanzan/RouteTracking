using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld.model
{
    class IpInfoModel
    {

        public string code
        {
            get; set;
        }

        public Data data
        {
            get; set;
        }

        public string msg
        {
            get; set;
        }
        
    }

    public class Data
    {
        public string ipaddress
        {
            get; set;
        }
        public string cuntry
        {
            get; set;
        }

        public string local
        {
            get; set;
        }
    }
}

using HelloWorld.model;
using Newtonsoft.Json;

namespace HelloWorld.utils
{
    class JsonParseUtils
    {

        public string JsonParse(string jsonRes)
        {
            // Console.WriteLine(jsonRes);

            string ipinfoRes = "";
            //Console.WriteLine(ipifo.code);
            if (jsonRes != "")
            {
                IpInfoModel ipinfo = JsonConvert.DeserializeObject<IpInfoModel>(jsonRes);
                ipinfoRes = string.Format(" Area:{0}  Local:{1}", ipinfo.data.cuntry, ipinfo.data.local);
                return ipinfoRes;
            }
            else
            {
                return "plz try again.";
            }
            return ipinfoRes;
        }

    }
}

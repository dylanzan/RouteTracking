namespace HelloWorld.model
{
    /*
     {
    "code": 2000001,
    "data": {
        "IpStr": "1.1.1.1",
        "Address": "美国",
        "Operator": "APNIC&CloudFlare公共DNS服务器"
    },
    "msg": "请求处理成功"
}
    */

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
        public string IpStr
        {
            get; set;
        }
        public string Address
        {
            get; set;
        }

        public string Operator
        {
            get; set;
        }
    }
}

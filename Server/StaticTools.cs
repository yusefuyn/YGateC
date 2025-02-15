using YGate.Entities;
using YGate.Json.Operations;

namespace YGate.Server
{
    public static class StaticTools
    {

        public static string SiteName { get; set; } = "Yussefuynstein";

        public static int NumberOfAllowedRequests = 10;
        public static int AllowedRequestCountTimeout = 5;

        public static Token tokenService { get; set; }

        public static T ConvertParameters<T>(this RequestParameter parameter)
        {
            return YGate.Json.Operations.JsonDeserialize<T>.Deserialize(parameter.Parameters.ToString());
        }

        public static List<RequestLogObject> IpAndDate = new();
        public static List<string> BlockedIp =new();
        public static List<string> WhiteList = new();
        public static void AddRequest(string Ip) {
            RequestLogObject obj = IpAndDate.FirstOrDefault(xd => xd.Ip == Ip);

            if (WhiteList.Contains(Ip))
                return;

            if (obj == null)
            {
                IpAndDate.Add(new(Ip));
                return;
            }

            DateTime sonIstekZamanı = obj.RequestTime.LastOrDefault();


            int requestsInLastTeenSeconds = obj.RequestTime.Count(rt => (sonIstekZamanı - rt).TotalSeconds <= StaticTools.AllowedRequestCountTimeout);

            if (requestsInLastTeenSeconds >= StaticTools.NumberOfAllowedRequests)
                BlockedIp.Add(Ip);
            obj.AddRequest();
        }
    }
}

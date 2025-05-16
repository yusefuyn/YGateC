using YGate.Entities;
using YGate.Json;
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
            return new JsonOperations().Deserialize<T>(parameter.Parameters.ToString());
        }

        public static List<RequestLogObject> IpAndDate = new();
        public static List<string> BlockedIp = new();
        public static List<string> WhiteList = new();

        public static void AddRequest(string Ip, string Path, string Data)
        {
            RequestLogObject obj = IpAndDate.LastOrDefault(xd => xd.Ip == Ip);

            if (WhiteList.Contains(Ip))
                return;

            IpAndDate.Add(new(Ip, Path, Data));
            if (obj == null)
                return;

            int requestsInLastTeenSeconds = IpAndDate.Where(xd => xd.Ip == Ip && xd.RequestTime > DateTime.UtcNow.AddSeconds((StaticTools.AllowedRequestCountTimeout * -1))).Count();

            if (requestsInLastTeenSeconds >= StaticTools.NumberOfAllowedRequests)
                BlockedIp.Add(Ip);
        }
    }
}

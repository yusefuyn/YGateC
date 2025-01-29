using YGate.Entities;
using YGate.Json.Operations;

namespace YGate.Server
{
    public static class StaticTools
    {

        public static string SiteName { get; set; } = "Yussefuynstein";

        public static Token tokenService { get; set; }

        public static T ConvertParameters<T>(this RequestParameter parameter)
        {
            return YGate.Json.Operations.JsonDeserialize<T>.Deserialize(parameter.Parameters.ToString());
        }

        public static List<RequestLogObject> IpAndDate = new();
        public static List<string> BlockedIp =new();

        public static void AddRequest(string Ip) {
            RequestLogObject obj = IpAndDate.FirstOrDefault(xd => xd.Ip == Ip);
            if (obj == null)
            {
                IpAndDate.Add(new(Ip));
                return;
            }

            DateTime sonIstekZamanı = obj.RequestTime.LastOrDefault();


            int requestsInLastTeenSeconds = obj.RequestTime.Count(rt => (sonIstekZamanı - rt).TotalSeconds <= 3);

            if (requestsInLastTeenSeconds >= 10)
                BlockedIp.Add(Ip);
            obj.AddRequest();
        }
    }
}

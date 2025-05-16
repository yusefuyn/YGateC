using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http;
using YGate.Client.Services;
using YGate.Entities;
using YGate.Entities.BasedModel;
using YGate.Interfaces.DomainLayer;
using YGate.Json;

namespace YGate.Client
{
    public static class StaticTools
    {
        public static HttpClient httpClient;

        /// <summary>
        /// Dışarıdan js ile tetiklenebilir.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [JSInvokable]
        public static async Task<string> SaveParameter(List<PageParameter> parameters)
        {
            RequestParameter parameter = new() { DateTimeUTC = DateTime.UtcNow, Address = "/api/Page/SavePageParameters" };
            parameter.Parameters = parameters;
            StringContent stringContent = new(new JsonOperations().Serialize(parameter), System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage res = await httpClient.PostAsync("/api/Page/SavePageParameters", stringContent);
            string Returned = await res.Content.ReadAsStringAsync();
            return Returned;
        }

        /// <summary>
        /// Burası Result durumunu kontrol ediyor.
        /// Amacımız sadece objeyi almak ise sizin Result'u kontrol etmenize gerek yok.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <returns></returns>
        public static T ConvertRequestObject<T>(this RequestResult result)
        {
            if (result != null && result.Result == EnumRequestResult.Success)
                return new JsonOperations().Deserialize<T>(result.Object.ToString());
            else
                return default(T);
        }

        public static List<DateTime> MyRequestLog = new();

        public static string SiteName { get; set; } = "Yussefuynstein";

    }
}

using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;
using System.Net.Http;
using YGate.Entities;
using YGate.Interfaces.OperationLayer;
using YGate.Interfaces.DomainLayer;

namespace YGate.Client.Services
{
    public class HttpClientService
    {
        public HttpClient httpClient;
        NavigationManager navigationManager;
        CookieService cookieService;
        IJsonSerializer jsonSerializer;



        public HttpClientService(
            HttpClient httpClient,
            NavigationManager navigationManager,
            CookieService cookieService,
            IJsonSerializer jsonSerializer)
        {
            this.httpClient = httpClient;
            this.navigationManager = navigationManager;
            this.cookieService = cookieService;
            this.jsonSerializer = jsonSerializer;
        }

        public async Task<RequestResult> GetPostAsync<T>(RequestParameter requestParameter)
        { // Merkez burada.
            RequestResult returned = new RequestResult("")
            {
                Result = EnumRequestResult.Error,
                To = EnumTo.Client,
                Object = "",
            };


            // İstek kontrolünü.
            DateTime sonIstekZamanı = StaticTools.MyRequestLog.LastOrDefault();
            int requestsInLastTeenSeconds = StaticTools.MyRequestLog.Count(rt => (sonIstekZamanı - rt).TotalSeconds <= 3);
            if (requestsInLastTeenSeconds == 9)
            {
                returned.ShortDescription = "Let's wait a bit before making this request.";
                returned.LongDescription = "Let's wait a bit before making this request.";
                return returned;
            }

            if (string.IsNullOrEmpty(requestParameter.Address))
            {
                returned.LongDescription = "Address empty";
                returned.ShortDescription = "Address empty";
                returned.Result = EnumRequestResult.Error;

                return returned;
            }

            if (requestParameter.Supply < 1)
            {
                returned.LongDescription = "Supply small to 1";
                returned.ShortDescription = "Supply small to 1";
                returned.Result = EnumRequestResult.Error;

                return returned;
            }

            requestParameter.ParameterHash = "";
            requestParameter.ParameterHash = YGate.String.Operations.Hash.ComputeSHA512(jsonSerializer.Serialize(requestParameter));

            string Uri = navigationManager.BaseUri + requestParameter.Address;
            StringContent stringContent = new(jsonSerializer.Serialize(requestParameter), System.Text.Encoding.UTF8, "application/json");
            var res = await httpClient.PostAsync(Uri, stringContent);
            string Returned = await res.Content.ReadAsStringAsync();
            StaticTools.MyRequestLog.Add(DateTime.Now);
            RequestResult returnedObject = jsonSerializer.Deserialize<RequestResult>(Returned);
            return returnedObject;
        }

        public async Task<RequestResult> GetPostAsync<T>(object param, string Address)
        {
            var reqparam = new RequestParameter() { Address = Address, DateTimeUTC = DateTime.UtcNow, Parameters = param, Supply = 1 };
            var token = await cookieService.GetCookie("Token");

            if (!string.IsNullOrEmpty(token))
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));
            else
                httpClient.DefaultRequestHeaders.Authorization = null; // Header yoksa boş bırak


            return await GetPostAsync<T>(reqparam);
        }
    }
}

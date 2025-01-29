using YGate.Entities;

namespace YGate.Client.Services.Statistics
{
    public class StatisticsService : IStatisticsService
    {
        HttpClientService httpClientService;
        public StatisticsService(HttpClientService httpClientService)
        {
            this.httpClientService = httpClientService;
        }

        public async Task<RequestResult> GetSiteName()
        {
            var res = await httpClientService.GetPostAsync<RequestResult>(null, "api/Statistics/GetSiteName");
            return res;
        }

        public async Task<RequestResult> GetStatistics() 
        {
            var res = await httpClientService.GetPostAsync<RequestResult>(null, "api/Statistics/GetStatistics");
            return res;
        }

    }
}

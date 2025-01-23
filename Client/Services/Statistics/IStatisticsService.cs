using YGate.Entities;

namespace YGate.Client.Services.Statistics
{
    public interface IStatisticsService
    {

        public Task<RequestResult> GetStatistics();
    }
}

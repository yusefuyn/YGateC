using YGate.Entities;
using YGate.Entities.BasedModel;

namespace YGate.Client.Services.Page
{
    public interface IPageService
    {
        public Task<RequestResult> GetAsync(DynamicPageDynamicParameter page);
        public Task<RequestResult> GetPageButPPAsync(DynamicPageParameter page);
        public Task<RequestResult> Add(DynamicPage page);
        public Task<RequestResult> GetAll();
        public Task<RequestResult> Delete(string PageDbGuid);
    }
}

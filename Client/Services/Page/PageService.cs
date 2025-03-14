using YGate.Entities;
using YGate.Entities.BasedModel;
using YGate.Entities.ViewModels;

namespace YGate.Client.Services.Page
{
    public class PageService : IPageService
    {

        HttpClientService httpClientService;
        public PageService(HttpClientService httpClientService)
        {
            this.httpClientService = httpClientService;
        }

        public async Task<RequestResult> Add(DynamicPage page)
        {
            var obj = await httpClientService.GetPostAsync<RequestResult>(page, "api/Page/AddPage");
            return obj;
        }

        public async Task<RequestResult> Delete(string PageDbGuid)
        {
            var obj = await httpClientService.GetPostAsync<RequestResult>(PageDbGuid, "api/Page/Delete");
            return obj;
        }

        public async Task<RequestResult> GetAll()
        {
            var obj = await httpClientService.GetPostAsync<RequestResult>("", "api/Page/GetAll");
            return obj;
        }

        public async Task<RequestResult> GetAsync(DynamicPageDynamicParameter page)
        {
            var obj = await httpClientService.GetPostAsync<RequestResult>(page, "api/Page/GetPage");
            return obj;
        }
    }
}

using System;
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

        public async Task<RequestResult> GetPageForGuid(string guid)
        {
            var obj = await httpClientService.GetPostAsync<RequestResult>(guid, "api/Page/GetPageForGuid");
            return obj;
        }

        public async Task<RequestResult> GetPageButPPAsync(DynamicPageParameter page)
        {
            var obj = await httpClientService.GetPostAsync<RequestResult>(page, "api/Page/GetPageButParameterPool");
            return obj;
        }

        public async Task<RequestResult> PageObjectUpdate(DynamicPage page) {
            var obj = await httpClientService.GetPostAsync<RequestResult>(page, "api/Page/UpdatePageObject");
            return obj;
        }

        public async Task<RequestResult> GetMyParametersPageUris()
        {
            var obj = await httpClientService.GetPostAsync<RequestResult>("", "api/Page/MyParametersPages");
            return obj;
        }

        public async Task<RequestResult> GetPageForName(string name)
        {
            var obj = await httpClientService.GetPostAsync<RequestResult>(name, "api/Page/GetPageForName");
            return obj;
        }
    }
}

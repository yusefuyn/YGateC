using System;
using YGate.Entities;
using YGate.Entities.BasedModel;

namespace YGate.Client.Services.Property
{
    public class PropertyService : IPropertyService
    {
        HttpClientService httpClientService;
        public PropertyService(HttpClientService httpClientService)
        {
            this.httpClientService = httpClientService;
        }
        public async Task<RequestResult> GetAllPropertyViewModel() 
        {
            var res = await httpClientService.GetPostAsync<RequestResult>("", "api/Property/GetAllPropertyViewModel");
            return res;
        }

        public async Task<RequestResult> AddGroup(PropertyGroup model) {
            var res = await httpClientService.GetPostAsync<RequestResult>(model, "api/Property/AddGroup");
            return res;
        }

        public async Task<RequestResult> GetGroup(string guid)
        {
            var res = await httpClientService.GetPostAsync<RequestResult>(guid, "api/Property/GetGroup");
            return res;
        }

        public async Task<RequestResult> AddValues(List<PropertyGroupValue> values)
        {
            var res = await httpClientService.GetPostAsync<RequestResult>(values, "api/Property/AddValues");
            return res;
        }
    }
}

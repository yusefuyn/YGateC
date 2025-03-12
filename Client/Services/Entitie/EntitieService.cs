using Microsoft.AspNetCore.Components;
using YGate.Entities;
using YGate.Entities.BasedModel;
using YGate.Entities.ViewModels;

namespace YGate.Client.Services.Entitie
{
    public class EntitieService : IEntitieService
    {
        HttpClientService httpClientService;
        public EntitieService(HttpClientService httpClientService)
        {
            this.httpClientService = httpClientService;
        }

        public async Task<RequestResult> AddEntitie(EntitieViewModel entitie, List<EntitieViewModel> subEntitie)
        {
            EntitieRequestViewModel parameters = new EntitieRequestViewModel() { MainModel = entitie, SubModel = subEntitie };
            var res = await httpClientService.GetPostAsync<RequestResult>(parameters, "api/Entitie/AddEntitite");
            return res;
        }

        public async Task<RequestResult> GetAllEntitie()
        {
            var res = await httpClientService.GetPostAsync<RequestResult>(null, "api/Entitie/GetAllEntitieViewModel");
            return res;
        }

        public async Task<RequestResult> GetAllEntitieButCategoryGuid(string CategoryGuid)
        {
            var res = await httpClientService.GetPostAsync<RequestResult>(CategoryGuid, "api/Entitie/GetAllEntitieButCategoryGuid");
            return res;
        }

        public async Task<RequestResult> GetAllMyEntitie(string ownerGuid)
        {
            var res = await httpClientService.GetPostAsync<RequestResult>(ownerGuid, "api/Entitie/GetAllMyEntitieViewModel");
            return res;
        }


        public async Task<RequestResult> GetEntitieViewModel(string EntitieGuid)
        {
            var res = await httpClientService.GetPostAsync<RequestResult>(EntitieGuid, "api/Entitie/GetEntitieViewModel");
            return res;
        }

        public async Task<RequestResult> DeleteEntity(string guid)
        {
            var res = await httpClientService.GetPostAsync<RequestResult>(guid, "api/Entitie/Delete");
            return res;
        }

        public async Task<RequestResult> GetAllEntitieButCategoryId(string Id)
        {
            var res = await httpClientService.GetPostAsync<RequestResult>(Id, "api/Entitie/GetAllEntitieButCategoryId");
            return res;
        }

        public async Task<RequestResult> Transfer(string VictimGuid, string ObjectGuid, string YourPassword)
        {
            return await httpClientService.GetPostAsync<RequestResult>(new { VictimGuid = VictimGuid, ObjectGuid = ObjectGuid, Password = YourPassword }, "api/Entitie/Transfer");
        }
    }
}

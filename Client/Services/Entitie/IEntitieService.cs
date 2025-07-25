﻿using Microsoft.AspNetCore.Components;
using YGate.Entities;
using YGate.Entities.BasedModel;
using YGate.Entities.ViewModels;
using static YGate.Client.Services.Entitie.EntitieService;

namespace YGate.Client.Services.Entitie
{
    public interface IEntitieService
    {

        public Task<RequestResult> AddEntitie(EntitieViewModel entitie, List<EntitieViewModel> subEntitie);
        public Task<RequestResult> GetAllEntitie();
        public Task<RequestResult> GetAllMyEntitie(string OwnerGuid);
        public Task<RequestResult> GetAllEntitieButCategoryGuid(string CategoryGuid);

        public Task<RequestResult> GetEntitieViewModel(string EntitieGuid);
        public Task<RequestResult> DeleteEntity(string guid);

        public Task<RequestResult> GetAllEntitieButCategoryId(string Id);

        public Task<RequestResult> Transfer(string VictimGuid, string ObjectGuid, string YourPassword);
    }
}

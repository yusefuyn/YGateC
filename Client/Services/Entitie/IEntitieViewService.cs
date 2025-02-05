using Microsoft.AspNetCore.Components;
using static YGate.Client.Services.Entitie.EntitieViewService;
using YGate.Entities.ViewModels;

namespace YGate.Client.Services.Entitie
{
    public interface IEntitieViewService
    {



        public MarkupString GetListView(EntitieViewModel entitieViewModel);
        public MarkupString GetDataView(EntitieViewModel entitieViewModel);
        public MarkupString GetChildView(EntitieViewModel entitieViewModel);
    }
}

using Microsoft.AspNetCore.Components;
using static YGate.Client.Services.Entitie.EntitieViewService;
using YGate.Entities.ViewModels;

namespace YGate.Client.Services.Entitie
{
    public interface IEntitieViewService
    {
        public MarkupString GetListView(EntitieViewModel entitieViewModel);
        public MarkupString GetListPage(EntitieViewModel PageViewModel, List<EntitieViewModel> ListEntitieViewModel);
        public MarkupString GetDataView(EntitieViewModel entitieViewModel);
        public MarkupString GetChildView(EntitieViewModel entitieViewModel);
        public MarkupString GetCreateView(CategoryViewModel categoryTemplateViewModel);
    }
}

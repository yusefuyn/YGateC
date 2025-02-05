using Microsoft.AspNetCore.Components;
using static YGate.Client.Services.Entitie.EntitieViewService;
using YGate.Entities.ViewModels;

namespace YGate.Client.Services.Entitie
{
    public interface IEntitieViewService
    {
        public MarkupString CategoryHtmlTemplateAddValues(EntitieViewModel entitie, TemplateEnum TempType);
        public MarkupString CategoryHtmlTemplateAddValues(EntitieViewModel entitie, string temp, TemplateEnum TempType);
    }
}

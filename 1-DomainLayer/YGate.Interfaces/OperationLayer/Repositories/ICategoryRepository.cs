
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Interfaces.DomainLayer;

namespace YGate.Interfaces.OperationLayer.Repositories
{
    public interface ICategoryRepository
    {
        Task<IRequestResult> GetAllButParentCategory();
        Task<IRequestResult> AddCategoryRoles(IRequestParameter parameter);
        Task<IRequestResult> AddNewCategory(IRequestParameter parameter);
        Task<IRequestResult> GetAll();
        Task<IRequestResult> RemoveCategoryRole(IRequestParameter parameter);
        Task<IRequestResult> GetAllButTreeView(IRequestParameter parameter = null);
        Task<IRequestResult> GetCategoryRoles(IRequestParameter parameter);
        Task<IRequestResult> DeleteCategory(IRequestParameter parameter);
        Task<IRequestResult> GetCategoryToId(IRequestParameter parameter);
        Task<IRequestResult> GetCategoryToGuid(IRequestParameter parameter);
        Task<IRequestResult> AddTemplates(IRequestParameter parameter);
        Task<IRequestResult> DeleteTemplate(IRequestParameter parameter);
        Task<IRequestResult> GetAllSubCategoryToParentCategoryGuid(IRequestParameter parameter);
        Task<IRequestResult> AddHtmlTemplate(IRequestParameter parameter);
        Task<IRequestResult> GetHtmlTemplateBCI(IRequestParameter parameter);
        Task<IRequestResult> GetHtmlTemplateBCDBG(IRequestParameter parameter);
        Task<IRequestResult> ModifyHtmlTemplate(IRequestParameter parameter);
        Task<IRequestResult> GetSidebarCategories(IRequestParameter parameters = null);
    }
}

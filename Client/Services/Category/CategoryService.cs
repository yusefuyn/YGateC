using YGate.Client.Services.Profile;
using YGate.Entities;
using YGate.Entities.BasedModel;
using YGate.Entities.ViewModels;

namespace YGate.Client.Services.Category
{
    public class CategoryService : ICategoryService
    {
        HttpClientService httpClientService;
        CookieService cookieService;
        IProfileService profileService;
        public CategoryService(IProfileService profileService, HttpClientService httpClientService, CookieService cookieService)
        {
            this.httpClientService = httpClientService;
            this.cookieService = cookieService;
            this.profileService = profileService;
        }

        public async Task<RequestResult> AddCategory(YGate.Entities.BasedModel.Category category)
        {
            category.CreatorGuid = await profileService.GetPropertiesValue("Guid"); 
            var res = await httpClientService.GetPostAsync<RequestResult>(category, "api/Category/AddNewCategory");
            return res;
        }

        public async Task Delete(string Guid)
        { 
            await httpClientService.GetPostAsync<RequestResult>(Guid, "api/Category/DeleteCategory");
        }



        public async Task<RequestResult> GetAll()
        {
            var res = await httpClientService.GetPostAsync<RequestResult>(null, "api/Category/GetAll");
            return res;
        }

        public async Task<RequestResult> GetAllButParentCategory()
        {
            var res = await httpClientService.GetPostAsync<RequestResult>(null, "api/Category/GetAllButParentCategory");
            return res;
        }

        public async Task<RequestResult> GetAllSubCategoryToParentCategoryGuid(string DbGuid)
        {
            var res = await httpClientService.GetPostAsync<RequestResult>(DbGuid, "api/Category/GetAllSubCategoryToParentCategoryGuid");
            return res;
        }


        public async Task<RequestResult> GetAllButTreeView()
        {
            var res = await httpClientService.GetPostAsync<RequestResult>(null, "api/Category/GetAllButTreeView");
            return res;
        }

        public async Task<RequestResult> GetCategory(int Id)
        {
            var res = await httpClientService.GetPostAsync<RequestResult>(Id, "api/Category/GetCategoryToId");
            return res;
        }


        public async Task<RequestResult> GetCategory(string guid)
        {
            var res = await httpClientService.GetPostAsync<RequestResult>(guid, "api/Category/GetCategoryToGuid");
            return res;
        }

        public async Task<RequestResult> AddTemplates(List<CategoryTemplateViewModel> viewmodels)
        {
            var res = await httpClientService.GetPostAsync<RequestResult>(viewmodels, "api/Category/AddTemplates");
            return res;
        }

        public async Task<RequestResult> DeleteTemplate(CategoryTemplateViewModel viewmodel)
        {
            var res = await httpClientService.GetPostAsync<RequestResult>(viewmodel, "api/Category/DeleteTemplate");
            return res;
        }

        public async Task<RequestResult> AddHtmlTemplate(CategoryHtmlTemplate categoryHtmlTemplate)
        {
            var res = await httpClientService.GetPostAsync<RequestResult>(categoryHtmlTemplate, "api/Category/AddHtmlTemplate");
            return res;
        }

        public async Task<RequestResult> GetHtmlTemplateButCategoryId(int categoryId)
        {
            var res = await httpClientService.GetPostAsync<RequestResult>(categoryId, "api/Category/GetHtmlTemplateBCI");
            return res;
        }

        
        public async Task<RequestResult> GetHtmlTemplateButCategoryDBGuid(string CategoryDBGuid)
        {
            var res = await httpClientService.GetPostAsync<RequestResult>(CategoryDBGuid, "api/Category/GetHtmlTemplateBCDBG");
            return res;
        }
        public async Task<RequestResult> ModifyHtmlTemplate(CategoryHtmlTemplate categoryHtmlTemplate)
        {
            var res = await httpClientService.GetPostAsync<RequestResult>(categoryHtmlTemplate, "api/Category/ModifyHtmlTemplate");
            return res;
        }

        public async Task<RequestResult> GetSidebarCategories()
        {
            var res = await httpClientService.GetPostAsync<RequestResult>("", "api/Category/GetSidebarCategories");
            return res;
        }

        public async Task<RequestResult> GetCategoryRoles(string categoryguid)
        {
            var res = await httpClientService.GetPostAsync<RequestResult>(categoryguid, "api/Category/GetCategoryRoles");
            return res;
        }

        public async Task<RequestResult> AddCategoryRoles(CategoryRoles parameters) {
            var res = await httpClientService.GetPostAsync<RequestResult>(parameters, "api/Category/AddCategoryRoles");
            return res;
        }


        public async Task<RequestResult> RemoveCategoryRole(dynamic guid) {
            var res = await httpClientService.GetPostAsync<RequestResult>(guid, "api/Category/RemoveCategoryRole");
            return res;
        }

    }
}

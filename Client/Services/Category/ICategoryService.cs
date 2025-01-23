using YGate.Entities;
using YGate.Entities.BasedModel;
using YGate.Entities.ViewModels;
using Category = YGate.Entities.BasedModel.Category;

namespace YGate.Client.Services.Category
{
    public interface ICategoryService
    {
        Task<RequestResult> AddCategory(YGate.Entities.BasedModel.Category category);
        public Task Delete(string Guid);
        Task<RequestResult> GetAll(); // Tüm kategorileri getirir
        Task<RequestResult> GetAllButParentCategory(); // Alt kategorileri getirir
        Task<RequestResult> GetAllSubCategoryToParentCategoryGuid(string DbGuid); // Seçilen Kategorinin Alt Kategorilerini getirir
        Task<RequestResult> GetAllButTreeView();
        Task<RequestResult> GetCategory(int Id);
        Task<RequestResult> GetCategory(string guid);

        Task<RequestResult> AddTemplates(List<CategoryTemplateViewModel> viewmodels);


        // Var olan bir Template'i silmek için
        Task<RequestResult> DeleteTemplate(CategoryTemplateViewModel viewmodel);


        Task<RequestResult> GetHtmlTemplateButCategoryId(int categoryId);
        Task<RequestResult> GetHtmlTemplateButCategoryDBGuid(string categoryDbGuid);

        Task<RequestResult> AddHtmlTemplate(CategoryHtmlTemplate categoryHtmlTemplate);
        Task<RequestResult> ModifyHtmlTemplate(CategoryHtmlTemplate categoryHtmlTemplate);
        Task<RequestResult> GetSidebarCategories();
        Task<RequestResult> GetCategoryRoles(string categoryguid);

        Task<RequestResult> AddCategoryRoles(CategoryRoles parameters);

        Task<RequestResult> RemoveCategoryRole(dynamic guid);




    }
}

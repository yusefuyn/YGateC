using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Entities.BasedModel;
using YGate.Entities;
using YGate.Interfaces.DomainLayer;
using YGate.Interfaces.OperationLayer;
using YGate.Server.Facades;
using YGate.Entities.ViewModels;

namespace YGate.BusinessLayer.EFCore.Concretes
{
    public class CategoryRepository : ICategoryRepository
    {
        Operations operations;
        IBaseFacades baseFacades;

        public CategoryRepository(IBaseFacades baseFacades, Operations operations)
        {
            this.baseFacades = baseFacades;
            this.operations = operations;
        }

        public async Task<IRequestResult> AddCategoryRoles(IRequestParameter parameter)
        {
            CategoryRoles ob = parameter.Parameters as CategoryRoles;


            RequestResult returned = new($"AddCategoryRoles {ob.RoleGuid} {ob.CategoryGuid} {ob.CreatorGuid}");
            returned.To = EnumTo.Server;
            returned.Result = EnumRequestResult.Success;
            ob.DBGuid = YGate.String.Operations.GuidGen.Generate("CategoryRole");
            operations.Context.CategoryRoles.Add(ob);
            operations.Context.SaveChanges();
            returned.Object = operations.GetCategoryRole(ob.CategoryGuid);


            return returned;
        }

        public async Task<IRequestResult> AddHtmlTemplate(IRequestParameter parameter)
        {
            CategoryHtmlTemplate viewModel = parameter.Parameters as CategoryHtmlTemplate;

            RequestResult returned = new($"AddHtmlTemplate {viewModel.DBGuid}");
            returned.Result = EnumRequestResult.Success;
            operations.Context.CategoryHtmlTemplates.Add(viewModel);
            operations.Context.SaveChanges();
            return returned;
        }

        public async Task<IRequestResult> AddNewCategory(IRequestParameter parameter)
        {
            RequestResult returned = new("AddNewCategory");
            Category category = parameter as Category;
            category.DBGuid = YGate.String.Operations.GuidGen.Generate("Category");
            if (string.IsNullOrEmpty(category.Icon))
                category.Icon = "fa fa-star";
            operations.Context.Categories.Add(category);
            operations.Context.SaveChanges();
            returned.Result = EnumRequestResult.Success;
            returned.Object = category;
            return returned;
        }

        public async Task<IRequestResult> GetAll()
        {
            RequestResult returned = new("GetAllCategory");
            returned.Result = EnumRequestResult.Success;
            returned.Object = operations.Context.Categories.ToList();
            return returned;
        }


        private List<CategorySidebarViewModel> ConvertToSidebarViewModel(List<CategoryViewModel> categoryViewModels)
        {
            List<CategorySidebarViewModel> returnedList = categoryViewModels.Select(cv => new CategorySidebarViewModel
            {
                CategoryRole = operations.GetCategoryRole(cv.DBGuid).AddedRoles.Select(xd => xd.Name).ToList(),
                CategoryGuid = cv.DBGuid,
                CategoryName = cv.Name,
                //CategoryViewLink = $"/GetEntitiesButCategoryGuid/{cv.DBGuid}", // Seoya uygun değildi.
                CategoryViewLink = string.IsNullOrEmpty(cv.Address) == true ? $"/GetCategoryEntities/{cv.Name}-{cv.Id.ToString().PadLeft(8, '0')}" : cv.Address, // Seoya uygun yapıldı.
                CategorySymbol = string.IsNullOrEmpty(cv.Icon) == true ? "fa fa-square" : cv.Icon,
                SubCategories = ConvertToSidebarViewModel(cv.ChildCategories)
            }).Select(cvm =>
            {
                cvm.CategoryRole.Add("Administrator");
                return cvm;
            })
            .ToList();

            returnedList.Add(new CategorySidebarViewModel() { CategoryName = "Chat", CategorySymbol = "fa-message", CategoryRole = new() { "All" }, CategoryViewLink = "Chat" });

            return returnedList;
        }

        private CategoryViewModel GetCategoryViewModel(string guid)
        {
            CategoryViewModel categoryViewModel = operations.GetCategoryButViewModel(xd => xd.DBGuid == guid);

            return categoryViewModel;
        }


        private CategoryHtmlTemplate GetHtmlTemplateBCDBGp(string dbguid)
        {
            CategoryHtmlTemplate returnedObj = operations.Context.CategoryHtmlTemplates.FirstOrDefault(xd => xd.CategoryGuid == dbguid);

            if (returnedObj == null)
            {
                returnedObj = new();
                returnedObj.DBGuid = YGate.String.Operations.GuidGen.Generate("HtmlTemplate");
                returnedObj.CategoryGuid = dbguid;
            }

            returnedObj.Category = GetCategoryViewModel(dbguid);
            return returnedObj;
        }

        public async Task<IRequestResult> GetAllButParentCategory()
        {
            RequestResult returned = new("GetAllButParentCategory");
            returned.Result = EnumRequestResult.Success;
            var res = operations.Context.Categories.Where(xd => xd.IsActive == true && xd.ParentCategoryId == null).ToList();
            returned.Object = res;
            return returned;
        }

        public async Task<IRequestResult> AddTemplates(IRequestParameter parameter)
        {
            RequestResult returned = new($"AddTemplates");
            List<CategoryTemplateViewModel> viewmodels = parameter.Parameters as List<CategoryTemplateViewModel>;

            returned.Result = EnumRequestResult.Success;
            List<CategoryTemplate> Templates = YGate.Mapping.Operations.ConvertToList<CategoryTemplate>(viewmodels);
            List<CategoryTemplateValue> Values = new();
            viewmodels.ForEach(xd =>
            {
                if (xd.ValueType == PropertyValueType.Unit ||
                xd.ValueType == PropertyValueType.ItemGroup)
                    Values.AddRange(xd.categoryTemplateValues);
            });

            operations.Context.CategoryTemplates.AddRange(Templates);
            operations.Context.CategoryTemplateValues.AddRange(Values);
            try
            {
                operations.Context.SaveChanges();

            }
            catch (Exception ex)
            {

            }

            return returned;
        }

        public async Task<IRequestResult> DeleteCategory(IRequestParameter parameter)
        {
            RequestResult returned = new("DeleteCategory");
            returned.Result = EnumRequestResult.Success;

            Category resobj = operations.Context.Categories.FirstOrDefault(xd => xd.DBGuid == parameter.Parameters.ToString());
            if (resobj == null)
            {
                returned.Result = EnumRequestResult.Error;
                return returned;
            }
            using (var transaction = operations.Context.Database.BeginTransaction())
            {
                try
                {
                    string categoryGuid = resobj.DBGuid;
                    List<CategoryHtmlTemplate> categoryHtmlTemplatesToRemove = operations.Context.CategoryHtmlTemplates.Where(xd => xd.CategoryGuid == categoryGuid).ToList();
                    List<Entitie> entitiesToRemove = operations.Context.Entities.Where(xd => xd.CategoryDBGuid == categoryGuid).ToList();
                    List<Category> subCategories = operations.Context.Categories.Where(xd => xd.ParentCategoryId == resobj.Id).ToList();
                    List<Category> allCategoriesToRemove = new List<Category>(subCategories);
                    List<CategoryHtmlTemplate> allCategoryHtmlTemplatesToRemove = new List<CategoryHtmlTemplate>(categoryHtmlTemplatesToRemove);
                    List<Entitie> allEntitiesToRemove = new List<Entitie>(entitiesToRemove);
                    List<CategoryRoles> roles = operations.Context.CategoryRoles.Where(xd => xd.CategoryGuid == categoryGuid).ToList();
                    subCategories.Add(resobj);

                    foreach (var subCategory in subCategories)
                    {
                        allCategoryHtmlTemplatesToRemove.AddRange(operations.Context.CategoryHtmlTemplates.Where(xd => xd.CategoryGuid == subCategory.DBGuid).ToList());
                        allEntitiesToRemove.AddRange(operations.Context.Entities.Where(xd => xd.CategoryDBGuid == subCategory.DBGuid).ToList());
                        List<Category> subSubCategories = operations.Context.Categories.Where(xd => xd.ParentCategoryId == subCategory.Id).ToList();
                        allCategoriesToRemove.AddRange(subSubCategories);
                    }

                    foreach (var entity in allEntitiesToRemove)
                    {
                        var entityPropertyValues = operations.Context.EntitiePropertyValues.Where(xd => xd.EntitieDbGuid == entity.DBGuid).ToList();
                        if (entityPropertyValues.Any())
                        {
                            operations.Context.EntitiePropertyValues.RemoveRange(entityPropertyValues);
                        }
                    }


                    if (subCategories.Any())
                        operations.Context.Categories.RemoveRange(subCategories);
                    if (allCategoryHtmlTemplatesToRemove.Any())
                        operations.Context.CategoryHtmlTemplates.RemoveRange(allCategoryHtmlTemplatesToRemove);
                    if (allEntitiesToRemove.Any())
                        operations.Context.Entities.RemoveRange(allEntitiesToRemove);
                    if (allCategoriesToRemove.Any())
                        operations.Context.Categories.RemoveRange(allCategoriesToRemove);
                    if (roles.Any())
                        operations.Context.CategoryRoles.RemoveRange(roles);


                    operations.Context.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }


            return returned;
        }

        public async Task<IRequestResult> DeleteTemplate(IRequestParameter parameter)
        {
            CategoryTemplateViewModel viewModel = parameter.Parameters as CategoryTemplateViewModel;

            RequestResult returned = new($"DeleteTemplate {viewModel.DBGuid}");
            returned.Result = EnumRequestResult.Success;
            operations.DeleteCategoryTemplate(viewModel);
            return returned;
        }



        public async Task<IRequestResult> GetAllButTreeView(IRequestParameter parameter = null)
        {
            RequestResult returned = new("GetAllCategory");
            returned.Result = EnumRequestResult.Success;
            var resobj = await operations.GetCategoryTreeAsync();
            returned.Object = resobj;
            return returned;
        }

        public async Task<IRequestResult> GetAllSubCategoryToParentCategoryGuid(IRequestParameter parameter)
        {
            RequestResult returned = new($"Get Sub Category {parameter.Parameters.ToString()}");
            returned.Result = EnumRequestResult.Success;
            CategoryViewModel category = operations.GetCategoryButViewModel(xd => xd.DBGuid == parameter.Parameters.ToString());
            operations.LoadFirstChildCategories(category);
            returned.Object = category.ChildCategories;
            return returned;
        }

        public async Task<IRequestResult> GetCategoryRoles(IRequestParameter parameter)
        {
            RequestResult returned = new($"Get Category Roles By CategoryGuid {parameter.Parameters.ToString()}");
            returned.To = EnumTo.Server;
            returned.Result = EnumRequestResult.Success;
            returned.Object = operations.GetCategoryRole(parameter.Parameters.ToString());
            return returned;
        }

        public async Task<IRequestResult> GetCategoryToGuid(IRequestParameter parameter)
        {
            RequestResult returned = new($"GetCategory {parameter.Parameters.ToString()}");
            returned.Result = EnumRequestResult.Success;
            returned.Object = GetCategoryViewModel(parameter.Parameters.ToString());
            return returned;
        }

        public async Task<IRequestResult> GetCategoryToId(IRequestParameter parameter)
        {
            RequestResult returned = new($"GetCategory {parameter.Parameters.ToString()}");
            returned.Result = EnumRequestResult.Success;
            CategoryViewModel resobj = operations.GetCategoryButViewModel(xd => xd.Id == Convert.ToInt32(parameter.Parameters.ToString()));
            returned.Object = resobj;
            return returned;
        }

        public async Task<IRequestResult> GetHtmlTemplateBCDBG(IRequestParameter parameter)
        {
            string dbguid = parameter.Parameters.ToString();
            RequestResult returned = new($"GetHtmlTemplate DBGuid {dbguid}");
            returned.Result = EnumRequestResult.Success;

            returned.Object = GetHtmlTemplateBCDBGp(dbguid);
            return returned;
        }

        public async Task<IRequestResult> GetHtmlTemplateBCI(IRequestParameter parameter)
        {
            int Id = Convert.ToInt32(parameter.Parameters.ToString());
            RequestResult returned = new($"GetHtmlTemplate ID {Id}");
            returned.Result = EnumRequestResult.Success;
            Category category = operations.Context.Categories.FirstOrDefault(xd => xd.Id == Id);
            returned.Object = GetHtmlTemplateBCDBGp(category.DBGuid);
            return returned;
        }

        public async Task<IRequestResult> GetSidebarCategories(IRequestParameter parameters = null)
        {
            RequestResult returned = new($"");
            returned.Result = EnumRequestResult.Success;

            List<CategoryViewModel> obs = await operations.GetCategoryTreeAsync();
            returned.Object = ConvertToSidebarViewModel(obs);

            return returned;
        }

        public async Task<IRequestResult> ModifyHtmlTemplate(IRequestParameter parameter)
        {
            CategoryHtmlTemplate template = parameter.Parameters as CategoryHtmlTemplate;
            RequestResult returned = new($"ModifyHtmlTemplate {template.Id}");
            returned.Result = EnumRequestResult.Success;
            var obj = operations.Context.CategoryHtmlTemplates.FirstOrDefault(xd => xd.DBGuid == template.DBGuid);
            obj.Template = template.Template;
            operations.Context.SaveChanges();
            return returned;
        }

        public async Task<IRequestResult> RemoveCategoryRole(IRequestParameter parameter)
        {
            dynamic param = parameter.Parameters as dynamic;
            string CategoryGuid = param["CategoryGuid"];
            string RoleGuid = param["RoleGuid"];

            RequestResult returned = new($"Remove CategoryRole {parameter.Parameters.ToString()}");
            returned.Result = EnumRequestResult.Success;

            var ob = operations.Context.CategoryRoles.FirstOrDefault(xd => xd.CategoryGuid == CategoryGuid && xd.RoleGuid == RoleGuid);
            if (ob != null)
                operations.Context.CategoryRoles.Remove(ob);
            operations.Context.SaveChanges();


            returned.Object = operations.GetCategoryRole(ob.CategoryGuid);
            return returned;

        }


    }
}

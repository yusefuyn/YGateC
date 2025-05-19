using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using YGate.BusinessLayer.EFCore;
using YGate.Client.Pages.Category;
using YGate.Client.Shared.Components;
using YGate.Entities;
using YGate.Entities.BasedModel;
using YGate.Entities.ResultModel;
using YGate.Entities.ViewModels;
using YGate.Entities.ViewModels.Chat;
using YGate.Interfaces.DomainLayer;
using YGate.Interfaces.OperationLayer.Repositories;
using YGate.Mapping;
using YGate.Server.Facades;
namespace YGate.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    public class CategoryController : Controller
    {

        IHubContext<MyHub> hub;
        ICategoryRepository categoryRepository;

        public CategoryController(IHubContext<MyHub> hub, ICategoryRepository categoryRepository)
        {

            this.categoryRepository = categoryRepository;
            this.hub = hub;

        }
        [HttpPost]
        public async Task<IRequestResult> AddNewCategory([FromBody] RequestParameter parameter)
        {
            await hub.Clients.Group("SideBar").SendAsync("RefreshSideBar");
            return await categoryRepository.AddNewCategory(parameter);
        }

        [HttpPost]
        public async Task<IRequestResult> GetAll()
        {
            return await categoryRepository.GetAll();
        }

        [HttpPost]
        public async Task<IRequestResult> AddCategoryRoles([FromBody] RequestParameter parameter)
        {
            await hub.Clients.Group("SideBar").SendAsync("RefreshSideBar");
            return await categoryRepository.AddCategoryRoles(parameter);
        }

        [HttpPost]
        public async Task<IRequestResult> GetAllButParentCategory()
        {
            return await categoryRepository.GetAllButParentCategory();
        }

        [HttpPost]
        public async Task<IRequestResult> RemoveCategoryRole([FromBody] RequestParameter parameter)
        {
            await hub.Clients.Group("SideBar").SendAsync("RefreshSideBar");
            return await categoryRepository.RemoveCategoryRole(parameter);
        }

        [HttpPost]
        public async Task<IRequestResult> GetAllButTreeView([FromBody] RequestParameter parameter = null)
        {
            return await categoryRepository.GetAllButTreeView(parameter);
        }

        [HttpPost]
        public async Task<IRequestResult> GetCategoryRoles([FromBody] RequestParameter parameter)
        {
            return await categoryRepository.GetCategoryRoles(parameter);
        }

        [HttpPost]
        public async Task<IRequestResult> DeleteCategory([FromBody] RequestParameter parameter)
        {
            return await categoryRepository.DeleteCategory(parameter);
        }

        [HttpPost]
        public async Task<IRequestResult> GetCategoryToId([FromBody] RequestParameter parameter)
        {
            return await categoryRepository.GetCategoryToId(parameter);
        }

        [HttpPost]
        public async Task<IRequestResult> GetCategoryToGuid([FromBody] RequestParameter parameter)
        {
            return await categoryRepository.GetCategoryToGuid(parameter);
        }

        [HttpPost]
        public async Task<IRequestResult> AddTemplates([FromBody] RequestParameter parameter)
        {
            return await categoryRepository.AddTemplates(parameter);
        }


        [HttpPost]
        public async Task<IRequestResult> DeleteTemplate([FromBody] RequestParameter parameter)
        {
            await hub.Clients.Group("SideBar").SendAsync("RefreshSideBar");
            return await categoryRepository.DeleteTemplate(parameter);
        }
        [HttpPost]
        public async Task<IRequestResult> GetAllSubCategoryToParentCategoryGuid([FromBody] RequestParameter parameter)
        {
            return await categoryRepository.GetAllSubCategoryToParentCategoryGuid(parameter);
        }

        [HttpPost]
        public async Task<IRequestResult> AddHtmlTemplate([FromBody] RequestParameter parameter)
        {
            return await categoryRepository.AddHtmlTemplate(parameter);
        }


        [HttpPost]
        public async Task<IRequestResult> GetHtmlTemplateBCI([FromBody] RequestParameter parameter)
        {
            return await categoryRepository.GetHtmlTemplateBCI(parameter);
        }



        [HttpPost]
        public async Task<IRequestResult> GetHtmlTemplateBCDBG([FromBody] RequestParameter parameter)
        {
            return await categoryRepository.GetHtmlTemplateBCDBG(parameter);
        }

        [HttpPost]
        public async Task<IRequestResult> ModifyHtmlTemplate([FromBody] RequestParameter parameter)
        {
            return await categoryRepository.ModifyHtmlTemplate(parameter);
        }

        [HttpPost]
        public async Task<IRequestResult> GetSidebarCategories([FromBody] RequestParameter parameters = null)
        {
            return await categoryRepository.GetSidebarCategories(parameters);
        }


    }
}

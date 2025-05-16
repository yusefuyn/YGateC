using Microsoft.AspNetCore.Mvc;
using YGate.BusinessLayer.EFCore;
using YGate.Entities;
using YGate.Entities.BasedModel;
using YGate.Interfaces.DomainLayer;
using YGate.Interfaces.OperationLayer.Repositories;
using YGate.Server.Attributes;
using YGate.Server.Facades;

namespace YGate.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PageController : Controller
    {
        IPageRepository pageRepository;

        public PageController(IPageRepository pageRepository)
        {
            this.pageRepository = pageRepository;
        }

        [HttpPost]
        public async Task<IRequestResult> GetPage([FromBody] RequestParameter parameter)
        {
            return await pageRepository.GetPage(parameter);
        }

        [HttpPost]
        [GetAuthorizeToken]
        public async Task<IRequestResult> MyParametersPages([FromBody] RequestParameter parameter)
        {
            return await pageRepository.MyParametersPages(parameter);
        }


        [HttpPost]
        [GetAuthorizeToken]
        public async Task<IRequestResult> SavePageParameters([FromBody] RequestParameter parameter)
        {
            return await pageRepository.SavePageParameters(parameter);
        }


        [HttpPost]
        [GetAuthorizeToken]
        public async Task<IRequestResult> GetPageForGuid([FromBody] RequestParameter parameter)
        {
            return await pageRepository.GetPageForGuid(parameter);
        }


        [HttpPost]
        public async Task<IRequestResult> GetPageForName([FromBody] RequestParameter parameter)
        {
            return await pageRepository.GetPageForName(parameter);
        }

        [HttpPost]
        [GetAuthorizeToken]
        public async Task<IRequestResult> UpdatePageObject([FromBody] RequestParameter parameter)
        {
            return await pageRepository.UpdatePageObject(parameter);


        }

        [HttpPost]
        public async Task<IRequestResult> GetPageButParameterPool([FromBody] RequestParameter parameter)
        {
            return await pageRepository.GetPageButParameterPool(parameter);

        }

        [HttpPost]
        [GetAuthorizeToken]
        public async Task<IRequestResult> AddPage([FromBody] RequestParameter parameter)
        {
            return await pageRepository.AddPage(parameter);

        }

        [HttpPost]
        [Authorized("Administrator")]
        public async Task<IRequestResult> GetAll([FromBody] RequestParameter parameter)
        {
            return await pageRepository.GetAll(parameter);

        }

        [HttpPost]
        [Authorized("Administrator")]
        public async Task<IRequestResult> Delete([FromBody] RequestParameter parameter)
        {
            return await pageRepository.Delete(parameter);

        }
    }
}

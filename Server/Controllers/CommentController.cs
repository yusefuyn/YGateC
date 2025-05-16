using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Data.Entity;
using YGate.BusinessLayer.EFCore;
using YGate.Entities;
using YGate.Entities.BasedModel;
using YGate.Interfaces.DomainLayer;
using YGate.Interfaces.OperationLayer.Repositories;
using YGate.Server.Attributes;
using YGate.Server.Facades;

namespace YGate.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CommentController : Controller
    {
        
        IHubContext<MyHub> hub;
        ICommentRepository commentRepository;
        public CommentController(IHubContext<MyHub> hub, ICommentRepository commentRepository)
        {
            this.commentRepository = commentRepository;
            this.hub = hub;
        }

        [HttpPost]
        public async Task<IRequestResult> Gets([FromBody] RequestParameter parameter)
        {
            return await commentRepository.Gets(parameter);
        }


        [HttpPost]
        [Authorized("Administrator")]
        public async Task<IRequestResult> GetAll([FromBody] RequestParameter parameter)
        {
           return await commentRepository.GetAll(parameter);
        }

        [HttpPost]
        [Authorized("Administrator")]
        public async Task<IRequestResult> Delete([FromBody] RequestParameter parameter)
        {
            return await commentRepository.Delete(parameter);   
        }

        [HttpPost]
        [GetAuthorizeToken]
        public async Task<IRequestResult> Add([FromBody] RequestParameter parameter)
        {
            return await commentRepository.Add(parameter);
        }
    }
}

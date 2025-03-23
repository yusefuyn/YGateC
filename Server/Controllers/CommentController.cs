using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Data.Entity;
using YGate.BusinessLayer.EFCore;
using YGate.Entities;
using YGate.Entities.BasedModel;
using YGate.Server.Attributes;

namespace YGate.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CommentController : Controller
    {
        private readonly Operations operations;
        IHubContext<MyHub> hub;
        public CommentController(IHubContext<MyHub> hub, Operations operations)
        {
            this.operations = operations;
            this.hub = hub;
        }

        [HttpPost]
        public async Task<string> Gets([FromBody] RequestParameter parameter)
        {
            RequestResult request = new("Gets Comment");
            request.Result = EnumRequestResult.Success;
            string obj = parameter.Parameters.ToString();

            List<Comment> comments = new();
            comments.Add(new Comment() { UserName = "System", Value = "Lütfen yorumlarımızda argo,küfür kullanmayalım. İyi eğlenceler." });

            try
            { // Aslında merkezi hata ayıklama sistemine göndersek iyi olacak ama pek uğraşmakta istemiyorum ;(
                var commentes = operations.Context.Comments.Where(xd => xd.ObjectGuid == obj && xd.IsActive).ToList();
                comments.AddRange(commentes);
            }
            catch (Exception ex)
            {

            }
            request.Object = comments;

            return YGate.Json.Operations.JsonSerialize.Serialize(request);
        }


        [HttpPost]
        [Authorized("Administrator")]
        public async Task<string> GetAll([FromBody] RequestParameter parameter)
        {
            RequestResult request = new("Get All Comments");
            request.Result = EnumRequestResult.Success;
            request.Object = operations.Context.Comments.ToList();
            return YGate.Json.Operations.JsonSerialize.Serialize(request);
        }

        [HttpPost]
        [Authorized("Administrator")]
        public async Task<string> Delete([FromBody] RequestParameter parameter)
        {
            RequestResult request = new("Delete Comment");
            string comGuid = parameter.Parameters.ToString();

            var com = operations.Context.Comments.FirstOrDefault(xd => xd.DBGuid == comGuid);
            operations.Context.Comments.Remove(com);
            operations.Context.SaveChanges();
            request.Result = EnumRequestResult.Success;
            request.Object = operations.Context.Comments.ToList();
            return YGate.Json.Operations.JsonSerialize.Serialize(request);
        }

        [HttpPost]
        [GetAuthorizeToken]
        public string Add([FromBody] RequestParameter parameter)
        {
            RequestResult result = new("Add Comment Operation Success");
            result.Result = EnumRequestResult.Success;
            YGate.Entities.BasedModel.Comment com = parameter.ConvertParameters<YGate.Entities.BasedModel.Comment>();
            com.IsActive = true;
            com.CreateDate = DateTime.UtcNow;
            Account user = new() { Username = "Anonim", DBGuid = "Anonim" };
            if (!string.IsNullOrEmpty(parameter.Token.ToString()))
            {
                com.CreatorGuid = parameter.Token.ToString();
                user = operations.Context.Accounts.FirstOrDefault(xd => xd.DBGuid == parameter.Token.ToString());
            }
            com.UserName = user.Username;
            com.CreatorGuid = user.DBGuid;
            operations.Context.Comments.Add(com);
            operations.Context.SaveChanges();

            return YGate.Json.Operations.JsonSerialize.Serialize(result);
        }
    }
}

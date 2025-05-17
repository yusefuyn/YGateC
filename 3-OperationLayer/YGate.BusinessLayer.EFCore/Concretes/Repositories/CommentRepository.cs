using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Entities.BasedModel;
using YGate.Entities;
using YGate.Interfaces.DomainLayer;
using YGate.Interfaces.OperationLayer.Repositories;
using YGate.Server.Facades;

namespace YGate.BusinessLayer.EFCore.Concretes.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private IBaseFacades baseFacades;
        private Operations operations;

        public CommentRepository(Operations operations, IBaseFacades baseFacades)
        {
            this.operations = operations;
            this.baseFacades = baseFacades;
        }

        public async Task<IRequestResult> Add(IRequestParameter parameter)
        {
            RequestResult result = new("Add Comment Operation Success");
            result.Result = EnumRequestResult.Success;
            Comment com = baseFacades.JsonSerializer.Deserialize<Comment>(parameter.Parameters.ToString());
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

            return result;
        }

        public async Task<IRequestResult> Delete(IRequestParameter parameter)
        {
            RequestResult request = new("Delete Comment");
            string comGuid = parameter.Parameters.ToString();

            var com = operations.Context.Comments.FirstOrDefault(xd => xd.DBGuid == comGuid);
            operations.Context.Comments.Remove(com);
            operations.Context.SaveChanges();
            request.Result = EnumRequestResult.Success;
            request.Object = operations.Context.Comments.ToList();
            return request;
        }

        public async Task<IRequestResult> GetAll(IRequestParameter parameter)
        {
            RequestResult request = new("Get All Comments");
            request.Result = EnumRequestResult.Success;
            request.Object = operations.Context.Comments.ToList();
            return request;
        }

        public async Task<IRequestResult> Gets(IRequestParameter parameter)
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

            return request;
        }
    }
}

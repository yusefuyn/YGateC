using YGate.Entities;
using YGate.Interfaces.Application.Advanced;

namespace YGate.Client.Services.Comment
{
    public class CommentService : ICommentService
    {
        HttpClientService httpClientService;
        public CommentService(HttpClientService httpClientService)
        {
            this.httpClientService = httpClientService;
        }


        public async Task<RequestResult> Add(Entities.BasedModel.Comment comment)
        {
            var res = await httpClientService.GetPostAsync<RequestResult>(comment, "api/Comment/Add");
            return res;
        }

        public async Task<RequestResult> Delete(string ObjectGuid)
        {
            var res = await httpClientService.GetPostAsync<RequestResult>(ObjectGuid, "api/Comment/Delete");
            return res;
        }

        public async Task<RequestResult> GetAll()
        {
            var res = await httpClientService.GetPostAsync<RequestResult>("", "api/Comment/GetAll");
            return res;
        }

        public async Task<RequestResult> Gets(string ObjectGuid)
        {
            var res = await httpClientService.GetPostAsync<RequestResult>(ObjectGuid, "api/Comment/Gets");
            return res;
        }
    }
}

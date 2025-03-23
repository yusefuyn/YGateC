using YGate.Entities;
using YGate.Entities.BasedModel;

namespace YGate.Client.Services.Comment
{
    public interface ICommentService
    {

        public Task<RequestResult> Add(YGate.Entities.BasedModel.Comment comment);

        public Task<RequestResult> Gets(string ObjectGuid);
        public Task<RequestResult> Delete(string ObjectGuid);
        public Task<RequestResult> GetAll();

    }
}

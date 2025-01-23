using YGate.Entities;
using YGate.Entities.ViewModels;

namespace YGate.Client.Services.Login
{
    public interface ILoginAndRegisterService
    {
        Task<RequestResult> LoginAsync(LoginViewModel loginViewModel);
        Task<RequestResult> RegisterAsync(RegisterViewModel registerViewModel);
    }
}

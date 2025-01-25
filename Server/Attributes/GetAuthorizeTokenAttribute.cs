
using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel;
using YGate.Entities;
using YGate.Json.Operations;

namespace YGate.Server.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    [Description("Bu Attribute kullanıldığı Aksiyondaki RequestParameter sınıfının Token Özelliğine kullanıcı ID'sini yerleştirmektedir.\nKullanıcı ID'ye sahip değilse boş bırakmaktadır.\n Tokenin geçerliliğinide kontrol etmektedir.")]
    /// <summary>
    /// Bunu tokenin olup olmaması durumlarında farklı işlem yapacak aksiyonlar için kullanın.
    /// </summary>
    public class GetAuthorizeTokenAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            HttpContext httpContext = context.HttpContext;
            RequestParameter? parameter = context.ActionArguments["parameter"] as RequestParameter;
            string requestToken = httpContext.Request.Headers["Authorization"].ToString();

            if (parameter == null)
                base.OnActionExecuting(context);

            if (string.IsNullOrEmpty(requestToken))
                requestToken = "";
            else
            {
                if (StaticTools.tokenService.ValidateJwtToken(ref requestToken))
                {
                    string userId = StaticTools.tokenService.GetUserRolesFromToken(requestToken);
                    requestToken = userId;
                }
            }
            parameter.Token = requestToken;
            base.OnActionExecuting(context);
        }
    }
}
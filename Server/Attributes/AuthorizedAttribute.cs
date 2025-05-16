using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using YGate.Entities;
using YGate.Interfaces.OperationLayer;
using YGate.Json;

namespace YGate.Server.Attributes
{



    /// <summary>
    /// Fazladan roller varsa "," karakteri ile ayırarak yazınız. \n "Administrator,Admin"
    /// </summary>
    public class AuthorizedAttribute : ActionFilterAttribute
    {
        private string[] Roles;
        JsonOperations JsonOperations = new();
        public AuthorizedAttribute(string Roles)
        {
            this.Roles = Roles.Split(',');
        }


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            HttpContext httpContext = context.HttpContext;
            string requestToken = httpContext.Request.Headers["Authorization"].ToString();

            if (context.ActionDescriptor.DisplayName.Contains("Setup") || context.ActionDescriptor.DisplayName.Contains("UpdateDatabase")) // Aksiyon setup'ise yada UpdateDatabase
            {
                base.OnActionExecuting(context); // Aksiyonu normal şekilde devam et
                return;
            }

            RequestResult result = new("You Not Authorize");
            result.Result = EnumRequestResult.Stop;
            result.To = EnumTo.Server;
            JsonOperations.Serialize(result);

            if (string.IsNullOrEmpty(requestToken))
            {
               
                context.Result = new JsonResult(result)
                {
                    StatusCode = 403 
                };
                return;
            }
            else
            {
                if (StaticTools.tokenService.ValidateJwtToken(ref requestToken))
                {
                    string userRoles = StaticTools.tokenService.GetUserRolesFromToken(requestToken);
                    string[] userRolesS = userRoles.Split(',');

                    bool hasCommonRole = userRolesS.Any(role => Roles.Contains(role));
                    if (hasCommonRole)
                    {
                        base.OnActionExecuting(context);
                    }
                    else
                    {
                        context.Result = new JsonResult(result)
                        {
                            StatusCode = 403 
                        };
                        return;
                    }
                }
                else
                {
                    context.Result = new JsonResult(result)
                    {
                        StatusCode = 403
                    };
                    return;
                }
            }
        }

    }
}
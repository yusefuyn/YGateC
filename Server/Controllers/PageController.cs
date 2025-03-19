using Microsoft.AspNetCore.Mvc;
using YGate.BusinessLayer.EFCore;
using YGate.Entities;
using YGate.Entities.BasedModel;
using YGate.Server.Attributes;

namespace YGate.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PageController : Controller
    {
        Operations operations;
        public PageController(Operations operations)
        {
            this.operations = operations;
        }

        [HttpPost]
        public async Task<string> GetPage([FromBody] RequestParameter parameter)
        {
            DynamicPageDynamicParameter page = parameter.ConvertParameters<DynamicPageDynamicParameter>();
            RequestResult returned = new($"Get Page {page.PageName}");
            returned.Result = EnumRequestResult.Success;
            DynamicPage dynamicPage = operations.Context.DynamicPages.FirstOrDefault(xd => xd.Name == page.PageName);
            if (dynamicPage == null)
            {
                returned.Object = $"<div class='container'><h3><h5 style='color:red'>{page.PageName}</h5> adında bir sayfa yok boşuna bekleme.</h3></div>";
            }
            else
            {
                page.Parameters.ForEach(xd => dynamicPage.Index = dynamicPage.Index.Replace(xd.ToString(), xd.Value.ToString()));
                returned.Object = dynamicPage.Index.ToString();
            }
            return YGate.Json.Operations.JsonSerialize.Serialize(returned);
        }

        [HttpPost]
        [GetAuthorizeToken]
        public async Task<string> GetPageObject([FromBody] RequestParameter parameter)
        {
            RequestResult requestResult = new("Get Page Object");
            requestResult.Result = EnumRequestResult.Success;
            string pageguid = parameter.Parameters.ToString();
            DynamicPage obj = operations.Context.DynamicPages.FirstOrDefault(xd => xd.DBGuid == pageguid);
            requestResult.Object = obj;

            if (string.IsNullOrEmpty(parameter.Token.ToString()) && obj.CreatorGuid != parameter.Token.ToString())
            {
                requestResult.Result = EnumRequestResult.Error;
                requestResult.LongDescription = "The object does not belong to you or you are not logged in.";
                requestResult.ShortDescription = "The object does not belong to you or you are not logged in.";
                requestResult.Object = null;
            }

            return YGate.Json.Operations.JsonSerialize.Serialize(requestResult);
        }

        [HttpPost]
        [GetAuthorizeToken]
        public async Task<string> UpdatePageObject([FromBody] RequestParameter parameter)
        {
            RequestResult requestResult = new("Update Page");


            DynamicPage page = parameter.ConvertParameters<DynamicPage>();
            DynamicPage updatePage = operations.Context.DynamicPages.FirstOrDefault(xd=> xd.DBGuid == page.DBGuid);

            if (updatePage.CreatorGuid != parameter.Token.ToString())
            {
                requestResult.Result = EnumRequestResult.Error;
                requestResult.LongDescription = "The object does not belong to you or you are not logged in.";
                requestResult.ShortDescription = "The object does not belong to you or you are not logged in.";
                requestResult.Object = null;
            }
            else
            {
                requestResult.Result = EnumRequestResult.Success;
                updatePage.Index = page.Index;
                updatePage.Name = page.Name;
                operations.Context.DynamicPages.Update(updatePage);
                operations.Context.SaveChanges();
                requestResult.LongDescription = "Operations success";
                requestResult.ShortDescription = "Operations success";

            }
            return YGate.Json.Operations.JsonSerialize.Serialize(requestResult);

        }

        [HttpPost]
        public async Task<string> GetPageButParameterPool([FromBody] RequestParameter parameter)
        {   // Burada şöyle olacak yine bir dynamicpageparameter objesine benzer bir obje olacak
            // sadece parametre havuzunu içinde barındırmayacak sadece parametre havuzunun id'sini içinde barındıracak
            // parametre havuzu veritabanından gelecek ve gelen havuz dataları sayfaya yerleştirilecek böylece
            // url oynanıp değiştirildiğinde sorun yaratacak sayfalar buradan alınacak misal ödeme sayfası
            DynamicPageParameter page = parameter.ConvertParameters<DynamicPageParameter>();
            RequestResult returned = new($"Get Page {page.PageName}");
            return YGate.Json.Operations.JsonSerialize.Serialize(returned);
        }

        [HttpPost]
        [GetAuthorizeToken]
        public async Task<string> AddPage([FromBody] RequestParameter parameter)
        {
            RequestResult returned = new($"Add Page Success");
            returned.Result = EnumRequestResult.Success;
            if (string.IsNullOrEmpty(parameter.Token) && !operations.IsThereSuchAUser(parameter.Token))
            {
                returned.Result = EnumRequestResult.Error;
                returned.ShortDescription = "Opss!";
                returned.LongDescription = "Opss!";
                return YGate.Json.Operations.JsonSerialize.Serialize(returned);
            }


            DynamicPage page = parameter.ConvertParameters<DynamicPage>();
            page.IsActive = true;
            page.DBGuid = YGate.String.Operations.GuidGen.Generate("Page");
            page.CreatorGuid = parameter.Token;
            operations.Context.DynamicPages.Add(page);
            operations.Context.SaveChanges();

            return YGate.Json.Operations.JsonSerialize.Serialize(returned);
        }

        [HttpPost]
        [Authorized("Administrator")]
        public async Task<string> GetAll([FromBody] RequestParameter parameter)
        {
            RequestResult returned = new($"Get All Page");
            returned.Result = EnumRequestResult.Success;
            returned.Object = operations.Context.DynamicPages.ToList();
            return YGate.Json.Operations.JsonSerialize.Serialize(returned);
        }

        [HttpPost]
        [Authorized("Administrator")]
        public async Task<string> Delete([FromBody] RequestParameter parameter)
        {
            RequestResult returned = new($"Delete Page");
            returned.Result = EnumRequestResult.Success;

            var page = operations.Context.DynamicPages.FirstOrDefault(xd => xd.DBGuid == parameter.Parameters.ToString());
            if (page == null)
            {
                returned.Result = EnumRequestResult.Error;
                return YGate.Json.Operations.JsonSerialize.Serialize(returned);
            }

            operations.Context.DynamicPages.Remove(page);
            operations.Context.SaveChanges();

            returned.Object = operations.Context.DynamicPages.ToList();
            return YGate.Json.Operations.JsonSerialize.Serialize(returned);
        }
    }
}

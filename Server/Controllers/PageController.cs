using Microsoft.AspNetCore.Mvc;
using YGate.BusinessLayer.EFCore;
using YGate.Entities;
using YGate.Entities.BasedModel;
using YGate.Server.Attributes;
using YGate.Server.Facades;

namespace YGate.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PageController : Controller
    {
        Operations operations;
        IBaseFacades baseFacades;
        public PageController(Operations operations, IBaseFacades baseFacades)
        {
            this.operations = operations;
            this.baseFacades = baseFacades;
        }

        [HttpPost]
        public async Task<string> GetPage([FromBody] RequestParameter parameter)
        {
            DynamicPageDynamicParameter page = parameter.ConvertParameters<DynamicPageDynamicParameter>();
            RequestResult returned = new($"Get Page {page.PageName}");
            returned.Result = EnumRequestResult.Success;
            DynamicPage dynamicPage = operations.Context.DynamicPages.FirstOrDefault(xd => xd.Name == page.PageName);
            if (dynamicPage == null)
                page.PageSource = $"<div class='container'><h3><h5 style='color:red'>{page.PageName}</h5> adında bir sayfa yok boşuna bekleme.</h3></br><a class='special-button' href='/'>Ana Sayfa Yönlen</a></div>";
            else
            {
                page.PageName = dynamicPage.Name.ToString();
                page.PageSource = dynamicPage.Index.ToString();
            }
            returned.Object = page;
            return baseFacades.JsonSerializer.Serialize(returned);
        }




        [HttpPost]
        [GetAuthorizeToken]
        public async Task<string> MyParametersPages([FromBody] RequestParameter parameter)
        {
            RequestResult returned = new("Your parameters uri");
            returned.Result = EnumRequestResult.Success;
            if (string.IsNullOrEmpty(parameter.Token))
                returned.Object = new List<string>();
            else
                returned.Object = operations.Context.PageParameters.Where(xd => xd.CreatorGuid == parameter.Token && xd.CreateDate > DateTime.UtcNow.AddMinutes(-10)).Select(xd => $"Show/p/Gates/{xd.PageName}").Distinct().ToList();
            return baseFacades.JsonSerializer.Serialize(returned);
        }


        [HttpPost]
        [GetAuthorizeToken]
        public async Task<string> SavePageParameters([FromBody] RequestParameter parameter)
        {
            List<PageParameter> pageParameters = parameter.ConvertParameters<List<PageParameter>>();
            string pageGuid = YGate.String.Operations.GuidGen.Generate("TempParametersGroup");
            string Token = "";
            if (!string.IsNullOrEmpty(parameter.Token))
                Token = parameter.Token;
            pageParameters.ForEach(xd => { xd.CreatorGuid = Token; xd.PageName = pageGuid; xd.Id = 0; xd.CreateDate = DateTime.UtcNow; });
            operations.Context.PageParameters.RemoveRange(operations.Context.PageParameters.Where(xd => xd.CreateDate < DateTime.UtcNow.AddMinutes(-10))); // 10dk geçenleri sil
            operations.Context.PageParameters.AddRange(pageParameters);
            operations.Context.SaveChanges();
            return pageGuid;
        }




        [HttpPost]
        [GetAuthorizeToken]
        public async Task<string> GetPageForGuid([FromBody] RequestParameter parameter)
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

            return baseFacades.JsonSerializer.Serialize(requestResult);
        }


        [HttpPost]
        public async Task<string> GetPageForName([FromBody] RequestParameter parameter)
        {
            RequestResult requestResult = new("Get Page Object");
            requestResult.Result = EnumRequestResult.Success;
            string pageName = parameter.Parameters.ToString();
            try
            {
                DynamicPage obj = operations.Context.DynamicPages.FirstOrDefault(xd => xd.Name == pageName);
                if (obj == null)
                    obj = new() { Index = $"<div class='container'><h3>{pageName} adında bir sayfa yok yada bulunamadı.</h3></div>" };
                requestResult.Object = obj;
            }
            catch (Exception ex)
            {

            }

            return baseFacades.JsonSerializer.Serialize(requestResult);
        }

        [HttpPost]
        [GetAuthorizeToken]
        public async Task<string> UpdatePageObject([FromBody] RequestParameter parameter)
        {
            RequestResult requestResult = new("Update Page");


            DynamicPage page = parameter.ConvertParameters<DynamicPage>();
            DynamicPage updatePage = operations.Context.DynamicPages.FirstOrDefault(xd => xd.DBGuid == page.DBGuid);

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
            return baseFacades.JsonSerializer.Serialize(requestResult);

        }

        [HttpPost]
        public async Task<string> GetPageButParameterPool([FromBody] RequestParameter parameter)
        {
            DynamicPageParameter page = parameter.ConvertParameters<DynamicPageParameter>();
            RequestResult returned = new($"Get Page {page.PageName}");
            returned.Result = EnumRequestResult.Success;
            DynamicPageDynamicParameter returnedObj = new(page.PageName.ToString());

            var pageO = operations.Context.DynamicPages.FirstOrDefault(xd => xd.Name == page.PageName.ToString());

            if (pageO == null)
            {
                returned.Result = EnumRequestResult.Error;
                returned.ShortDescription = "Sayfa yok 404 :O";
                returned.LongDescription = "Sayfa yok 404 :O";
                return baseFacades.JsonSerializer.Serialize(returned);
            }

            returnedObj.PageSource = pageO.Index.ToString();
            returnedObj.PageName = pageO.Name.ToString();
            returnedObj.Parameters = operations.Context.PageParameters.Where(xd => xd.PageName == page.ParameterPoolName.ToString()).ToList();
            // Belki daha sonra indexlenmiş halini direk gönderirirz.
            // belli olmaz.
            returned.Object = returnedObj;
            return baseFacades.JsonSerializer.Serialize(returned);
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
                return baseFacades.JsonSerializer.Serialize(returned);
            }


            DynamicPage page = parameter.ConvertParameters<DynamicPage>();
            page.IsActive = true;
            page.DBGuid = YGate.String.Operations.GuidGen.Generate("Page");
            page.CreatorGuid = parameter.Token;
            operations.Context.DynamicPages.Add(page);
            operations.Context.SaveChanges();

            return baseFacades.JsonSerializer.Serialize(returned);
        }

        [HttpPost]
        [Authorized("Administrator")]
        public async Task<string> GetAll([FromBody] RequestParameter parameter)
        {
            RequestResult returned = new($"Get All Page");
            returned.Result = EnumRequestResult.Success;
            returned.Object = operations.Context.DynamicPages.ToList();
            return baseFacades.JsonSerializer.Serialize(returned);
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
                return baseFacades.JsonSerializer.Serialize(returned);
            }

            operations.Context.DynamicPages.Remove(page);
            operations.Context.SaveChanges();

            returned.Object = operations.Context.DynamicPages.ToList();
            return baseFacades.JsonSerializer.Serialize(returned);
        }
    }
}

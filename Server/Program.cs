using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YGate.BusinessLayer.EFCore;
using YGate.BusinessLayer.EFCore.Concretes.Repositories;
using YGate.Client;
using YGate.Client.Services;
using YGate.Entities;
using YGate.Interfaces.OperationLayer;
using YGate.Interfaces.OperationLayer.Repositories;
using YGate.Json;
using YGate.Json.Operations;
using YGate.Mail.Operations;
using YGate.Server;
using YGate.Server.Controllers;
using YGate.Server.Facades;
using YGate.Server.Middlewares;

var builder = WebApplication.CreateBuilder(args);

string TokenPasswordConf = YGate.String.Operations.Hash.ComputeSHA256(builder.Configuration["TokenConfiguration:TokenPassword"]); // Json Token Ayarlar�
int ValidityTimeConf = Convert.ToInt32(builder.Configuration["TokenConfiguration:ValidityTime"].ToString());

YGate.Server.StaticTools.tokenService = new TokenService(new JsonOperations(), ValidityTimeConf, TokenPasswordConf);

#region SonraDuzenle
List<ConnectionString> dbSettingsSection = builder.Configuration.GetSection("DbSettings").Get<List<ConnectionString>>();

builder.Services.AddSingleton<IJsonSerializer, JsonOperations>();

builder.Services.AddSingleton<ITokenService, TokenService>(xd =>
{
    var JsonServ = xd.GetService<IJsonSerializer>();
    TokenService to = new TokenService(JsonServ, ValidityTimeConf, TokenPasswordConf);
    return to;
});
builder.Services.AddSingleton<IBaseFacades, BaseFacades>();


YGate.Server.StaticTools.SiteName = builder.Configuration.GetSection("SiteSettings").GetValue<string>("Title");
YGate.Server.StaticTools.AllowedRequestCountTimeout = builder.Configuration.GetSection("SiteSettings").GetValue<int>("AllowedRequestCountTimeout");
YGate.Server.StaticTools.NumberOfAllowedRequests = builder.Configuration.GetSection("SiteSettings").GetValue<int>("NumberOfAllowedRequests");

builder.Services.AddScoped<IMailService, MailServices>(xd =>
{ // SMTP Ayalar�
    var res = new MailServices();
    res.SenderSettings(builder.Configuration.GetValue<string>("MailSettings:Mail"), builder.Configuration.GetValue<string>("MailSettings:Password"));
    res.ServiceSettings(builder.Configuration.GetValue<string>("MailSettings:SmtpAddress"), builder.Configuration.GetValue<int>("MailSettings:Port"), builder.Configuration.GetValue<bool>("MailSettings:SSL"));
    return res;
});


builder.Services.AddScoped<Operations>(xd =>
{ // DbAyarlar�

    var jsonService = xd.GetService<IJsonSerializer>();
    Operations returnedOperations = new(jsonService);
    returnedOperations.AddDbSettings(dbSettingsSection);
    return returnedOperations;
});


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login"; // Giri� sayfan�z�n yolu
        options.LogoutPath = "/Logout"; // ��k�� sayfan�z�n yolu
        options.AccessDeniedPath = "/AccessDenied"; // Eri�im reddedildi�inde y�nlendirme
    });




//  auth
builder.Services.AddScoped<CookieService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
#endregion

#region Repository
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IAdministratorRepository, AdministratorRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ILoginRegisterRepository, LoginRegisterRepository>();
builder.Services.AddScoped<IEntitieRepository, EntitieRepository>();
builder.Services.AddScoped<IMeasurementRepository, MeasurementRepository>();
builder.Services.AddScoped<IPageRepository, PageRepository>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IStatisticsRepository, StatisticsRepository>();
#endregion

builder.Services.AddAuthorizationCore();
builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSignalR();
builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
builder.Services.AddRazorPages();
var app = builder.Build();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.MapRazorPages();
app.MapControllers();


#region Middlewares
app.UseMiddleware<RequestMiddleware>();
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
#endregion

app.MapHub<MyHub>("/MyHub");
app.MapFallbackToFile("index.html");
app.Run();
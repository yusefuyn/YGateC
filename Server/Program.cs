using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YGate.BusinessLayer.EFCore;
using YGate.Client;
using YGate.Client.Services;
using YGate.Entities;
using YGate.Interfaces.OperationLayer;
using YGate.Json;
using YGate.Json.Operations;
using YGate.Mail.Operations;
using YGate.Server;
using YGate.Server.Controllers;
using YGate.Server.Facades;
using YGate.Server.Middlewares;

var builder = WebApplication.CreateBuilder(args);
#region SonraDuzenle
List<ConnectionString> dbSettingsSection = builder.Configuration.GetSection("DbSettings").Get<List<ConnectionString>>();

builder.Services.AddSingleton<IJsonSerializer, JsonOperations>();
builder.Services.AddSingleton<IBaseFacades, BaseFacades>();


YGate.Server.StaticTools.SiteName = builder.Configuration.GetSection("SiteSettings").GetValue<string>("Title");
YGate.Server.StaticTools.AllowedRequestCountTimeout = builder.Configuration.GetSection("SiteSettings").GetValue<int>("AllowedRequestCountTimeout");
YGate.Server.StaticTools.NumberOfAllowedRequests = builder.Configuration.GetSection("SiteSettings").GetValue<int>("NumberOfAllowedRequests");

builder.Services.AddScoped<MailServices>(xd =>
{ // SMTP Ayalarý
    var res = new MailServices();
    res.SenderSettings(builder.Configuration.GetValue<string>("MailSettings:Mail"), builder.Configuration.GetValue<string>("MailSettings:Password"));
    res.ServiceSettings(builder.Configuration.GetValue<string>("MailSettings:SmtpAddress"), builder.Configuration.GetValue<int>("MailSettings:Port"), builder.Configuration.GetValue<bool>("MailSettings:SSL"));
    return res;
});


builder.Services.AddScoped<Operations>(xd =>
{ // DbAyarlarý

    var jsonService = xd.GetService<IJsonSerializer>();
    Operations returnedOperations = new(jsonService);
    returnedOperations.AddDbSettings(dbSettingsSection);
    return returnedOperations;
});


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login"; // Giriþ sayfanýzýn yolu
        options.LogoutPath = "/Logout"; // Çýkýþ sayfanýzýn yolu
        options.AccessDeniedPath = "/AccessDenied"; // Eriþim reddedildiðinde yönlendirme
    });

string TokenPasswordConf = builder.Configuration["TokenConfiguration:TokenPassword"]; // Json Token Ayarlarý
int ValidityTimeConf = Convert.ToInt32(builder.Configuration["TokenConfiguration:ValidityTime"].ToString());

YGate.Server.StaticTools.tokenService = new(new JsonOperations(), ValidityTimeConf, YGate.String.Operations.Hash.ComputeSHA256(TokenPasswordConf));

//  auth
builder.Services.AddScoped<CookieService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
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
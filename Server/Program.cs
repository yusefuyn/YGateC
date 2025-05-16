using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YGate.BusinessLayer.EFCore;
using YGate.Client;
using YGate.Client.Services;
using YGate.Entities;
using YGate.Json.Operations;
using YGate.Mail.Operations;
using YGate.Server;
using YGate.Server.Controllers;
using YGate.Server.Middlewares;

var builder = WebApplication.CreateBuilder(args);

List<ConnectionString> dbSettingsSection = builder.Configuration.GetSection("DbSettings").Get<List<ConnectionString>>();

builder.Services.AddSingleton<Operations>(xd =>
{ // DbAyarlar�
    Operations returnedOperations = new();
    returnedOperations.AddDbSettings(dbSettingsSection);
    return returnedOperations;
});

YGate.Server.StaticTools.SiteName = builder.Configuration.GetSection("SiteSettings").GetValue<string>("Title");
YGate.Server.StaticTools.AllowedRequestCountTimeout = builder.Configuration.GetSection("SiteSettings").GetValue<int>("AllowedRequestCountTimeout");
YGate.Server.StaticTools.NumberOfAllowedRequests = builder.Configuration.GetSection("SiteSettings").GetValue<int>("NumberOfAllowedRequests");

builder.Services.AddScoped<MailServices>(xd =>
{ // SMTP Ayalar�
    var res = new MailServices();
    res.SenderSettings(builder.Configuration.GetValue<string>("MailSettings:Mail"), builder.Configuration.GetValue<string>("MailSettings:Password"));
    res.ServiceSettings(builder.Configuration.GetValue<string>("MailSettings:SmtpAddress"), builder.Configuration.GetValue<int>("MailSettings:Port"), builder.Configuration.GetValue<bool>("MailSettings:SSL"));
    return res;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login"; // Giri� sayfan�z�n yolu
        options.LogoutPath = "/Logout"; // ��k�� sayfan�z�n yolu
        options.AccessDeniedPath = "/AccessDenied"; // Eri�im reddedildi�inde y�nlendirme
    });

string TokenPasswordConf = builder.Configuration["TokenConfiguration:TokenPassword"]; // Json Token Ayarlar�
int ValidityTimeConf = Convert.ToInt32(builder.Configuration["TokenConfiguration:ValidityTime"].ToString());

YGate.Server.StaticTools.tokenService =
    new() { __secretkey = YGate.String.Operations.Hash.ComputeSHA256(TokenPasswordConf), ValidityTime = ValidityTimeConf };

//  auth
builder.Services.AddScoped<CookieService>();

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddAuthorizationCore();
builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddHttpContextAccessor();
//  auth
builder.Services.AddSignalR();

builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
builder.Services.AddRazorPages();
var app = builder.Build();
//app.UseCors("AllowAllOrigins");
//  auth
//app.UseStaticFiles();
//app.UseAntiforgery();
app.UseRouting();  // Bu �a�r� burada olmal�
app.UseAuthentication();
app.UseAuthorization();
// auth

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

app.MapHub<MyHub>("/MyHub"); // SignalR Burada.
app.MapFallbackToFile("index.html");
app.Run();
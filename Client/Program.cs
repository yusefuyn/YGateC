using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using YGate.Client;
using YGate.Client.Services;
using YGate.Client.Services.Administrator;
using YGate.Client.Services.Category;
using YGate.Client.Services.Comment;
using YGate.Client.Services.Entitie;
using YGate.Client.Services.Login;
using YGate.Client.Services.Measurement;
using YGate.Client.Services.Page;
using YGate.Client.Services.Profile;
using YGate.Client.Services.Property;
using YGate.Client.Services.Role;
using YGate.Client.Services.Statistics;
using YGate.Interfaces.OperationLayer;
using YGate.Json;
using YGate.Json.Operations;
using YGate.Shared;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var configuration = builder.Configuration;

RulesAndRoles.Rules = new() {
    new RuleAndRoles(1,"SidebarManagementButton","Administrator"),
    new RuleAndRoles(2,"SidebarEntityButton","Administrator,Admin,MarketMod,MarketUser,User")
};

builder.Services.AddScoped<CookieService>();
builder.Services.AddScoped(sp => {
    HttpClient returned = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
    StaticTools.httpClient = returned;
    return returned;
});
builder.Services.AddScoped<HttpClientService>();

builder.Services.AddScoped<ILoginAndRegisterService, LoginAndRegisterService>();
builder.Services.AddScoped<IAdministratorService, AdministratorService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProfileService, ProfileService>(); // Kullanýcý profil bilgilerini tutar yönetir, deðiþtirir.
builder.Services.AddScoped<IMeasurementService, MeasurementService>(); 
builder.Services.AddScoped<IPropertyService, PropertyService>();
builder.Services.AddScoped<IStatisticsService, StatisticsService>();
builder.Services.AddScoped<IEntitieService, EntitieService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IEntitieViewService, EntitieViewService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IPageService, PageService>();
builder.Services.AddScoped<IJsonSerializer, JsonOperations>();
builder.Services.AddLocalization();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
await builder.Build().RunAsync();

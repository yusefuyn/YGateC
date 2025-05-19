using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using YGate.Client.Pages.Category;
using YGate.Client.Shared.Components;
using YGate.Entities.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.SignalR.Client;
using YGate.Client.Pages;
using YGate.Client.Services.Category;
using YGate.Client.Services.Statistics;
using YGate.Client.Shared.Components;
using YGate.Entities.ViewModels;
using YGate.Client.Services.Profile;
using YGate.Entities.ViewModels.Chat;
using YGate.Interfaces.DomainLayer;


namespace YGate.Client.Shared
{
    [Authorize]
    public partial class Sidebar : IRootPage
    {
        private List<CategorySidebarViewModel> categorySidebarViewModels = null;
        private bool isRefreshing = false;
        public string SignalRGroupName { get; set; }
        public HubConnection? hubConnection { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await RefreshData();

            hubConnection = new HubConnectionBuilder()
             .WithUrl(navigationManager.ToAbsoluteUri("/MyHub"))
             .Build();

            hubConnection.On("RefreshSiteName", async () => await RefreshSiteName());
            hubConnection.On("RefreshSideBar", async () => { 
                isRefreshing = false;
                categorySidebarViewModels = null;
                await CheckForNewDataPeriodically();
            });

            await hubConnection.StartAsync();
            await JoinGroup();
        }

        private async Task CheckForNewDataPeriodically()
        {
            while (categorySidebarViewModels == null)
            {
                if (!isRefreshing)
                {
                    isRefreshing = true;
                    await RefreshSideBar();
                    isRefreshing = false;
                }

                await Task.Delay(1000);
            }
        }


        private async Task RefreshSideBar()
        {
            var res = await categoryService.GetSidebarCategories();
            categorySidebarViewModels = res.ConvertRequestObject<List<CategorySidebarViewModel>>();
            StateHasChanged();
        }

        public async Task RefreshData()
        {
            await CheckForNewDataPeriodically();
            SignalRGroupName = "SideBar";

            await RefreshSiteName();

            StateHasChanged();
        }

        private async Task RefreshSiteName()
        {
            var res1 = await statisticService.GetSiteName();
            if (res1.Result == EnumRequestResult.Success)
                YGate.Client.StaticTools.SiteName = res1.Object.ToString();
            StateHasChanged();

        }

        public async ValueTask DisposeAsync()
        {
            if (!string.IsNullOrEmpty(SignalRGroupName) && hubConnection != null)
            {
                await hubConnection.SendAsync("ExitGroup", SignalRGroupName);
            }
        }

        public async Task JoinGroup()
        {
            if (!string.IsNullOrEmpty(SignalRGroupName) && hubConnection != null)
            {
                await hubConnection.SendAsync("JoinGroup", SignalRGroupName);
            }
        }
    }
}

using Microsoft.AspNetCore.SignalR.Client;

namespace YGate.Client
{
    public interface IRootPage : IAsyncDisposable
    {
        public Task RefreshData();
        public string SignalRGroupName { get; set; }
        public HubConnection? hubConnection { get; set; }
        public ValueTask DisposeAsync();
    }
}

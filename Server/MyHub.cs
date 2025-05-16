using Microsoft.AspNetCore.SignalR;
using YGate.Server.Facades;

namespace YGate.Server
{
    public class MyHub : Hub
    {
        IBaseFacades baseFacades;
        public MyHub(IBaseFacades baseFacades)
        {
            this.baseFacades = baseFacades;
        }

        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            List<(string Key, string Value)> obj = new List<(string Key, string Value)>
            {
                ("System", "Odaya yeni birisi katıldı."),
                ("System", "Lütfen saygılı olalım.") ,
                ("System", "Bu oda tipi kayıt tutmaz ve herkeze açık yayın yapar.Herhangi bir güvenliğe tabi tutulmaz.")
            };
            obj.ForEach(async xd =>
            {
                await SendMessageToGroup(groupName, baseFacades.JsonSerializer.Serialize(xd));
            });
        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            (string Key, string Value) obj = new("System", "Odadan birisi ayrıldı.");
            await SendMessageToGroup(groupName, baseFacades.JsonSerializer.Serialize(obj));
        }

        public async Task SendMessageToGroup(string groupName, string message)
        {
            await Clients.Group(groupName).SendAsync("ReceiveMessage", message);
        }
    }
}

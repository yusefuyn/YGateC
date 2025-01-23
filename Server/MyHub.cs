using Microsoft.AspNetCore.SignalR;

namespace YGate.Server
{
    public class MyHub : Hub
    {
        public MyHub()
        {
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
                await SendMessageToGroup(groupName, YGate.Json.Operations.JsonSerialize.Serialize(xd));
            });
        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            (string Key, string Value) obj = new("System", "Odadan birisi ayrıldı.");
            await SendMessageToGroup(groupName, YGate.Json.Operations.JsonSerialize.Serialize(obj));
        }

        public async Task SendMessageToGroup(string groupName, string message)
        {
            await Clients.Group(groupName).SendAsync("ReceiveMessage", message);
        }
    }
}

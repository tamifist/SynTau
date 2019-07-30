using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Shared.DTO.Responses;
using Shared.Resources;

namespace AutoGene.Presentation.Host.Hubs
{
    [HubName(Configuration.SignalHubName)]
    public class SignalHub: Hub
    {
        public SignalHub()
        {
            
        }

        public async Task<SignalMessage> Send(SignalMessage signalMessage)
        {
            await Clients.Group(signalMessage.GroupId).addMessage(signalMessage);

            return signalMessage;
        }

        public Task JoinGroup(string groupId)
        {
            return Groups.Add(Context.ConnectionId, groupId);
        }

        public Task LeaveGroup(string groupId)
        {
            return Groups.Remove(Context.ConnectionId, groupId);
        }
    }
}
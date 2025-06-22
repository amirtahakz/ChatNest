using System.Threading.Tasks;

namespace ChatNest.Client.Hubs
{
    public interface IChatHub
    {
        //Task CreateGroup(string groupName);
        //Task SendMessage(string text );
        Task JoinGroup(string token, long currentGroupId);
        Task SendMessage(string text, long groupId);
        Task JoinPrivateGroup(long receiverId, long currentGroupId);
    }
}
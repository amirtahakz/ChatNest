using System.Collections.Generic;
using System.Threading.Tasks;
using ChatNest.Core.ViewModels.Chats;


namespace ChatNest.Core.Services.Users.UserGroups
{
    public interface IUserGroupService
    {
        Task<List<UserGroupViewModel>> GetUserGroups(long userId);
        Task JoinGroup(long userId, long groupId);
        Task<bool> IsUserInGroup(long userId, long groupId);
        Task<bool> IsUserInGroup(long userId, string token);
        Task<List<string>> GetUserIds(long groupId);
        Task JoinGroup(List<long> userIds, long groupId);
    }
}
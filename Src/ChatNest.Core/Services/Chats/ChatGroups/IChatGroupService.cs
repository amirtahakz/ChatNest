using System.Collections.Generic;
using System.Threading.Tasks;
using ChatNest.Core.ViewModels.Chats;
using ChatNest.Data.Entities.Chats;

namespace ChatNest.Core.Services.Chats.ChatGroups
{
    public interface IChatGroupService
    {
        Task<List<SearchResultViewModel>> Search(string title, long userId);
        Task<List<ChatGroup>> GetUserGroups(long userId);
        Task<ChatGroup> InsertGroup(CreateGroupViewModel model);
        Task<ChatGroup> GetGroupBy(long id);
        Task<ChatGroup> GetGroupBy(string token);
        Task<ChatGroup> InsertPrivateGroup(long userId, long receiverId);
    }
}
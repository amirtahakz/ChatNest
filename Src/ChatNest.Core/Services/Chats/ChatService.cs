using ChatNest.Core.ViewModels.Chats;
using ChatNest.Data.Context;
using ChatNest.Data.Entities.Chats;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatNest.Core.Services.Chats
{
    public class ChatService : BaseService ,IChatService
    {
        public ChatService(TestChatContext context) : base(context)
        {
        }

        public async Task SendMessage(Chat chat)
        {
            Insert(chat);
            await Save();
        }
        public async Task<List<ChatViewModel>> GetChatGroup(long groupId)
        {
            return await Table<Chat>()
                .Include(c => c.User)
                .Include(c => c.ChatGroup)
                .Where(g => g.GroupId == groupId)
                .Select(s => new ChatViewModel()
                {
                    UserName = s.User.UserName,
                    CreateDate = $"{s.CreateDate.Hour}:{s.CreateDate.Minute}",
                    ChatBody = s.ChatBody,
                    GroupName = s.ChatGroup.GroupTitle,
                    UserId = s.UserId,
                    GroupId = s.GroupId
                }).ToListAsync();
        }
    }
}

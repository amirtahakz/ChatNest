using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ChatNest.Core.Services.Chats;
using ChatNest.Core.Services.Chats.ChatGroups;
using ChatNest.Core.Services.Users.UserGroups;
using ChatNest.Core.Utilities;
using ChatNest.Core.ViewModels.Chats;
using ChatNest.Data.Entities.Chats;
using Microsoft.AspNetCore.SignalR;

namespace ChatNest.Client.Hubs
{
    public class ChatHub : Hub, IChatHub
    {
        private IChatGroupService _groupService;
        private IUserGroupService _userGroup;
        private IChatService _chatService;

        public ChatHub(IChatGroupService groupService, IUserGroupService userGroup, IChatService chatService)
        {
            _groupService = groupService;
            _userGroup = userGroup;
            _chatService = chatService;
        }

        public override Task OnConnectedAsync()
        {
            Clients.Caller.SendAsync("Welcome", Context.User.GetUserId());
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        //public async Task SendMessage(string text)
        //{
        //    var userName = Context.Owner.FindFirstValue(ClaimTypes.Name);
        //    await Clients.All.SendAsync("ReceiveMessage" , $"{userName} : {text}");
        //}
        //public async Task CreateGroup(string groupName)
        //{
        //    try
        //    {
        //        var group = await _groupService.InsertGroup(groupName, Context.Owner.GetUserId());
        //        await Clients.Caller.SendAsync("NewGroup", groupName, group.GroupToken);
        //    }
        //    catch
        //    {
        //        await Clients.Caller.SendAsync("NewGroup", "Error");
        //    }
        //}

        public async Task JoinGroup(string token, long currentGroupId)
        {
            var group = await _groupService.GetGroupBy(token);
            var groupDto = FixGroupModel(group);
            if (group == null)
                await Clients.Caller.SendAsync("Error", "Group Not Found");
            else
            {
                var chats = await _chatService.GetChatGroup(group.Id);
                if (!await _userGroup.IsUserInGroup(Context.User.GetUserId(), token))
                {
                    await _userGroup.JoinGroup(Context.User.GetUserId(), group.Id);
                    await Clients.Caller.SendAsync("NewGroup", groupDto.GroupTitle, groupDto.GroupToken, groupDto.ImageName);
                }
                if (currentGroupId > 0)
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, currentGroupId.ToString());

                await Groups.AddToGroupAsync(Context.ConnectionId, group.Id.ToString());
                await Clients.Caller.SendAsync("JoinGroup", groupDto, chats);
            }
        }

        public async Task SendMessage(string text, long groupId)
        {
            var group = await _groupService.GetGroupBy(groupId);
            if (group == null)
                return;
            var chat = new Chat()
            {
                ChatBody = text,
                GroupId = groupId,
                CreateDate = DateTime.Now,
                UserId = Context.User.GetUserId()
            };
            await _chatService.SendMessage(chat);
            var chatModel = new ChatViewModel()
            {
                ChatBody = text,
                UserName = Context.User.GetUserName(),
                CreateDate = $"{chat.CreateDate.Hour}:{chat.CreateDate.Minute}",
                UserId = Context.User.GetUserId(),
                GroupName = group.GroupTitle,
                GroupId = groupId
            };
            var userIds = await _userGroup.GetUserIds(groupId);
            await Clients.Users(userIds).SendAsync("ReceiveNotification", chatModel);
            await Clients.Group(groupId.ToString()).SendAsync("ReceiveMessage", chatModel);
        }

        public async Task JoinPrivateGroup(long receiverId, long currentGroupId)
        {
            if (currentGroupId > 0)
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, currentGroupId.ToString());

            var group = await _groupService.InsertPrivateGroup(Context.User.GetUserId(), receiverId);
            var groupDto = FixGroupModel(group);

            if (!await _userGroup.IsUserInGroup(Context.User.GetUserId(), group.GroupToken))
            {
                await _userGroup.JoinGroup(new List<long>()
                    { groupDto.ReceiverId ?? 0, group.OwnerId }, group.Id);

                await Clients.Caller.SendAsync("NewGroup", groupDto.GroupTitle, groupDto.GroupToken, groupDto.ImageName);
                await Clients.User(groupDto.ReceiverId.ToString()).SendAsync("NewGroup", Context.User.GetUserName(), groupDto.GroupToken, groupDto.ImageName);
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, group.Id.ToString());


            var chats = await _chatService.GetChatGroup(group.Id);


            await Clients.Caller.SendAsync("JoinGroup", groupDto, chats);
        }


        #region Utils

        private ChatGroup FixGroupModel(ChatGroup chatGroup)
        {
            if (chatGroup.IsPrivate)
            {
                if (chatGroup.OwnerId == Context.User.GetUserId())
                {
                    return new ChatGroup()
                    {
                        Id = chatGroup.Id,
                        GroupToken = chatGroup.GroupToken,
                        CreateDate = chatGroup.CreateDate,
                        GroupTitle = chatGroup.Receiver.UserName,
                        ImageName = chatGroup.Receiver.Avatar,
                        IsPrivate = false,
                        OwnerId = chatGroup.OwnerId,
                        ReceiverId = chatGroup.ReceiverId
                    };
                }
                return new ChatGroup()
                {
                    Id = chatGroup.Id,
                    GroupToken = chatGroup.GroupToken,
                    CreateDate = chatGroup.CreateDate,
                    GroupTitle = chatGroup.Owner.UserName,
                    ImageName = chatGroup.Owner.Avatar,
                    IsPrivate = false,
                    OwnerId = chatGroup.OwnerId,
                    ReceiverId = chatGroup.ReceiverId
                };
            }
            return new ChatGroup()
            {
                Id = chatGroup.Id,
                GroupToken = chatGroup.GroupToken,
                CreateDate = chatGroup.CreateDate,
                GroupTitle = chatGroup.GroupTitle,
                ImageName = chatGroup.ImageName,
                IsPrivate = false,
                OwnerId = chatGroup.OwnerId,
                ReceiverId = chatGroup.ReceiverId
            };
        }

        #endregion
    }
}

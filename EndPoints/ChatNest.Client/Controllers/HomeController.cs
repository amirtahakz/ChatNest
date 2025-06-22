using ChatNest.Client.Hubs;
using ChatNest.Core.Services.Chats.ChatGroups;
using ChatNest.Core.Services.Users.UserGroups;
using ChatNest.Core.Utilities;
using ChatNest.Core.ViewModels.Chats;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ChatNest.Client.Controllers
{
    public class HomeController : Controller
    {
        private IChatGroupService _chatGroup;
        private IHubContext<ChatHub> _chatHub;
        private IUserGroupService _userGroup;

        public HomeController(IChatGroupService chatGroup, IHubContext<ChatHub> chatHub, IUserGroupService userGroup)
        {
            _chatGroup = chatGroup;
            _chatHub = chatHub;
            _userGroup = userGroup;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var model = await _userGroup.GetUserGroups(User.GetUserId());
            return View(model);
        }
        [Authorize]
        [HttpPost]
        public async Task CreateGroup([FromForm] CreateGroupViewModel model)
        {
            try
            {
                model.UserId = User.GetUserId();
                var result = await _chatGroup.InsertGroup(model);
                await _chatHub.Clients.User(User.GetUserId().ToString()).SendAsync("NewGroup", result.GroupTitle, result.GroupToken, result.ImageName);
            }
            catch
            {
                await _chatHub.Clients.User(User.GetUserId().ToString()).SendAsync("NewGroup", "ERROR");
            }
        }

        public async Task<IActionResult> Search(string title)
        {
            return new ObjectResult(await _chatGroup.Search(title, User.GetUserId()));
        }
    }
}

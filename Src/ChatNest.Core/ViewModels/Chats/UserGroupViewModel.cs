using ChatNest.Data.Entities.Chats;

namespace ChatNest.Core.ViewModels.Chats
{
    public class UserGroupViewModel
    {
        public string GroupName { get; set; }
        public string Token { get; set; }
        public string ImageName { get; set; }
        public Chat LastChat { get; set; }
    }
}
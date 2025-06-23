using ChatNest.Data.Entities;
using ChatNest.Data.Entities.Chats;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatNest.Data.Entities.Users
{
    public class User : BaseEntity
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Avatar { get; set; }

        [InverseProperty("Owner")]
        public ICollection<ChatGroup> OwnedChatGroups { get; set; } = new HashSet<ChatGroup>();
        [InverseProperty("Receiver")]
        public ICollection<ChatGroup> ReceivedPrivateGroup { get; set; } = new HashSet<ChatGroup>();
        public ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
        public ICollection<Chat> Chats { get; set; } = new HashSet<Chat>();
        public ICollection<UserGroup> UserGroups { get; set; } = new HashSet<UserGroup>();
    }
}

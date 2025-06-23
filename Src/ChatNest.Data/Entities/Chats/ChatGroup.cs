using ChatNest.Data.Entities;
using ChatNest.Data.Entities.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatNest.Data.Entities.Chats
{
    public class ChatGroup : BaseEntity
    {
        public string GroupTitle { get; set; }
        public string GroupToken { get; set; }
        public long OwnerId { get; set; }
        public string ImageName { get; set; }
        public long? ReceiverId { get; set; }
        public bool IsPrivate { get; set; }

        [ForeignKey("OwnerId")]
        public User Owner { get; set; } 

        [ForeignKey("ReceiverId")]
        public User Receiver { get; set; }

        public ICollection<Chat> Chats { get; set; } = new HashSet<Chat>();
        public ICollection<UserGroup> UserGroups { get; set; } = new HashSet<UserGroup>();
    }
}

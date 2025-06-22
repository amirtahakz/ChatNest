using ChatNest.Data.Entities;
using ChatNest.Data.Entities.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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

        #region Relation
        [ForeignKey(name: "OwnerId")]
        public User User { get; set; }
        [ForeignKey("ReceiverId")]
        public User Receiver { get; set; }
        public ICollection<Chat> Chats { get; set; }
        public ICollection<UserGroup> UserGroups { get; set; }

        #endregion
    }
}

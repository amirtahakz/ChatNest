using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using ChatNest.Data.Entities;
using ChatNest.Data.Entities.Chats;

namespace ChatNest.Data.Entities.Users
{
    public class UserGroup : BaseEntity
    {
        public long UserId { get; set; }
        public long GroupId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
        [ForeignKey("GroupId")]
        public ChatGroup ChatGroup { get; set; }
    }
}
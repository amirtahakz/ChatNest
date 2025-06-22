using ChatNest.Data.Entities.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatNest.Data.Entities.Chats
{
    public class Chat : BaseEntity
    {
        public string ChatBody { get; set; }
        public long UserId { get; set; }
        public long GroupId { get; set; }
        #region Relation
        [ForeignKey(name: "UserId")]
        public User User { get; set; }
        [ForeignKey(name: "GroupId")]
        public ChatGroup ChatGroup { get; set; }

        #endregion
    }
}

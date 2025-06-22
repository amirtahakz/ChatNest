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

        #region Relation

        [InverseProperty("User")]
        public ICollection<ChatGroup> ChatGroups { get; set; }
        [InverseProperty("Receiver")]
        public ICollection<ChatGroup> PrivateGroup { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<Chat> Chats { get; set; }
        public ICollection<UserGroup> UserGroups { get; set; }

        #endregion
    }
}

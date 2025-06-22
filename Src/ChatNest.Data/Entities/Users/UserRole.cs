using ChatNest.Data.Entities.Roles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatNest.Data.Entities.Users
{
    public class UserRole : BaseEntity
    {
        public long RoleId { get; set; }
        public long UserId { get; set; }

        #region Relation
        [ForeignKey(name: "UserId")]
        public User User { get; set; }
        [ForeignKey(name: "RoleId")]
        public Role Role { get; set; }

        #endregion
    }
}

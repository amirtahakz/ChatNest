using ChatNest.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatNest.Data.Entities.Roles
{
    public class RolePermission : BaseEntity
    {
        public long RoleId { get; set; }
        #region Relation

        public Permission Permission { get; set; }
        [ForeignKey(name: "RoleId")]
        public Role Role { get; set; }

        #endregion
    }
}

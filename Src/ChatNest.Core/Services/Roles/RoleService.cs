using ChatNest.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatNest.Core.Services.Roles
{
    public class RoleService : BaseService, IRoleService
    {
        public RoleService(TestChatContext context) : base(context)
        {
        }
    }
}

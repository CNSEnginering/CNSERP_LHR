using Abp.Authorization;
using ERP.Authorization.Roles;
using ERP.Authorization.Users;

namespace ERP.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}

using Abp.Zero.Ldap.Authentication;
using Abp.Zero.Ldap.Configuration;
using ERP.Authorization.Users;
using ERP.MultiTenancy;

namespace ERP.Authorization.Ldap
{
    public class AppLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User>
    {
        public AppLdapAuthenticationSource(ILdapSettings settings, IAbpZeroLdapModuleConfig ldapModuleConfig)
            : base(settings, ldapModuleConfig)
        {
        }
    }
}
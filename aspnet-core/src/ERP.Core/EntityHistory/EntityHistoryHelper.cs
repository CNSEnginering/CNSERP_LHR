using ERP.GeneralLedger.SetupForms;
using System;
using System.Linq;
using Abp.Organizations;
using ERP.Authorization.Roles;
using ERP.MultiTenancy;

namespace ERP.EntityHistory
{
    public static class EntityHistoryHelper
    {
        public const string EntityHistoryConfigurationName = "EntityHistory";

        public static readonly Type[] HostSideTrackedTypes =
        {
           
            typeof(OrganizationUnit), typeof(Role), typeof(Tenant)
        };

      

        public static readonly Type[] TrackedTypes =
            HostSideTrackedTypes
               
                .GroupBy(type => type.FullName)
                .Select(types => types.First())
                .ToArray();
    }
}

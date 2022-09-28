using System;
using Abp.Configuration.Startup;
using Abp.Domain.Uow;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using ERP.Configuration;
using ERP.Web;
using ERP.EntityFrameworkCore;
using ERP;
using Abp.MultiTenancy;
using Abp.Runtime.Session;
using Abp.Extensions;
using ERP.Tenants.DbPerTenant;

namespace ERP.EntityFrameworkCore
{

    public class MultiDbConnectionStringResolver : DefaultConnectionStringResolver
    {
        private readonly ICurrentUnitOfWorkProvider _currentUnitOfWorkProvider;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly IDBPerTenantAppService _IDBPerTenantAppService;
        public IAbpSession AbpSession { get; set; }

        public MultiDbConnectionStringResolver(ICurrentUnitOfWorkProvider currentUnitOfWorkProvider, IDBPerTenantAppService iDBPerTenantAppService, IAbpStartupConfiguration configuration, IHostingEnvironment hostingEnvironment)
                : base(configuration)
        {
            _currentUnitOfWorkProvider = currentUnitOfWorkProvider;
            _IDBPerTenantAppService = iDBPerTenantAppService;
            AbpSession = NullAbpSession.Instance;
            _appConfiguration =
                    AppConfigurations.Get(hostingEnvironment.ContentRootPath, hostingEnvironment.EnvironmentName);
        }

        public override string GetNameOrConnectionString(ConnectionStringResolveArgs args)
        {
            if (args["DbContextConcreteType"] as Type == typeof(INVDbContext))
            {

                var connctions = _IDBPerTenantAppService.GetAllConnections((int)GetCurrentTenantId(), "INV");

                return connctions.Result.Items[0].VConnectionString; //_appConfiguration.GetConnectionString(ERPConsts.SecondDbConnectionStringName);
            }
            if (args["DbContextConcreteType"] as Type == typeof(HRMDbContext))
            {

                var connctions = _IDBPerTenantAppService.GetAllConnections((int)GetCurrentTenantId(), "HRM");
                return connctions.Result.Items[0].VConnectionString; //_appConfiguration.GetConnectionString(ERPConsts.SecondDbConnectionStringName);
            }

            return base.GetNameOrConnectionString(args);
        }

        protected virtual int? GetCurrentTenantId()
        {
            return _currentUnitOfWorkProvider.Current != null
                ? _currentUnitOfWorkProvider.Current.GetTenantId()
                : AbpSession.TenantId;
        }
    }
}
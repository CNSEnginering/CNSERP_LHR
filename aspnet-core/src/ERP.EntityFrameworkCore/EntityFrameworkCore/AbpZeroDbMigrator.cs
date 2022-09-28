using Abp.Domain.Uow;
using Abp.EntityFrameworkCore;
using Abp.MultiTenancy;
using Abp.Zero.EntityFrameworkCore;

namespace ERP.EntityFrameworkCore
{
    public class AbpZeroDbMigrator : AbpZeroDbMigrator<ERPDbContext>
    {
        public AbpZeroDbMigrator(
            IUnitOfWorkManager unitOfWorkManager,
            IDbPerTenantConnectionStringResolver connectionStringResolver,
            IDbContextResolver dbContextResolver
            ) :
            base(
                unitOfWorkManager,
                connectionStringResolver,
                dbContextResolver
                )
        {

        }
    }
}

using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace ERP.EntityFrameworkCore
{
    public static class HRMDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<HRMDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<HRMDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace ERP.EntityFrameworkCore
{
    public static class INVDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<INVDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<INVDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
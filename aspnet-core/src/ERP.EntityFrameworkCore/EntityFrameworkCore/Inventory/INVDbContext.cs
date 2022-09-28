

using Abp.IdentityServer4;
using Microsoft.EntityFrameworkCore;
using Abp.EntityFrameworkCore;
using ERP.SupplyChain.Inventory.IC_Segment1;
using ERP.SupplyChain.Inventory.IC_Segment2;
using ERP.SupplyChain.Inventory.IC_Segment3;
using ERP.SupplyChain.Inventory.IC_Item;
using ERP.SupplyChain.Inventory.ICOPT4;
using ERP.SupplyChain.Inventory.ICOPT5;

namespace ERP.EntityFrameworkCore
{
    public class INVDbContext : AbpDbContext
    {
        //public virtual DbSet<ICSegment1> ICSegment1s { get; set; }
        //public virtual DbSet<ICSegment2> ICSegment2s { get; set; }
        //public virtual DbSet<ICSegment3> ICSegment3s { get; set; }
        //public virtual DbSet<ICItem> ICItems{ get; set; }

       // public virtual DbSet<ICOPT5> ICOPT5 { get; set; }

        //public virtual DbSet<ICOPT4> ICOPT4 { get; set; }
        public INVDbContext(DbContextOptions<INVDbContext> options)
            : base(options)
        {
            
        }


        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<ICOPT5>(i =>
            //{
            //    i.HasIndex(e => new { e.TenantId });
            //});

            //modelBuilder.Entity<ICOPT4>(i =>
            //{
            //    i.HasIndex(e => new { e.TenantId });
            //});

            //modelBuilder.Entity<ICSegment1>(g =>
            //{
            //    g.HasIndex(e => new { e.TenantId });
            //});

            //modelBuilder.Entity<ICSegment2>(g =>
            //{
            //    g.HasIndex(e => new { e.TenantId });
            //});

            //modelBuilder.Entity<ICSegment3>(g =>
            //{
            //    g.HasIndex(e => new { e.TenantId });
            //});

            //modelBuilder.Entity<ICItem>(g =>
            //{
            //    g.HasIndex(e => new { e.TenantId });
            //});
        }
    }
}

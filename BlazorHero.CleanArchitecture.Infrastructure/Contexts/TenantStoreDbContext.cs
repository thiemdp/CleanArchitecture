using BlazorHero.CleanArchitecture.Infrastructure.Models.Tenant;
using BlazorHero.CleanArchitecture.Shared.Constants.Tenant;
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Stores;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Infrastructure.Contexts
{
    public class TenantStoreDbContext : EFCoreStoreDbContext<BlazorHeroTenant>
    {
        public static readonly BlazorHeroTenant DefaultTenant = new BlazorHeroTenant() { Id = TenantConstants.DefaultTenantId, Identifier = TenantConstants.DefaultTenantId, Name = TenantConstants.DefaultTenantId,IsActive = true };
        public TenantStoreDbContext(DbContextOptions<TenantStoreDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BlazorHeroTenant>()
                .Property(t => t.ConnectionString)
                .IsRequired(false);
            modelBuilder.Entity<BlazorHeroTenant>()
                .HasData(DefaultTenant);
        }
    }
}

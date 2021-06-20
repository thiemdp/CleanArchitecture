using Finbuckle.MultiTenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using BlazorHero.CleanArchitecture.Shared.Constants.Tenant;

namespace BlazorHero.CleanArchitecture.Infrastructure.Models.Tenant
{
    public class BlazorHeroTenant : ITenantInfo
    {
        [Required]
        [MaxLength(64)]
        public string Id { get; set; }
        [MaxLength(64)]
        [Required]
        public string Identifier { get; set; }
        [MaxLength(256)]
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public TenantDBType DatabaseType  { get; set; }
        public string TenantLogo { get; set; }
        public bool IsActive { get; set; }
    }
}

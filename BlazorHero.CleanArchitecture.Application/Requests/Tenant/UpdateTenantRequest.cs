using BlazorHero.CleanArchitecture.Shared.Constants.Tenant;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Requests.Tenant
{
    public class UpdateTenantRequest
    {
        public string Id { get; set; }
        [Required]
        [MaxLength(64)]
        public string Identifier { get; set; }
        [Required]
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        //public TenantDBType DatabaseType { get; set; }
        public string TenantLogo { get; set; }
        public bool IsActive { get; set; }
    }
}

using BlazorHero.CleanArchitecture.Shared.Constants.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Responses.Tenant
{
    public class TenantResponse
    {
        public string Id { get; set; }
        public string Identifier { get; set; }
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public TenantDBType DatabaseType { get; set; }
        public string TenantLogo { get; set; }
        public bool IsActive { get; set; }
    }
}

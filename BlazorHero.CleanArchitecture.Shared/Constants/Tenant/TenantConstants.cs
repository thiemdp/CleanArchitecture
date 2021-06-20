using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Shared.Constants.Tenant
{
    public static class TenantConstants
    {
        public const string DefaultTenantId = "Master";
    }

    public enum TenantDBType
    {
        MSSQL=1,
        SQLite,
        MySQL,
        PostgreSQL
    }

}

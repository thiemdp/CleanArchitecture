namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Routes
{
    public static class TenantEndpoints
    {
        public static string CreateUpdate = "api/tenant";
        public static string InitDatabaseForAllTenant = "api/tenant/InitDatabaseForAllTenant";
        public static string GetCurrentTenant = "api/tenant/GetCurrentTenant";
        public static string GetAllPaged(int pageNumber, int pageSize,string searchString)
        {
            return $"api/tenant?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}";
        }

        public static string InitDatabaseForTenant(string Id)
        {
            return $"api/tenant/InitDatabaseForTenant/{Id}";
        }
        public static string Delete(string Id)
        {
            return $"api/tenant/{Id}";
        }
    }
}
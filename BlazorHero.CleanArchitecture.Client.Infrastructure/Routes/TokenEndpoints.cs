using BlazorHero.CleanArchitecture.Shared.Constants.Storage;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Routes
{
    public static class TokenEndpoints
    {
        public static string Refresh = "api/identity/token/refresh";

        public static string Get(string tenantIdentifier)
        {
            if (string.IsNullOrEmpty(tenantIdentifier))
                return $"api/identity/token";
            else
                return $"api/identity/token?{StorageConstants.Local.TenantKey}={tenantIdentifier}";
        }
    }
}
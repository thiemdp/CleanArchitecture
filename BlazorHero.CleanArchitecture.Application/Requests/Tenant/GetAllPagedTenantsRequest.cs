namespace BlazorHero.CleanArchitecture.Application.Requests.Tenant
{
    public class GetAllPagedTenantsRequest : PagedRequest
    {
        public string SearchString { get; set; }
    }
}
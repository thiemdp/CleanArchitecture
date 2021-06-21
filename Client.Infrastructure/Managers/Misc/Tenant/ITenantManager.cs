using BlazorHero.CleanArchitecture.Application.Requests.Tenant;
using BlazorHero.CleanArchitecture.Application.Responses.Tenant;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Managers
{
    public interface ITenantManager : IManager
    {
        Task<IResult<TenantResponse>> AddUpdateAsync(UpdateTenantRequest request);
        Task<IResult> DeleteAsync(string id);
        Task<IResult<PaginatedResult<TenantResponse>>> GetAll(GetAllPagedTenantsRequest request);
        Task InitDatabaseForAllTenant();
        Task InitDatabaseForTenant(string id);
        Task<string> GetCurrentTenantSever();
        Task<string> GetCurrentTenantLocal();
        Task RememberCurrentTenant(string identifier,bool remember);
    }
}
using BlazorHero.CleanArchitecture.Application.Requests.Tenant;
using BlazorHero.CleanArchitecture.Application.Responses.Tenant;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Interfaces.Services.Tenant
{
   public interface ITenantService
    {
        Task<IResult<PaginatedResult<TenantResponse>>> GetAllPageTenantsAsync(GetAllPagedTenantsRequest request);
        Task<TenantResponse> GetByIdAsync(string Id);
        Task<IResult<TenantResponse>> AddUpdateAsync(UpdateTenantRequest request);
        Task<IResult> DeleteAsync(string Id);
    }
}

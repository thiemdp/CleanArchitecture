using BlazorHero.CleanArchitecture.Application.Interfaces.Services;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services.Tenant;
using BlazorHero.CleanArchitecture.Application.Requests.Tenant;
using BlazorHero.CleanArchitecture.Infrastructure.Models.Tenant;
using BlazorHero.CleanArchitecture.Shared.Constants.Permission;
using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Server.Controllers.Tenant
{
    [Route("api/[controller]")]
    public class TenantController:ControllerBase
    {
        private readonly ITenantService _tenantService;
        private readonly ICurrentTenantService _currentTenantService;
        public TenantController(ITenantService tenantService,ICurrentTenantService currentTenantService )
        {
            _tenantService = tenantService;
            _currentTenantService = currentTenantService;
        }
        [Authorize(Policy = Permissions.Tenants.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll(GetAllPagedTenantsRequest request)
        {
            var result = await _tenantService.GetAllPageTenantsAsync(request);
            return Ok(result);
        }
       
        [Authorize(Policy = Permissions.Tenants.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
          var result =  await _tenantService.DeleteAsync(id);
            return Ok(result);
        }

        [Authorize(Policy = Permissions.Tenants.Edit)]
        [HttpPost]
        public async Task<IActionResult> Update([FromBody] UpdateTenantRequest request)
        {
          var res =  await _tenantService.AddUpdateAsync(request);
            await this.InitDatabaseForTenant(res.Data.Id);
            return Ok(res);
        }
        [Authorize(Policy = Permissions.Tenants.Edit)]
        [HttpGet("InitDatabaseForTenant/{id}")]
        public async Task<IActionResult> InitDatabaseForTenant(string id)
        {
            var result =  await _tenantService.GetByIdAsync(id);
            BlazorHeroTenant tenantInfo = new BlazorHeroTenant() { Id = result.Id, ConnectionString = result.ConnectionString, Identifier = result.Identifier, Name = result.Name };
            if (!string.IsNullOrEmpty(result.ConnectionString) && HttpContext.TrySetTenantInfo<BlazorHeroTenant>(tenantInfo, true))
            {
                // This will be the new tenant.
               // var tenant = HttpContext.GetMultiTenantContext<BlazorHeroTenant>().TenantInfo;
                // This will regenerate the options class.
                //var optionsProvider = HttpContext.RequestServices.GetService<IOptions<MyScopedOptions>>();
                IDatabaseSeeder sv = HttpContext.RequestServices.GetService(typeof(IDatabaseSeeder)) as IDatabaseSeeder;
                sv.InitializeForTenant();
            }
            return Ok();
        }
        [Authorize(Policy = Permissions.Tenants.Edit)]
        [HttpGet("InitDatabaseForAllTenant")]
        public async Task<IActionResult> InitDatabaseForAllTenant()
        {
            var results = await _tenantService.GetAllPageTenantsAsync(new GetAllPagedTenantsRequest() { PageSize = int.MaxValue, PageNumber = 1 }) ;
            foreach (var result in results.Data.Data)
            {
                BlazorHeroTenant tenantInfo = new BlazorHeroTenant() { Id = result.Id, ConnectionString = result.ConnectionString, Identifier = result.Identifier, Name = result.Name };
                if (!string.IsNullOrEmpty(result.ConnectionString) && HttpContext.TrySetTenantInfo<BlazorHeroTenant>(tenantInfo, true))
                {
                    // This will be the new tenant.
                    //var tenant = HttpContext.GetMultiTenantContext<BlazorHeroTenant>().TenantInfo;
                    // This will regenerate the options class.
                    //var optionsProvider = HttpContext.RequestServices.GetService<IOptions<MyScopedOptions>>();
                    IDatabaseSeeder sv = HttpContext.RequestServices.GetService(typeof(IDatabaseSeeder)) as IDatabaseSeeder;
                    sv.InitializeForTenant();
                }
            }
            return Ok();
        }

        [HttpGet("GetCurrentTenant")]
        public async Task<IActionResult> GetCurrentTenant()
        {
            return Ok(_currentTenantService.Identifier);
        }
    }
}

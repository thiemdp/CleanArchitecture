using Blazored.LocalStorage;
using BlazorHero.CleanArchitecture.Application.Requests.Tenant;
using BlazorHero.CleanArchitecture.Application.Responses.Tenant;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Extensions;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Shared.Constants.Storage;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Managers
{
    public class TenantManager : ITenantManager
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;
        public TenantManager(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }
         
        public async Task<IResult> DeleteAsync(string id)
        {
           var response = await _httpClient.DeleteAsync(Routes.TenantEndpoints.Delete(id));
            return await response.ToResult();
        }

        public async Task<IResult<PaginatedResult<TenantResponse> >> GetAll(GetAllPagedTenantsRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.TenantEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString));
            return await response.ToResult<PaginatedResult<TenantResponse>>();
        }

        public async Task InitDatabaseForAllTenant()
        {
            await _httpClient.GetAsync(Routes.TenantEndpoints.InitDatabaseForAllTenant);
        }

        public async Task InitDatabaseForTenant(string id)
        {
            await _httpClient.GetAsync(Routes.TenantEndpoints.InitDatabaseForTenant(id));
        }

        public async Task<IResult<TenantResponse>> AddUpdateAsync(UpdateTenantRequest request)
        {
             var result =  await _httpClient.PostAsJsonAsync(Routes.TenantEndpoints.CreateUpdate, request);
            return await result.ToResult<TenantResponse>();
        }

        public async Task<string> GetCurrentTenantSever()
        {
            var response = await _httpClient.GetAsync(Routes.TenantEndpoints.GetCurrentTenant);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetCurrentTenantLocal()
        {
            string tenantLocal = await _localStorageService.GetItemAsStringAsync(StorageConstants.Local.TenantKey);
            return tenantLocal;
        }

        public async Task RememberCurrentTenant(string identifier,bool remember)
        {
            if (remember)
                await _localStorageService.SetItemAsStringAsync(StorageConstants.Local.TenantKey, identifier);
            else
                await _localStorageService.RemoveItemAsync(StorageConstants.Local.TenantKey);
        }
       
    }
}
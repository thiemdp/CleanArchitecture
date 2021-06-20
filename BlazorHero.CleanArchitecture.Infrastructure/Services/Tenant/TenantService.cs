using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Extensions;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services.Tenant;
using BlazorHero.CleanArchitecture.Application.Requests.Tenant;
using BlazorHero.CleanArchitecture.Application.Responses.Tenant;
using BlazorHero.CleanArchitecture.Infrastructure.Contexts;
using BlazorHero.CleanArchitecture.Infrastructure.Models.Tenant;
using BlazorHero.CleanArchitecture.Shared.Constants.Tenant;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using Finbuckle.MultiTenant;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Infrastructure.Tenant
{
    public class TenantService : ITenantService
    {
        private readonly TenantStoreDbContext _context;
        private readonly IStringLocalizer<TenantService> _localizer;
        private readonly IMapper _mapper;

        public TenantService(TenantStoreDbContext context, IMapper mapper, IStringLocalizer<TenantService> localizer)
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }

        public async Task<IResult<TenantResponse>> AddUpdateAsync(UpdateTenantRequest request)
        {
            if (string.IsNullOrEmpty(request.Id))
            {
                if (_context.TenantInfo.Any(x => x.Identifier == request.Identifier))
                    throw new Exception(_localizer["Tenant already exists"]);
                BlazorHeroTenant tenant = _mapper.Map<BlazorHeroTenant>(request);
                tenant.Id = Guid.NewGuid().ToString();
                await _context.TenantInfo.AddAsync(tenant);
                await _context.SaveChangesAsync();
                return await Result<TenantResponse>.SuccessAsync(_mapper.Map<TenantResponse>(tenant));
            }
            else
            {
                if (request.Id.ToLower() == TenantConstants.DefaultTenantId.ToLower())
                    throw new Exception(_localizer["No permission"]);
                var tenant = _context.TenantInfo.Where(x => x.Id == request.Id).FirstOrDefault();
                if (tenant == null)
                    throw new Exception(_localizer["Tenant is not exists"]);
                _mapper.Map(request, tenant);
                await _context.SaveChangesAsync();
                return await Result<TenantResponse>.SuccessAsync(_mapper.Map<TenantResponse>(tenant));
            }
        }
        public async Task<IResult> DeleteAsync(string Id)
        {
            if (Id.ToLower() == TenantConstants.DefaultTenantId.ToLower())
                throw new Exception(_localizer["No permission"]);
            var tenant = _context.TenantInfo.Where(x => x.Id == Id).FirstOrDefault();
            if (tenant == null)
                throw new Exception(_localizer["Tenant is not exists"]);
            _context.TenantInfo.Remove(tenant);
            await _context.SaveChangesAsync();
            return await Result.SuccessAsync();
        }

        public async Task<IResult<PaginatedResult<TenantResponse>>> GetAllPageTenantsAsync(GetAllPagedTenantsRequest request)
        {
            if (request.PageNumber < 1) request.PageNumber = 1;
            if (request.PageSize < 1) request.PageSize = 10;
            var tenants = _context.TenantInfo.AsQueryable();
            if (!string.IsNullOrEmpty(request.SearchString))
                tenants = tenants.Where(x => x.Identifier.Contains(request.SearchString) || x.Name.Contains(request.SearchString));
            var result = tenants.Select(x => _mapper.Map<TenantResponse>(x));
            var data = await result.ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return await Result<PaginatedResult<TenantResponse>>.SuccessAsync(data);
        }

        public async Task<TenantResponse> GetByIdAsync(string Id)
        {
            var result = _context.TenantInfo.Where(x => x.Id == Id).FirstOrDefault();
            return _mapper.Map<TenantResponse>(result);
        }


    }
}

using BlazorHero.CleanArchitecture.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Finbuckle.MultiTenant;
using BlazorHero.CleanArchitecture.Infrastructure.Models.Tenant;

namespace BlazorHero.CleanArchitecture.Server.Services
{
    public class CurrentTenantService : ICurrentTenantService
    {
        public CurrentTenantService(IHttpContextAccessor httpContextAccessor)
        {
            var tenentinfo = httpContextAccessor.HttpContext?.GetMultiTenantContext<BlazorHeroTenant>()?.TenantInfo;
            Id = tenentinfo?.Id;
            Identifier = tenentinfo?.Identifier;
            Name = tenentinfo?.Name;
            ConnectionString = tenentinfo?.ConnectionString;
        }

        public string Id { get; }

        public string Identifier { get; }

        public string Name { get; }

        public string ConnectionString { get; }
    }
}
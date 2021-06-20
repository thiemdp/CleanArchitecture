using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Requests.Tenant;
using BlazorHero.CleanArchitecture.Application.Responses.Tenant;
using BlazorHero.CleanArchitecture.Infrastructure.Models.Tenant;

namespace BlazorHero.CleanArchitecture.Infrastructure.Mappings
{
    public class TenantProfile : Profile
    {
        public TenantProfile()
        {
            CreateMap<TenantResponse, BlazorHeroTenant>().ReverseMap();
            CreateMap<UpdateTenantRequest, BlazorHeroTenant>();
        }
    }
}
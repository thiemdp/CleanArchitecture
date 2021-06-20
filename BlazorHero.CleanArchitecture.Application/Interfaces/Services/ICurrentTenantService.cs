using BlazorHero.CleanArchitecture.Application.Interfaces.Common;

namespace BlazorHero.CleanArchitecture.Application.Interfaces.Services
{
    public interface ICurrentTenantService : IService
    {
        string Id { get; }
        string Identifier { get; }
        string Name { get; }
        string ConnectionString { get; }

    }
}
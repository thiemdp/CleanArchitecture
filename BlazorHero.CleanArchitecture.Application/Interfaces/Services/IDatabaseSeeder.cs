namespace BlazorHero.CleanArchitecture.Application.Interfaces.Services
{
    public interface IDatabaseSeeder
    {
        void Initialize();
        void InitializeForTenant();
    }
}
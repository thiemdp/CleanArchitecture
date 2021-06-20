using BlazorHero.CleanArchitecture.Application.Interfaces.Caching;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services;
using LazyCache;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Infrastructure.Caching
{
    public class BlazorHeroCache : IBlazorHeroCache
    {
        private readonly ICurrentTenantService _currentTenantService;
        private readonly IAppCache _cache;

        public BlazorHeroCache(ICurrentTenantService currentTenantService, IAppCache cache)
        {
            _currentTenantService = currentTenantService;
            _cache = cache;
        }

        private string CreateKeyWithTenant(string key)
        {
            return $"{_currentTenantService.Identifier}_{key}";
        }
        public void Add<T>(string key, T item, MemoryCacheEntryOptions policy)
        {
            _cache.Add(CreateKeyWithTenant(key), item, policy);
        }

        public T Get<T>(string key)
        {
            return _cache.Get<T>(CreateKeyWithTenant(key));
        }

        public Task<T> GetAsync<T>(string key)
        {
            return _cache.GetAsync<T>(CreateKeyWithTenant(key));
        }

        public T GetOrAdd<T>(string key, Func<T> addItemFactory)
        {
            return _cache.GetOrAdd<T>(CreateKeyWithTenant(key), addItemFactory);
        }

        public T GetOrAdd<T>(string key, Func<T> addItemFactory, MemoryCacheEntryOptions policy)
        {
            return _cache.GetOrAdd<T>(CreateKeyWithTenant(key), addItemFactory, policy);
        }

        public Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> addItemFactory)
        {
            return _cache.GetOrAddAsync<T>(CreateKeyWithTenant(key), addItemFactory);
        }

        public Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> addItemFactory, MemoryCacheEntryOptions policy)
        {
            return _cache.GetOrAddAsync<T>(CreateKeyWithTenant(key), addItemFactory, policy);
        }

        public void Remove(string key)
        {
            _cache.Remove(CreateKeyWithTenant(key));
        }

        public bool TryGetValue<T>(string key, out object value)
        {
            return _cache.TryGetValue<T>(CreateKeyWithTenant(key), out value);
        }
    }
}

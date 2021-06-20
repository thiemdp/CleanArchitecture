using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Interfaces.Caching
{
    public interface IBlazorHeroCache
    {
        void Add<T>(string key, T item, MemoryCacheEntryOptions policy);
        T Get<T>(string key);
        Task<T> GetAsync<T>(string key);
        bool TryGetValue<T>(string key, out object value);
        T GetOrAdd<T>(string key, Func<  T> addItemFactory);
        T GetOrAdd<T>(string key, Func<  T> addItemFactory, MemoryCacheEntryOptions policy);
        Task<T> GetOrAddAsync<T>(string key, Func< Task<T>> addItemFactory);
        Task<T> GetOrAddAsync<T>(string key, Func< Task<T>> addItemFactory, MemoryCacheEntryOptions policy);
        void Remove(string key);
    }
}

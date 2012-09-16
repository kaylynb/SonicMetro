using System.Collections.Concurrent;
using System.Threading.Tasks;
using SonicCache.Abstract;
using SonicCache.Interfaces;

namespace SonicCache
{
    public class MemoryCache<T> : ADataSource<T>
    {
        private bool _cacheExists = false;
        private T _cacheItem;

        public MemoryCache(IDataSource<T> dataSource) 
            : base(dataSource)
        {
        }

        public async Task<T> GetAsync()
        {
            if (!_cacheExists)
            {
                _cacheItem = await DataSource.GetAsync(SourcePolicy.Cache);
                _cacheExists = true;
            }

            return _cacheItem;
        }

        public async Task<T> RefreshAsync()
        {
            _cacheItem = await DataSource.GetAsync(SourcePolicy.Refresh);

            return _cacheItem;
        }

        public override Task<T> GetAsync(SourcePolicy sourcePolicy)
        {
            return sourcePolicy == SourcePolicy.Cache ? GetAsync() : RefreshAsync();
        }
    }

    public class MemoryCache<T, TKey> : ADataSource<T, TKey>
    {
        private readonly ConcurrentDictionary<TKey, T> _cacheItems = new ConcurrentDictionary<TKey, T>();

        public MemoryCache(IDataSource<T, TKey> dataSource)
            : base(dataSource) { }

        public Task<T> GetAsync(TKey key)
        {
            return Task.Run(async () => _cacheItems.GetOrAdd(key, await DataSource.GetAsync(key, SourcePolicy.Cache)));
        }

        public async Task<T> RefreshAsync(TKey key)
        {
            var updateItem = await DataSource.GetAsync(key, SourcePolicy.Refresh);
            _cacheItems.AddOrUpdate(key, updateItem, (_, __) => updateItem);
            return updateItem;
        }

        public override Task<T> GetAsync(TKey key, SourcePolicy sourcePolicy)
        {
            return sourcePolicy == SourcePolicy.Cache ? GetAsync(key) : RefreshAsync(key);
        }
    }
}

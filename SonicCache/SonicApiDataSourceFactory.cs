using System;
using System.Threading.Tasks;
using SonicCache.DataSource;
using SonicCache.Interfaces;
using SonicUtil.Utility;
using Windows.Storage;

namespace SonicCache
{
    public class SonicApiDataSourceFactory
    {
        public SonicApiDataSourceFactory(ISonicCache cache)
        {
            ThrowIf.Null(cache, "cache");

            _cache = cache;

            _apiCacheFolder = _cache.CacheFolder.CreateFolderAsync("apiCache", CreationCollisionOption.OpenIfExists).AsTask().Result;
        }

        private readonly StorageFolder _apiCacheFolder;
        private readonly ISonicCache _cache;

        public IDataSource<T> GenerateDefaultSonicApiDataSource<T, TRestData>(Func<TRestData, Task<T>> conversionFunc, Func<Task<Uri>> requestFunc)
        {
            ThrowIf.Null(conversionFunc, "conversionFunc");
            ThrowIf.Null(requestFunc, "requestFunc");

            return new MemoryCache<T>(
                new ConvertingDataSource<TRestData, T, Uri>(conversionFunc, requestFunc,
                    new MemoryCache<TRestData, Uri>(
                            new FileCache<TRestData, Uri, SerializableXmlObjectFileConverter<TRestData>>(_apiCacheFolder,
                                new WebDataSource<TRestData>(_cache.Query)))));
        }

        public IDataSource<T, TKey> GenerateDefaultSonicApiDataSource<T, TKey, TRestData>(Func<TRestData, Task<T>> conversionFunc, Func<TKey, Task<Uri>> requestFunc)
        {
            ThrowIf.Null(conversionFunc, "conversionFunc");
            ThrowIf.Null(requestFunc, "requestFunc");

            return new MemoryCache<T, TKey>(
                new ConvertingDataSource<TRestData, T, TKey, Uri>(conversionFunc, requestFunc,
                    new MemoryCache<TRestData, Uri>(
                        new FileCache<TRestData, Uri, SerializableXmlObjectFileConverter<TRestData>>(_apiCacheFolder,
                            new WebDataSource<TRestData>(_cache.Query)))));
        }
    }
}
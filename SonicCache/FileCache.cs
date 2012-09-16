using System;
using System.Threading.Tasks;
using SonicCache.Abstract;
using SonicCache.Interfaces;
using SonicUtil.Utility;
using Windows.Storage;

namespace SonicCache
{
    public class FileCache<T, TKey, TFileConverter> : ADataSource<T, TKey>
        where TFileConverter : IFileConverter<T, TKey>, new()
    {
        private readonly TFileConverter _fileConverter;
        private readonly StorageFolder _cacheLocation;

        public FileCache(StorageFolder cacheLocation, IDataSource<T, TKey> dataSource)
            : base(dataSource)
        {
            ThrowIf.Null(cacheLocation, "cacheLocation");

            _fileConverter = new TFileConverter();
            _cacheLocation = cacheLocation;
        }

        public override Task<T> GetAsync(TKey key, SourcePolicy sourcePolicy)
        {
            return sourcePolicy == SourcePolicy.Cache ? GetFromCacheAsync(key) : GetFromSourceAsync(key, SourcePolicy.Refresh);
        }

        private async Task<T> GetFromCacheAsync(TKey key)
        {
            try
            {
                return await _fileConverter.LoadFromFileAsync(await _cacheLocation.GetFileAsync(_fileConverter.GetFilenameFromKey(key))).ConfigureAwait(false);
            }
            catch (Exception)
            {
                // Fall through to refresh if file not found
            }

            return await GetFromSourceAsync(key, SourcePolicy.Cache);
        }

        private async Task<T> GetFromSourceAsync(TKey key, SourcePolicy sourcePolicy)
        {
            var sourceData = await DataSource.GetAsync(key, sourcePolicy).ConfigureAwait(false);
            await
                _fileConverter.SaveToFileAsync(sourceData, await _cacheLocation.CreateFileAsync(_fileConverter.GetFilenameFromKey(key), CreationCollisionOption.ReplaceExisting))
                              .ConfigureAwait(false);

            return sourceData;
        }
    }
}

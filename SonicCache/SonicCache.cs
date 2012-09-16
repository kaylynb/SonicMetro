using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using SonicCache.Data;
using SonicCache.DataSource;
using SonicCache.Interfaces;
using SonicUtil.Extensions;
using SonicUtil.Utility;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace SonicCache
{
    public class SonicCache : ISonicCache
    {
        public StorageFolder CacheFolder { get; private set; }

        public SonicCache(Guid cacheId, SonicAPI.Query query)
        {
            ThrowIf.Null(query, "query");

            Query = query;
            CacheID = cacheId;

            CacheFolder = ApplicationData.Current.LocalFolder.CreateFolderAsync(
                string.Format(CultureInfo.InvariantCulture, "cache\\{0}", CacheID), CreationCollisionOption.OpenIfExists).AsTask().Result;

            var factory = new SonicApiDataSourceFactory(this);

            MusicFolders = factory.GenerateDefaultSonicApiDataSource<IEnumerable<MusicFolder>, SonicAPI.RESTSchema.MusicFolders>(
                x => Task.FromResult(x.musicFolder.Select(y => new MusicFolder(this, y.id.ToString(CultureInfo.InvariantCulture), y.name))),
                () => Task.FromResult(Query.GetMusicFoldersQuery()));

            Indexes = factory.GenerateDefaultSonicApiDataSource<IEnumerable<Index>, string, SonicAPI.RESTSchema.Indexes>(
                x => Task.FromResult(x.index.Select(index => new Index(this, index.name, index.artist.Select(a => a.id).ToList()))),
                x => Task.FromResult(Query.GetIndexesQuery(x)));

            MusicDirectory = factory.GenerateDefaultSonicApiDataSource<MusicDirectory, string, SonicAPI.RESTSchema.Directory>(
                x => Task.FromResult(new MusicDirectory(this, x.id, x.name, x.child)),
                x => Task.FromResult(Query.GetMusicDirectoryQuery(x)));

            CoverArt = new MemoryCache<BitmapImage, string>(
                new ConvertingDataSource<byte[], BitmapImage, string, string>(
                    x => x.ConvertToBitmapImageAsync(),
                    Task.FromResult,
                        new FileCache<byte[], string, ByteArrayFileConverter>(CacheFolder.CreateFolderAsync("coverArt", CreationCollisionOption.OpenIfExists).AsTask().Result,
                            new CoverArtDataSource(Query))));
        }

        public SonicAPI.Query Query { get; private set; }
        public Guid CacheID { get; private set; }

        public IDataSource<IEnumerable<MusicFolder>> MusicFolders { get; private set; }
        public IDataSource<IEnumerable<Index>, string> Indexes { get; set; }
        public IDataSource<MusicDirectory, string> MusicDirectory { get; private set; }
        public IDataSource<BitmapImage, string> CoverArt { get; private set; }
    }
}
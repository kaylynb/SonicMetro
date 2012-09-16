using System;
using System.Collections.Generic;
using SonicCache.Data;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace SonicCache.Interfaces
{
    public interface ISonicCache
    {
        Guid CacheID { get; }
        SonicAPI.Query Query { get; }
        StorageFolder CacheFolder { get; }

        IDataSource<IEnumerable<MusicFolder>> MusicFolders { get; }
        IDataSource<IEnumerable<Index>, string> Indexes { get; }
        IDataSource<MusicDirectory, string> MusicDirectory { get; }
        IDataSource<BitmapImage, string> CoverArt { get; }
    }
}
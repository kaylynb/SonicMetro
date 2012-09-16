using System.Threading.Tasks;
using Windows.Storage;

namespace SonicCache.Interfaces
{
    public interface IFileConverter<T>
    {
        Task<T> LoadFromFileAsync(StorageFile file);
        Task SaveToFileAsync(T data, StorageFile file);

        string GetFilename();
    }

    public interface IFileConverter<T, TKey>
    {
        Task<T> LoadFromFileAsync(StorageFile file);
        Task SaveToFileAsync(T data, StorageFile file);

        string GetFilenameFromKey(TKey key);
    }
}

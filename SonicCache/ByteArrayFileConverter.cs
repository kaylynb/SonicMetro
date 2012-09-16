using System.Threading.Tasks;
using SonicCache.Interfaces;
using SonicUtil.Extensions;
using Windows.Storage;

namespace SonicCache
{
    public class ByteArrayFileConverter : IFileConverter<byte[], string>
    {
        public Task<byte[]> LoadFromFileAsync(StorageFile file)
        {
            return file.GetByteArray();
        }

        public Task SaveToFileAsync(byte[] data, StorageFile file)
        {
            return data.SaveToFileAsync(file);
        }

        public string GetFilenameFromKey(string key)
        {
            return key;
        }
    }
}

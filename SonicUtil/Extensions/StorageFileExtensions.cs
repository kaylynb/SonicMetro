using System;
using System.Threading.Tasks;
using SonicUtil.Utility;
using Windows.Storage;
using Windows.Storage.Streams;

namespace SonicUtil.Extensions
{
    public static class StorageFileExtensions
    {
        public static async Task<byte[]> GetByteArray(this StorageFile file)
        {
            ThrowIf.Null(file, "file");

            byte[] ret;
            using (var stream = await file.OpenReadAsync())
            {
                ret = new byte[stream.Size];
                using (var reader = new DataReader(stream))
                {
                    await reader.LoadAsync((uint) stream.Size);
                    reader.ReadBytes(ret);
                }
            }

            return ret;
        }
    }
}
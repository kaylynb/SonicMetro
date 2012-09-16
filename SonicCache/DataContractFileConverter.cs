using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using SonicCache.Data;
using SonicCache.Interfaces;
using SonicUtil.Utility;
using Windows.Storage;

namespace SonicCache
{
    public class DataContractFileConverter : IFileConverter<MusicFolder, string>
    {
        public async Task<MusicFolder> LoadFromFileAsync(StorageFile file)
        {
            ThrowIf.Null(file, "file");

            var buf = await FileIO.ReadTextAsync(file);
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(buf)))
            {
                var serializer = new DataContractJsonSerializer(typeof (MusicFolder));

                return (MusicFolder) serializer.ReadObject(ms);
            }
        }

        public async Task SaveToFileAsync(MusicFolder data, StorageFile file)
        {
            ThrowIf.Null(data, "data");
            ThrowIf.Null(file, "file");

            var serializer = new DataContractJsonSerializer(typeof (MusicFolder));
            using (var ms = new MemoryStream())
            {
                serializer.WriteObject(ms, data);
                using (var s = new StreamReader(ms))
                {
                    ms.Position = 0;
                    await FileIO.WriteTextAsync(file, s.ReadToEnd());
                }
            }
        }

        public string GetFilenameFromKey(string key)
        {
            return key;
        }
    }
}
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using SonicCache.Interfaces;
using SonicUtil.Utility;
using Windows.Storage;
using UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding;

namespace SonicCache
{
    class SerializableXmlObjectFileConverter<T> : IFileConverter<T, Uri>
    {
        private readonly XmlSerializer _serializer = new XmlSerializer(typeof(T));

        public async Task<T> LoadFromFileAsync(StorageFile file)
        {
            ThrowIf.Null(file, "file");

            using (var ms = new MemoryStream())
            {
                var text = await FileIO.ReadTextAsync(file, UnicodeEncoding.Utf8);
                using (var sr = new StreamWriter(ms, Encoding.UTF8, text.Length))
                {
                    sr.Write(text);
                    await sr.FlushAsync();

                    ms.Position = 0;
                    return (T)_serializer.Deserialize(ms);
                }
            }
        }

        public async Task SaveToFileAsync(T data, StorageFile file)
        {
            ThrowIf.Null(file, "file");

            using (var ms = new MemoryStream())
            {
                _serializer.Serialize(ms, data);
                using (var sr = new StreamReader(ms))
                {
                    ms.Position = 0;
                    await FileIO.WriteTextAsync(file, sr.ReadToEnd(), UnicodeEncoding.Utf8);
                }
            }
        }

        public string GetFilenameFromKey(Uri key)
        {
            return key.GetFileHash();
        }
    }
}
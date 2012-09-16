using System;
using System.Threading.Tasks;
using SonicUtil.Utility;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml.Media.Imaging;

namespace SonicUtil.Extensions
{
    public static class ByteArrayExtensions
    {
        public static async Task<BitmapImage> ConvertToBitmapImageAsync(this byte[] byteArray)
        {
            ThrowIf.Null(byteArray, "byteArray");

            using (var stream = new InMemoryRandomAccessStream())
            {
                using (var writer = new DataWriter(stream.GetOutputStreamAt(0)))
                {
                    writer.WriteBytes(byteArray);
                    await writer.StoreAsync();
                    writer.DetachStream();
                }

                BitmapImage image = null;
                // NOTE: ~HUGE HACK~  BitmapImage can only be created/manipulated on UI thread :(
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                    CoreDispatcherPriority.Normal,
                    () =>
                        {
                            image = new BitmapImage();
                            image.SetSource(stream);
                        });

                return image;

                /*Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.Invoke(Windows.UI.Core.CoreDispatcherPriority.Normal, (s, a) => {
    if (_image == null)
        {
            IRandomAccessStream imageStream = await _imageFile.OpenAsync(FileAccessMode.Read);
            _image = new BitmapImage();
            _image.DecodePixelHeight = _decodeHeight;
            _image.SetSource(imageStream);
        }
}, null, null);

return _image;*/
                /* var image = new BitmapImage();
                await image.SetSourceAsync(stream);
                return image;*/
            }
        }

        public static async Task SaveToFileAsync(this byte[] byteArray, StorageFile file)
        {
            ThrowIf.Null(byteArray, "byteArray");
            ThrowIf.Null(file, "file");

            using (var filestream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                using (var writer = new DataWriter(filestream.GetOutputStreamAt(0)))
                {
                    writer.WriteBytes(byteArray);
                    await writer.StoreAsync();
                    writer.DetachStream();
                }

                await filestream.FlushAsync();
            }
        }
    }
}
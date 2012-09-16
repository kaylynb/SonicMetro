using System;
using SonicUtil.Utility;
using Windows.UI.Xaml.Controls;

namespace SonicMetro.MediaPlayer
{
    public class MediaPlayer : IMediaPlayer
    {
        private readonly MediaElement _mediaElement;

        public MediaPlayer(MediaElement mediaElement)
        {
            ThrowIf.Null(mediaElement, "mediaElement");

            _mediaElement = mediaElement;
        }

        public void Play()
        {
            _mediaElement.Source = new Uri("source");
        }
    }
}
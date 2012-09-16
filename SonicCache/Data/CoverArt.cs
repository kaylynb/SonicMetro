using System.Threading.Tasks;
using SonicCache.Interfaces;
using Windows.UI.Xaml.Media.Imaging;

namespace SonicCache.Data
{
    public class CoverArt : ASubsonicData<CoverArt>
    {
        public CoverArt(ISonicCache cache)
            : this(cache, null) { }

        public CoverArt(ISonicCache cache, string coverArtId)
            : base(cache)
        {
            _covertArtId = coverArtId;
        }

        private readonly string _covertArtId;

        public bool HasCoverArtId
        {
            get { return !string.IsNullOrEmpty(_covertArtId); }
        }

        private bool _imageLoaded;
        private BitmapImage _image;
        public BitmapImage Image
        {
            get
            {
                if (!_imageLoaded)
                {
                    _imageLoaded = true;
                    GetAsync(SourcePolicy.Cache);
                }
                return _image;
            }
            set
            {
                SetProperty(ref _image, value);
            }
        }

        public async override Task GetAsync(SourcePolicy sourcePolicy)
        {
            if (string.IsNullOrEmpty(_covertArtId))
                return;

            Image = await Cache.CoverArt.GetAsync(_covertArtId, sourcePolicy);
        }

        public override void Merge(CoverArt other)
        {
            if (ReferenceEquals(this, other))
                return;

            Image = other.Image;
            _imageLoaded = other._imageLoaded;
        }

        public override bool Equals(CoverArt other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return _covertArtId == other._covertArtId;
        }

        public override bool Equals(object obj)
        {
            var coverArt = obj as CoverArt;

            return coverArt != null && Equals(coverArt);
        }

        public override int GetHashCode()
        {
            return _covertArtId.GetHashCode();
        }
    }
}

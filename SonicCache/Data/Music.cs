using System.Threading.Tasks;
using SonicCache.Interfaces;
using SonicUtil.Utility;

namespace SonicCache.Data
{
    public sealed class Music : ASubsonicData<Music>
    {
        public Music(ISonicCache cache, string id, string coverArtId, string name) 
            : base(cache)
        {
            ThrowIf.NullOrEmpty(id, "id");
            ThrowIf.NullOrEmpty(name, "name");

            Name = name;
            ID = id;

            Cover = new CoverArt(cache, coverArtId);
        }

        public string ID { get; private set; }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private CoverArt _cover;
        public CoverArt Cover
        {
            get { return _cover; }
            set { SetProperty(ref _cover, value); }
        }

        public override Task GetAsync(SourcePolicy sourcePolicy)
        {
            return Task.FromResult(0);
        }

        public override void Merge(Music other)
        {
            if (ReferenceEquals(this, other))
                return;

            if (!Name.Equals(other.Name))
                Name = other.Name;

            if (!Cover.Equals(other.Cover))
                Cover = other.Cover;
        }

        public override bool Equals(Music other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return ID.Equals(other.ID);
        }

        public override bool Equals(object obj)
        {
            var music = obj as Music;

            return music != null && Equals(music);
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }
    }
}
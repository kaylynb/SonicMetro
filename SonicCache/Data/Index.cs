using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using SonicCache.Interfaces;
using SonicUtil.Extensions;
using SonicUtil.Utility;

namespace SonicCache.Data
{
    public sealed class Index : ASubsonicData<Index>
    {
        public Index(ISonicCache cache, string name, IList<string> artistIDs)
            : base(cache)
        {
            ThrowIf.NullOrEmpty(name, "name");
            ThrowIf.Null(artistIDs, "artistIDs");

            Name = name;
            ArtistsIDs = artistIDs;
            MusicDirectories = new ObservableCollection<MusicDirectory>();
            Cover = new CoverArt(cache);
        }

        private IList<string> ArtistsIDs { get; set; }

        public string Name { get; private set; }

        private CoverArt _cover;
        public CoverArt Cover
        {
            get { return _cover; }
            set { SetProperty(ref _cover, value); }
        }

        public ObservableCollection<MusicDirectory> MusicDirectories { get; private set; }

        public async override Task GetAsync(SourcePolicy sourcePolicy)
        {
            var newArtists = await ArtistsIDs.SelectAsync(async x => await Cache.MusicDirectory.GetAsync(x, sourcePolicy));

            MusicDirectories.StableMerge(newArtists);

            await MusicDirectories.ForEachAsync(x => x.GetAsync(sourcePolicy));

            var coverArt = MusicDirectories.FirstOrDefault(x => x.Cover.HasCoverArtId);
            if (coverArt != null)
                Cover = coverArt.Cover;
        }

        public override void Merge(Index other)
        {
            ArtistsIDs.StableMerge(other.ArtistsIDs);

            MusicDirectories.StableMerge(other.MusicDirectories);
        }

        public override bool Equals(Index other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other)) 
                return true;

            return Name.Equals(other.Name);
        }

        public override bool Equals(object obj)
        {
            var index = obj as Index;

            return index != null && Equals(index);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using SonicCache.Interfaces;
using SonicUtil.Extensions;
using SonicUtil.Utility;

namespace SonicCache.Data
{
    public sealed class MusicDirectory : ASubsonicData<MusicDirectory>
    {
        public MusicDirectory(ISonicCache cache, string id, string name, IEnumerable<SonicAPI.RESTSchema.Child> children)
            : base(cache)
        {
            ThrowIf.NullOrEmpty(id, "id");
            ThrowIf.NullOrEmpty(name, "name");
            ThrowIf.Null(children, "children");

            Name = name;
            ID = id;

            Directories = new ObservableCollection<MusicDirectory>();
            Music = new ObservableCollection<Music>();
            Cover = new CoverArt(cache);

            _children = children.ToList();
        }

        private readonly List<SonicAPI.RESTSchema.Child> _children;

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

        public ObservableCollection<MusicDirectory> Directories { get; private set; }
        public ObservableCollection<Music> Music { get; private set; } 

        public async override Task GetAsync(SourcePolicy sourcePolicy)
        {
            var newDirectories = await _children.Where(x => x.isDir)
                .SelectAsync(async x => await Cache.MusicDirectory.GetAsync(x.id, sourcePolicy));
            Directories.StableMergeUpdate(newDirectories);
            await Directories.ForEachAsync(x => x.GetAsync(sourcePolicy));

            var newMusic = _children.Where(x => !x.isDir && x.type == SonicAPI.RESTSchema.MediaType.music)
                                          .Select(x => new Music(Cache, x.id, x.coverArt, x.title));

            Music.StableMergeUpdate(newMusic);

            var musicArt = Music.FirstOrDefault(x => x.Cover.HasCoverArtId);
            if (musicArt != null)
                Cover = musicArt.Cover;
            else
            {
                var directoryArt = Directories.FirstOrDefault(x => x.Cover.HasCoverArtId);
                if (directoryArt != null)
                    Cover = directoryArt.Cover;
            }

        }

        public override void Merge(MusicDirectory other)
        {
            if (ReferenceEquals(this, other))
                return;

            if (!Name.Equals(other.Name))
                Name = other.Name;

           Directories.StableMergeUpdate(other.Directories);
        }

        public override bool Equals(MusicDirectory other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return ID.Equals(other.ID);
        }

        public override bool Equals(object obj)
        {
            var musicDirectory = obj as MusicDirectory;

            return musicDirectory != null && Equals(musicDirectory);
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }
    }
}
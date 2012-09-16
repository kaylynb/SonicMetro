using System.Collections.ObjectModel;
using System.Threading.Tasks;
using SonicCache.Interfaces;
using SonicUtil.Extensions;
using SonicUtil.Utility;

namespace SonicCache.Data
{
    public sealed class MusicFolder : ASubsonicData<MusicFolder>
    {
        public MusicFolder(ISonicCache cache, string id, string name)
            : base(cache)
        {
            ThrowIf.NullOrEmpty(id, "id");
            ThrowIf.NullOrEmpty(name, "name");

            Indexes = new ObservableCollection<Index>();

            ID = id;
            Name = name;
        }

        public ObservableCollection<Index> Indexes { get; private set; }

        public string ID { get; private set; }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public async override Task GetAsync(SourcePolicy sourcePolicy)
        {
            Indexes.StableMergeUpdate((await Cache.Indexes.GetAsync(ID, sourcePolicy)));

            await Indexes.ForEachAsync(async x => await x.GetAsync(sourcePolicy));
        }

        public override void Merge(MusicFolder other)
        {
            if (ReferenceEquals(this, other))
                return;

            if (!Name.Equals(other.Name))
                Name = other.Name;
        }

        public override bool Equals(MusicFolder other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return ID == other.ID;
        }

        public override bool Equals(object obj)
        {
            var musicFolder = obj as MusicFolder;

            return musicFolder != null && Equals(musicFolder);
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }
    }
}

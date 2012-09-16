using System.Collections.ObjectModel;
using System.Threading.Tasks;
using SonicCache.Interfaces;
using SonicUtil.Extensions;

namespace SonicCache.Data
{
    public sealed class MusicFolders : ASubsonicData<MusicFolders>
    {
        public MusicFolders(ISonicCache cache)
            : base(cache)
        {
            Folders = new ObservableCollection<MusicFolder>();
        }

        public ObservableCollection<MusicFolder> Folders { get; private set; }

        public async override Task GetAsync(SourcePolicy sourcePolicy)
        {
            Folders.StableMergeUpdate((await Cache.MusicFolders.GetAsync(sourcePolicy)));

            await Folders.ForEachAsync(async x => await x.GetAsync(sourcePolicy));
        }

        public override void Merge(MusicFolders other)
        {
            //throw new NotImplementedException();
        }

        public override bool Equals(MusicFolders other)
        {
            return true;
        }

        public override bool Equals(object obj)
        {
            var musicFolders = obj as MusicFolders;

            return musicFolders != null;
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}
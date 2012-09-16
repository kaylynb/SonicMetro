using System.Collections.ObjectModel;
using System.Linq;
using Arbitrary;
using GalaSoft.MvvmLight.Command;
using SonicCache.Data;
using SonicCache.Interfaces;
using SonicMetro.ViewModel.Navigation;
using SonicMetro.Views;
using SonicUtil.Utility;
using SonicUtil.ViewModel;

namespace SonicMetro.ViewModel
{
    public class IndexesViewModel : ViewModelBase
    {
        public IndexesViewModel(ISonicCache cache)
        {
            LoadIndexesOperation = new LongRunningOperation();

            LoadIndexesAsync(cache);

            RefreshCommand = new RelayCommand(async () =>
                {
                    ArbitraryContainer.Default.Resolve<INavigationService>().ClearPageCache();

                    var x = new MusicFolders(cache);
                    await x.GetAsync(SourcePolicy.Refresh);

                    Indexes.StableMergeUpdate(x.Folders.First().Indexes);
                });

            DirectoryClickedCommand = new RelayCommand<MusicDirectory>(
                x => ArbitraryContainer.Default.Resolve<INavigationService>().Navigate(typeof(MusicDirectoryView), x));
        }

        public LongRunningOperation LoadIndexesOperation { get; private set; }

        public RelayCommand RefreshCommand { get; private set; }
        public RelayCommand<MusicDirectory> DirectoryClickedCommand { get; private set; }

        private ObservableCollection<Index> _indexes = new ObservableCollection<Index>();
        public ObservableCollection<Index> Indexes
        {
            get { return _indexes; }
            set { SetProperty(ref _indexes, value); }
        }

        public async void LoadIndexesAsync(ISonicCache cache)
        {
            var x = new MusicFolders(cache);
            await LoadIndexesOperation.RunAsync(() => x.GetAsync(SourcePolicy.Cache), e => { });


            Indexes.StableMergeUpdate(x.Folders.First().Indexes);
        }
    }
}

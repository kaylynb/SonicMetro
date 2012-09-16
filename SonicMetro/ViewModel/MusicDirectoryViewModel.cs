using Arbitrary;
using GalaSoft.MvvmLight.Command;
using SonicCache.Data;
using SonicMetro.ViewModel.Navigation;
using SonicMetro.Views;
using SonicUtil.ViewModel;

namespace SonicMetro.ViewModel
{
    public class MusicDirectoryViewModel : ViewModelBase
    {
        public MusicDirectoryViewModel()
        {
            DirectoryClickedCommand = new RelayCommand<MusicDirectory>(
            x => ArbitraryContainer.Default.Resolve<INavigationService>().Navigate(typeof(MusicDirectoryView), x));
        }

        public override void Initialize(object parameter)
        {
            var directory = parameter as MusicDirectory;

            if (directory != null)
                Directory = directory;
        }

        public RelayCommand<MusicDirectory> DirectoryClickedCommand { get; private set; }

        private MusicDirectory _directory;
        public MusicDirectory Directory
        {
            get { return _directory; }
            set { SetProperty(ref _directory, value); }
        }
    }
}

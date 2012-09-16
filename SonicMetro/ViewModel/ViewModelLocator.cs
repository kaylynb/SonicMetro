using Arbitrary;
using GalaSoft.MvvmLight;

namespace SonicMetro.ViewModel
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            if (ViewModelBase.IsInDesignModeStatic)
            {
               /* ArbitraryContainer.Default.Register<ISubsonicLicenseStatus, DesignSubsonicLicenseStatus>();
                ArbitraryContainer.Default.Register<ISubsonicArtistsID3, DesignSubsonicArtistsID3>();*/
            }
            else
            {
           /*     ArbitraryContainer.Default.Register<ISubsonicLicenseStatus, SubsonicLicenseStatus>();
                ArbitraryContainer.Default.Register<ISubsonicArtistsID3, SubsonicArtistsID3>();*/
            }
        }

        public IndexesViewModel Indexes { get { return ArbitraryContainer.Default.Resolve<IndexesViewModel>(); } }
        public MusicDirectoryViewModel MusicDirectory { get { return ArbitraryContainer.Default.Resolve<MusicDirectoryViewModel>(); } }
    }
}
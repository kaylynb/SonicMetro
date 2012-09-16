using System;

namespace SonicMetro.ViewModel.Navigation
{
    public interface INavigationService
    {
        void Navigate(Type page);
        void Navigate(Type page, object parameter);

        bool CanGoBack { get; }
        bool CanGoForward { get; }

        void GoBack();
        void GoForward();

        void ClearPageCache();
    }
}

using System;
using SonicMetro.Common;
using SonicUtil.Utility;
using SonicUtil.ViewModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace SonicMetro.ViewModel.Navigation
{
    public class FrameNavigationService : INavigationService
    {
        private readonly Frame _frame;

        public FrameNavigationService(Frame frame)
        {
            ThrowIf.Null(frame, "frame");
            _frame = frame;
            _frame.Navigated += OnFrameNavigated;
        }

        public void Navigate(Type page)
        {
            _frame.Navigate(page);
        }

        public void Navigate(Type page, object parameter)
        {
            _frame.Navigate(page, parameter);
        }

        public bool CanGoBack
        {
            get { return _frame.CanGoBack; }
        }

        public bool CanGoForward
        {
            get { return _frame.CanGoForward; }
        }

        public void GoBack()
        {
            _frame.GoBack();
        }

        public void GoForward()
        {
            _frame.GoForward();
        }

        public void ClearPageCache()
        {
            var cacheSize = _frame.CacheSize;
            _frame.CacheSize = 0;
            _frame.CacheSize = cacheSize;
        }

        private void OnFrameNavigated(object sender, NavigationEventArgs args)
        {
            var page = args.Content as LayoutAwarePage;

            if (page == null)
                return;

            var viewModel = page.DataContext as ViewModelBase;

            if (viewModel == null)
                return;

            viewModel.Initialize(args.Parameter);
        }
    }
}

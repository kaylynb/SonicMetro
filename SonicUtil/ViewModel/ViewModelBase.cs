using SonicUtil.Utility;

namespace SonicUtil.ViewModel
{
    public class ViewModelBase : BindableBase
    {
        public virtual void Initialize(object parameter) { }

        private static bool? _isInDesignMode;

        public static bool IsInDesignMode
        {
            get
            {
                if (!_isInDesignMode.HasValue)
                {
                    _isInDesignMode = Windows.ApplicationModel.DesignMode.DesignModeEnabled;
                }

                return _isInDesignMode.Value;
            }
        }
    }
}
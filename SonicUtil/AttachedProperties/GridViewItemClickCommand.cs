using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SonicUtil.AttachedProperties
{
    public class GridViewItemClickCommand
    {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached(
            "Command",
            typeof (ICommand),
            typeof (GridViewItemClickCommand),
            new PropertyMetadata(null, CommandPropertyChanged));

        public static void SetCommand(DependencyObject attached, ICommand value)
        {
            attached.SetValue(CommandProperty, value);
        }

        public static ICommand GetCommand(DependencyObject attached)
        {
            return (ICommand) attached.GetValue(CommandProperty);
        }

        private static void CommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
// ReSharper disable PossibleNullReferenceException
            (d as GridView).ItemClick += gridView_ItemClick;
// ReSharper restore PossibleNullReferenceException
        }

// ReSharper disable InconsistentNaming
        private static void gridView_ItemClick(object sender, ItemClickEventArgs e)
// ReSharper restore InconsistentNaming
        {
            GetCommand(sender as GridView).Execute(e.ClickedItem);
        }
    }
}

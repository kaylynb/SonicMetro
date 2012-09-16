using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace SonicControls
{
    [TemplatePart(Name = BufferRect, Type = typeof(Rectangle))]
    public sealed class BufferSlider : Slider
    {
        private const string BufferRect = "HorizontalBufferRect";

        private Rectangle _horizontalBufferRectangle;

        public BufferSlider()
        {
            DefaultStyleKey = typeof(BufferSlider);
        }

        protected override void OnApplyTemplate()
        {
            _horizontalBufferRectangle = GetTemplateChild(BufferRect) as Rectangle;

            this.SizeChanged += (_, args) =>
            {
                if (args.NewSize.Width != args.PreviousSize.Width)
                    UpdateControls();
            };

            UpdateControls();

            base.OnApplyTemplate();
        }

        public static readonly DependencyProperty BufferProperty = DependencyProperty.Register(
            "Buffer", typeof(double), typeof(BufferSlider), new PropertyMetadata(0.0, OnBufferChanged));

        public double Buffer
        {
            get { return (double)GetValue(BufferProperty); }
            set { SetValue(BufferProperty, value); }
        }

        private static void OnBufferChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var bufferSlider = sender as BufferSlider;
            if (bufferSlider != null)
                bufferSlider.UpdateControls();
        }

        private void UpdateControls()
        {
            if (_horizontalBufferRectangle != null)
            {
                var maxMin = Maximum - Minimum;
                if (maxMin == 0.0)
                    _horizontalBufferRectangle.Width = 0;
                else
                    _horizontalBufferRectangle.Width = (ActualWidth / maxMin) * Buffer;
            }
        }
    }
}

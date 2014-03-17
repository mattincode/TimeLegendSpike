using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TimeLegendSpike
{
    public partial class TopLegendControl : UserControl
    {
        private SolidColorBrush _fontBrush = new SolidColorBrush(Colors.Black);
        #region Dependency properties
        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(IEnumerable<TopLegendItem>), typeof(UserControl), new PropertyMetadata(new PropertyChangedCallback(ItemsChanged)));
        public IEnumerable<TopLegendItem> Items
        {
            get { return (IEnumerable<TopLegendItem>)GetValue(ItemsProperty); }
            set
            {
                SetValue(ItemsProperty, value);
            }
        }
        #endregion

        public TopLegendControl()
        {
            InitializeComponent();

            // Subscribe to change notifications
            this.SizeChanged += TopLegendControl_SizeChanged;
        }

        private static void ItemsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var control = sender as TopLegendControl;
            if (control != null)
            {
                control.DrawLegend();   
            }
        }

        void TopLegendControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DrawLegend();
        }

        public void DrawLegend()
        {
            double controlWidth = LayoutRoot.ActualWidth;
            double controlHeight = LayoutRoot.ActualHeight;

            // Sanity check
            if (Items == null || double.IsNaN(controlWidth))
            {
                LayoutRoot.Children.Clear();
                Debug.WriteLine("#TopLegendControl.DrawLegend Width: NaN");
                return;
            }
            double verticalPosition = controlHeight/2;

            LayoutRoot.Children.Clear();
            foreach (var legendItem in Items)
            {
                // Check if legend item fits
                if (legendItem.Left >= controlWidth)
                    continue;

                TextHelper.DrawText(LayoutRoot.Children, new Point(legendItem.Left, verticalPosition), legendItem.MaxWidth, legendItem.Text, _fontBrush, height: controlHeight);
            }

        }
    }

    #region TopLegendItem
    public class TopLegendItem
    {
        public string Text { get; set; }
        public double Left { get; set; }
        public double MaxWidth { get; set; }
    }
    #endregion
}

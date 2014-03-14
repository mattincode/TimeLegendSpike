using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TimeLegendSpike.Helpers;

namespace TimeLegendSpike
{
    public partial class TopLegendControl : UserControl
    {
        private SolidColorBrush _fontBrush = new SolidColorBrush(Colors.Black);
        #region Dependency properties
        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(IEnumerable<TopLegendItem>), typeof(UserControl), new PropertyMetadata(null));
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
            DependencyPropertyChangedListener itemsChangedListener = DependencyPropertyChangedListener.Create(this, "Items");
            itemsChangedListener.ValueChanged += itemsChangedListener_ValueChanged;
            this.SizeChanged += TopLegendControl_SizeChanged;
        }

        void TopLegendControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DrawLegend();
        }

        void itemsChangedListener_ValueChanged(object sender, DependencyPropertyValueChangedEventArgs e)
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

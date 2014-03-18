using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using TimeLegendSpike.ViewModels;

namespace TimeLegendSpike
{
    public partial class DrawingAreaControl : UserControl
    {
        #region Dependency properties
        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(ObservableCollection<TopLegendItem>), typeof(UserControl), new PropertyMetadata(null));
        public ObservableCollection<TopLegendItem> Items
        {
            get { return (ObservableCollection<TopLegendItem>)GetValue(ItemsProperty); }
            set
            {
                SetValue(ItemsProperty, value);
            }
        }
        #endregion

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            var vm = DataContext as MainViewModel;


            List<TopLegendItem> items = new List<TopLegendItem>();
            double currentPosition = 10;
            const int spacingPx = 50;
            for (int i = 0; i < 5; i++)
            {
                items.Add(new TopLegendItem() { Text = "LegendItem" + i, Left = currentPosition, MaxWidth = 50 });
                currentPosition += spacingPx;
            }

            Items = new ObservableCollection<TopLegendItem>(items);
        }

        public DrawingAreaControl()
        {
            InitializeComponent();



        }
    }
}

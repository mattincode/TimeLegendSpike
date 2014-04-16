using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
        public static readonly DependencyProperty DrawingAreaViewModelProperty = DependencyProperty.Register("DrawingAreaViewModel", typeof(DrawingAreaViewModel), typeof(UserControl), new PropertyMetadata(null));
        public DrawingAreaViewModel DrawingAreaViewModel
        {
            get { return (DrawingAreaViewModel)GetValue(DrawingAreaViewModelProperty); }
            set
            {
                SetValue(DrawingAreaViewModelProperty, value);
            }
        }

        public static readonly DependencyProperty PeriodStartProperty = DependencyProperty.Register("PeriodStart", typeof(DateTime), typeof(UserControl), new PropertyMetadata(null));
        public DateTime PeriodStart
        {
            get { return (DateTime)GetValue(PeriodStartProperty); }
            set
            {
                SetValue(PeriodStartProperty, value);
            }
        }

        public static readonly DependencyProperty PeriodEndProperty = DependencyProperty.Register("PeriodEnd", typeof(DateTime), typeof(UserControl), new PropertyMetadata(null));
        public DateTime PeriodEnd
        {
            get { return (DateTime)GetValue(PeriodEndProperty); }
            set
            {
                SetValue(PeriodEndProperty, value);
            }
        }
        #endregion

        //protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        //{
        //    base.OnMouseLeftButtonDown(e);
        //    var vm = DataContext as DrawingAreaViewModel;


        //    List<TopLegendItem> items = new List<TopLegendItem>();
        //    double currentPosition = 10;
        //    const int spacingPx = 50;
        //    for (int i = 0; i < 5; i++)
        //    {
        //        items.Add(new TopLegendItem() { Text = "LegendItem" + i, Left = currentPosition, MaxWidth = 50 });
        //        currentPosition += spacingPx;
        //    }

        //    Items = new ObservableCollection<TopLegendItem>(items);
        //}

        //private DrawingAreaViewModel _vm = new DrawingAreaViewModel();

        public DrawingAreaControl()
        {
            InitializeComponent();
        }

        //private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        //{
        //    foreach (CheckPoint checkPoint in _vm.Terminal.CheckPoints)
        //    {
        //        var maxX = checkPoint.Bookings.Max(x => x.X);
        //        checkPoint.Width = maxX + 24;
        //    }
        //    Checks.Visibility = Visibility.Visible;

        //}
    }

    public class CanvasItemsControl : System.Windows.Controls.ItemsControl
    {
        protected override void PrepareContainerForItemOverride(
                            DependencyObject element,
                            object item)
        {
            Binding leftBinding = new Binding() { Path = new PropertyPath("X") };
            Binding topBinding = new Binding() { Path = new PropertyPath("Y") };

            FrameworkElement contentControl = (FrameworkElement)element;
            contentControl.SetBinding(Canvas.LeftProperty, leftBinding);
            contentControl.SetBinding(Canvas.TopProperty, topBinding);

            base.PrepareContainerForItemOverride(element, item);
        }
    }
}

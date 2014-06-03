using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using TimeLegendSpike.ViewModels;

namespace TimeLegendSpike
{
    public partial class ContainerControl : UserControl
    {
        public ContainerControl()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var start = DateTime.Now;
            var vm = this.DataContext as MainViewModel;                                
            vm.Redraw(UseCanvasCheckbox.IsChecked);
            var end = DateTime.Now;
            long diff = end.Ticks - start.Ticks;
            TimeElapsedText.Text = String.Format("{0}ms", new TimeSpan(diff).TotalMilliseconds);            
//            vm.DrawingAreaViewModel.Terminal.CheckPoints[0].Bookings.RemoveAt(1); //.Bookings[1].Start = vm.DrawingAreaViewModel.Terminal.CheckPoints[0].Bookings[1].Start.AddMinutes(30);

        }

        private void ButtonBase_On2Click(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as MainViewModel;   
            vm.DrawingAreaViewModel.SetContext();
        }
    }
}

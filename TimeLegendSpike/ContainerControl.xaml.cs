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
            var vm = this.DataContext as MainViewModel;
            vm.DrawingAreaViewModel.Terminal.CheckPoints[0].Bookings.RemoveAt(1); //.Bookings[1].Start = vm.DrawingAreaViewModel.Terminal.CheckPoints[0].Bookings[1].Start.AddMinutes(30);

        }
    }
}

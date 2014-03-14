using System;
using System.Windows.Controls;
using Jounce.Core.Command;
using Jounce.Framework.Command;
using TimeLegendSpike.ViewModels;

namespace TimeLegendSpike
{
    public partial class MainPage : UserControl
    {
        readonly MainViewModel _context = new MainViewModel();
        public MainPage()
        {
            InitializeComponent();
            this.DataContext = _context;
        }
    }

}

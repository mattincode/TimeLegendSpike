using System;
using System.Windows.Controls;

namespace TimeLegendSpike
{
    public partial class MainPage : UserControl
    {
        readonly TestDataContext _context = new TestDataContext();
        public MainPage()
        {
            InitializeComponent();
            this.DataContext = _context;
        }
    }

    public class TestDataContext
    {
        public DateTime Start { get; set; }

        public TestDataContext()
        {
            Start = DateTime.Now;
        }

        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
    }
}

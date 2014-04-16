using System.Collections.ObjectModel;
using System.Windows.Media;

namespace TimeLegendSpike.ViewModels
{
    public class DrawingAreaViewModel : BaseViewModel
    {
                private Terminal _terminal;

        public Terminal Terminal
        {
            get { return _terminal; }
            set { _terminal = value; RaisePropertyChanged(() => Terminal);}
        }

        public DrawingAreaViewModel()
        {
            var checkpoints = new ObservableCollection<CheckPoint>()
            {
                new CheckPoint()
                {
                    Width = 100,
                    BackgroundBrush = new SolidColorBrush(Colors.Brown),
                    Bookings = new ObservableCollection<Booking>()
                    {
                        new Booking()
                        {
                            Text = "1",
                            X = 10,
                            Y = 10,
                            Length = 50,
                            Width = 24
                        },
                        new Booking()
                        {
                            Text = "2",
                            X = 10,
                            Y = 100,
                            Length = 50,
                            Width = 24
                        },
                        new Booking()
                        {
                            Text = "10",
                            X = 50,
                            Y = 10,
                            Length = 50,
                            Width = 24
                        },
                        new Booking()
                        {
                            Text = "20",
                            X = 50,
                            Y = 100,
                            Length = 50,
                            Width = 24
                        },
                    }
                },
                new CheckPoint()
                {
                    Width = 100,
                    BackgroundBrush = new SolidColorBrush(Colors.DarkGray),
                    Bookings = new ObservableCollection<Booking>()
                    {
                        new Booking()
                        {
                            Text = "3",
                            X = 20,
                            Y = 30,
                            Length = 80,
                            Width = 24
                        },
                        new Booking()
                        {
                            Text = "4",
                            X = 10,
                            Y = 100,
                            Length = 100,
                            Width = 24
                        },
                    }
                }
            };

            Terminal = new Terminal()
            {
                CheckPoints = checkpoints
            };
        }

    }
}

using System;
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

        public DrawingAreaViewModel(DateTime start)
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
                            Start = start,
                            End = start.AddMinutes(90)
                        },
                        new Booking()
                        {
                            Text = "2",
                            Start = start,
                            End = start.AddMinutes(90),
                        },
                        new Booking()
                        {
                            Text = "10",
                            Start = start.AddMinutes(120),
                            End = start.AddMinutes(240)
                        },
                        new Booking()
                        {
                            Text = "20",
                            Start = start.AddMinutes(120),
                            End = start.AddMinutes(240)
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
                            Start = start,
                            End = start.AddMinutes(240)
                        },
                        new Booking()
                        {
                            Text = "4",
                            Start = start.AddMinutes(120),
                            End = start.AddMinutes(240)
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

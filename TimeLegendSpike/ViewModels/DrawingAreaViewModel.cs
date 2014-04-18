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
                            Id = 1,
                            Text = "1",
                            QualificationId = 1,
                            Start = start,
                            End = start.AddMinutes(90)
                        },
                        new Booking()
                        {
                            Id = 2,
                            Text = "2",
                            QualificationId = 1,
                            Start = start,
                            End = start.AddMinutes(90),
                        },
                        new Booking()
                        {
                            Id = 3,
                            Text = "10",
                            QualificationId = 1,
                            Start = start.AddMinutes(120),
                            End = start.AddMinutes(240)
                        },
                        new Booking()
                        {
                            Id = 4,
                            Text = "20",
                            QualificationId = 1,
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
                            Id = 5,
                            Text = "3",
                            QualificationId = 1,
                            Start = start,
                            End = start.AddMinutes(240)
                        },
                        new Booking()
                        {
                            Id = 6,
                            Text = "4",
                            QualificationId = 1,
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

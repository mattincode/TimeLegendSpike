using System;
using System.Collections.Generic;
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

        public bool? UseCanvas { get; set; }

        private Terminal _temp;
        public DrawingAreaViewModel(DateTime start)
        {
            var bgBrush = new SolidColorBrush(Colors.Brown);
            var checkpoints = new ObservableCollection<CheckPoint>()
            {
                new CheckPoint()
                {
                    Width = 100,
                    BackgroundBrush = bgBrush,
                    Bookings = GetBookings(start)
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
                            QualificationId = 1,
                            Start = start,
                            End = start.AddMinutes(240)
                        },
                        new Booking()
                        {
                            Text = "4",
                            QualificationId = 1,
                            Start = start.AddMinutes(120),
                            End = start.AddMinutes(240)
                        },
                    }
                }
            };

            _temp = new Terminal()
            {
                CheckPoints = checkpoints
            };
        }

        public void SetContext()
        {
            Terminal = _temp;
        }

        ObservableCollection<Booking> GetBookings(DateTime start)
        {
            double y = 0;
            var bookings = new List<Booking>();
            for (int i = 0; i < 5000; i++)
            {                
                bookings.Add(new Booking()
                {
                    Text = i.ToString(),
                    QualificationId = 1,
                    X = 10,
                    Y = y,       
                    Length = 40,
                    Width = 50,
                    Start = start,
                    End = start.AddMinutes(60)            
                });
               start = start.AddMinutes(30);
                y += 40;
            }


            return new ObservableCollection<Booking>(bookings);
        }

    }
}

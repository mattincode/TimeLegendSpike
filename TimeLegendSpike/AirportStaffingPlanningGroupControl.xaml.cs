using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TimeLegendSpike.ViewModels;

namespace TimeLegendSpike
{
    public partial class AirportStaffingPlanningGroupControl : UserControl
    {
        public AirportStaffingPlanningGroupControl()
        {
            InitializeComponent();
        }

        #region Dependency properties
        public static readonly DependencyProperty PeriodStartProperty = DependencyProperty.Register("PeriodStart", typeof(DateTime), typeof(UserControl), new PropertyMetadata(new PropertyChangedCallback(PeriodStartChanged)));
        public DateTime PeriodStart
        {
            get { return (DateTime)GetValue(PeriodStartProperty); }
            set
            {
                SetValue(PeriodStartProperty, value);
            }
        }

        public static readonly DependencyProperty PeriodEndProperty = DependencyProperty.Register("PeriodEnd", typeof(DateTime), typeof(UserControl), new PropertyMetadata(new PropertyChangedCallback(PeriodEndChanged)));
        public DateTime PeriodEnd
        {
            get { return (DateTime)GetValue(PeriodEndProperty); }
            set
            {
                SetValue(PeriodEndProperty, value);
            }
        }

        public static readonly DependencyProperty BookingsProperty = DependencyProperty.Register("Bookings", typeof(ObservableCollection<Booking>), typeof(UserControl), new PropertyMetadata(new PropertyChangedCallback(BookingsChanged)));
        public ObservableCollection<Booking> Bookings
        {
            get { return (ObservableCollection<Booking>)GetValue(BookingsProperty); }
            set
            {
                SetValue(BookingsProperty, value);
            }
        }

        private static void PeriodStartChanged(DependencyObject sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var ctl = sender as AirportStaffingPlanningGroupControl;
            var newStart = dependencyPropertyChangedEventArgs.NewValue as DateTime?;
            ctl.UpdatePeriodChanged(newStart, null);
        }

        private static void PeriodEndChanged(DependencyObject sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var ctl = sender as AirportStaffingPlanningGroupControl;
            var newEnd = dependencyPropertyChangedEventArgs.NewValue as DateTime?;
            ctl.UpdatePeriodChanged(null, newEnd);
        }

        private static void BookingsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var oldBookings = e.OldValue as ObservableCollection<Booking>;
            var bookings = e.NewValue as ObservableCollection<Booking>;
            var ctrl = sender as AirportStaffingPlanningGroupControl;
            if (oldBookings != null)
                bookings.CollectionChanged += ctrl.bookings_CollectionChanged;
            if (bookings != null)
                bookings.CollectionChanged += ctrl.bookings_CollectionChanged;
        }

        private void bookings_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var booking in e.NewItems)
                {                    
                    AddBookingControl(booking as Booking);    
                }                
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var booking in e.OldItems)
                {
                    RemoveBookingControl(booking as Booking);
                }                                
            }
        }

        private void UpdatePeriodChanged(DateTime? periodStart, DateTime? periodEnd)
        {
            if (periodStart.HasValue)
            {
                foreach (var control in BookingCanvas.Children)
                {
                    var bookingControl = control as AirportStaffingBookingControl;
                    if (bookingControl.PeriodStart != periodStart.Value)
                        bookingControl.PeriodStart = periodStart.Value;
                }
            }
            else if (periodEnd.HasValue)
            {
                foreach (var control in BookingCanvas.Children)
                {
                    var bookingControl = control as AirportStaffingBookingControl;
                    if (bookingControl.PeriodEnd != periodEnd.Value)
                        bookingControl.PeriodEnd = periodEnd.Value;
                }
            }
        }
        #endregion

        
        // Create controls and add to canvas
        private void AirportStaffingPlanningGroupControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            BookingCanvas.Children.Clear();
            foreach (var booking in Bookings)
            {
                AddBookingControl(booking);
            }       
        }

        private void AddBookingControl(Booking booking)
        {
            BookingCanvas.Children.Add(new AirportStaffingBookingControl()
            {
                Booking = booking,                 
                PeriodStart = PeriodStart,
                PeriodEnd = PeriodEnd
            });        
        }

        private void RemoveBookingControl(Booking booking)
        {
            var bookingControl = BookingCanvas.Children.FirstOrDefault(x => (x as AirportStaffingBookingControl).Booking.Equals(booking));
            if (bookingControl != null)
                BookingCanvas.Children.Remove(bookingControl);
        }


    }
}

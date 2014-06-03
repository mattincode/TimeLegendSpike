using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using TimeLegendSpike.ViewModels;

namespace TimeLegendSpike
{
    public partial class AirportStaffingPGITemsControl : UserControl
    {
        public AirportStaffingPGITemsControl()
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
            var ctl = sender as AirportStaffingPGITemsControl;
            var newStart = dependencyPropertyChangedEventArgs.NewValue as DateTime?;
            ctl.UpdatePeriodChanged(newStart, null);
        }

        private static void PeriodEndChanged(DependencyObject sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var ctl = sender as AirportStaffingPGITemsControl;
            var newEnd = dependencyPropertyChangedEventArgs.NewValue as DateTime?;
            ctl.UpdatePeriodChanged(null, newEnd);
        }

        private static void BookingsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var oldBookings = e.OldValue as ObservableCollection<Booking>;
            var bookings = e.NewValue as ObservableCollection<Booking>;
            var ctrl = sender as AirportStaffingPGITemsControl;
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
                var children = BookingItemControl.GetChildObjects<AirportStaffingBookingControl>(null);

                foreach (var control in children)
                {
                    var bookingControl = control as AirportStaffingBookingControl;
                    if (bookingControl.PeriodStart != periodStart.Value)
                        bookingControl.PeriodStart = periodStart.Value;
                }
            }
            else if (periodEnd.HasValue)
            {
                var children = BookingItemControl.GetChildObjects<AirportStaffingBookingControl>(null);
                foreach (var control in children)
                {
                    var bookingControl = control as AirportStaffingBookingControl;
                    if (bookingControl.PeriodEnd != periodEnd.Value)
                        bookingControl.PeriodEnd = periodEnd.Value;
                }
            }
        }
        #endregion

        
        // Create controls and add to canvas
        private void AirportStaffingPGITemsControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            //BookingCanvas.Children.Clear();
            //foreach (var booking in Bookings)
            //{
            //    AddBookingControl(booking);
            //}       
        }

        private void AddBookingControl(Booking booking)
        {
            //BookingCanvas.Children.Add(new AirportStaffingBookingControl()
            //{
            //    Booking = booking,                 
            //    PeriodStart = PeriodStart,
            //    PeriodEnd = PeriodEnd
            //});        
        }

        private void RemoveBookingControl(Booking booking)
        {
            //var bookingControl = BookingCanvas.Children.FirstOrDefault(x => (x as AirportStaffingBookingControl).Booking.Equals(booking));
            //if (bookingControl != null)
            //    BookingCanvas.Children.Remove(bookingControl);
        }
    }

    // Custom implementation of itemscontrol to be able to position an item on a canvas parent
    public class CanvasItemsControl : ItemsControl
    {
        protected override void PrepareContainerForItemOverride(
                            DependencyObject element,
                            object item)
        {
            // Bind to X and Y of item
            var leftBinding = new Binding() { Path = new PropertyPath("X") };
            var topBinding = new Binding() { Path = new PropertyPath("Y") };
            var contentControl = (FrameworkElement)element;
            // Bind to parent canvas
            contentControl.SetBinding(Canvas.LeftProperty, leftBinding);
            contentControl.SetBinding(Canvas.TopProperty, topBinding);

            base.PrepareContainerForItemOverride(element, item);
        }
    }

    public static class ListExtensions
    {
        public static List<T> GetChildObjects<T>(this DependencyObject obj, string name)
        {
            var retVal = new List<T>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                object c = VisualTreeHelper.GetChild(obj, i);
                if (c.GetType().FullName == typeof(T).FullName && (String.IsNullOrEmpty(name) || ((FrameworkElement)c).Name == name))
                {
                    retVal.Add((T)c);
                }
                var gc = ((DependencyObject)c).GetChildObjects<T>(name);
                if (gc != null)
                    retVal.AddRange(gc);
            }

            return retVal;
        }

        public static T GetChildObject<T>(this DependencyObject obj, string name) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                object c = VisualTreeHelper.GetChild(obj, i);
                if (c.GetType().FullName == typeof(T).FullName && (String.IsNullOrEmpty(name) || ((FrameworkElement)c).Name == name))
                {
                    return (T)c;
                }
                object gc = ((DependencyObject)c).GetChildObject<T>(name);
                if (gc != null)
                    return (T)gc;
            }

            return null;
        }
    }

}

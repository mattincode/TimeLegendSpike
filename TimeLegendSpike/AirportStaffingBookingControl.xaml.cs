using System;
using System.Windows;
using System.Windows.Controls;
using TimeLegendSpike.ViewModels;

namespace TimeLegendSpike
{
    public partial class AirportStaffingBookingControl : UserControl
    {
        #region Dependency properties
        public static readonly DependencyProperty PeriodStartProperty = DependencyProperty.Register("PeriodStart", typeof(DateTime), typeof(UserControl), new PropertyMetadata(new PropertyChangedCallback(PeriodUpdated)));
        public DateTime PeriodStart
        {
            get { return (DateTime)GetValue(PeriodStartProperty); }
            set
            {
                SetValue(PeriodStartProperty, value);
            }
        }

        public static readonly DependencyProperty PeriodEndProperty = DependencyProperty.Register("PeriodEnd", typeof(DateTime), typeof(UserControl), new PropertyMetadata(new PropertyChangedCallback(PeriodUpdated)));


        public DateTime PeriodEnd
        {
            get { return (DateTime)GetValue(PeriodEndProperty); }
            set
            {
                SetValue(PeriodEndProperty, value);
            }
        }

        public static readonly DependencyProperty BookingProperty = DependencyProperty.Register("Booking", typeof(Booking), typeof(UserControl), new PropertyMetadata(new PropertyChangedCallback(BookingUpdated)));

        public Booking Booking
        {
            get { return (Booking)GetValue(BookingProperty); }
            set
            {                
                SetValue(BookingProperty, value);                
            }
        }
        #endregion

        #region Update position on changed data
        // Add changed event handler
        private static void BookingUpdated(DependencyObject sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var oldBooking = dependencyPropertyChangedEventArgs.OldValue as Booking;
            var booking = dependencyPropertyChangedEventArgs.NewValue as Booking;
            var ctrl = sender as AirportStaffingBookingControl;
            if (oldBooking != null)
                booking.PropertyChanged -= ctrl.booking_PropertyChanged;
            if (booking != null)
                booking.PropertyChanged += ctrl.booking_PropertyChanged;
        }

        private void booking_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Start":
                case "End":
                    UpdatePosition();
                    break;
            }
        }

        private static void PeriodUpdated(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = sender as AirportStaffingBookingControl;
            ctrl.UpdatePosition();
        }

        private void UpdatePosition()
        {
            this.SetValue(Canvas.LeftProperty, Booking.X);  // TODO - Calculate position
            this.SetValue(Canvas.TopProperty, Booking.Y);
        }
        #endregion 

        public AirportStaffingBookingControl()
        {
            InitializeComponent();
        }

        private void MainBorder_OnLoaded(object sender, RoutedEventArgs e)
        {
            UpdatePosition();
        }
    }
}

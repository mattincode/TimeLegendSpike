using System;
using System.Windows;
using System.Windows.Controls;
using TimeLegendSpike.ViewModels;

namespace TimeLegendSpike
{
    public partial class AirportStaffingBookingControl : UserControl
    {
        #region Dependency properties
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

        #region Properties
        private DateTime _periodEnd;
        private DateTime _periodStart;

        public DateTime PeriodStart
        {
            get { return _periodStart; }
            set
            {
                _periodStart = value;
                UpdatePosition();
            }
        }

        public DateTime PeriodEnd
        {
            get { return _periodEnd; }
            set
            {
                _periodEnd = value;
                UpdatePosition();
            }
        }
        #endregion Properties

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
            ctrl.UpdatePosition();
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

        private void UpdatePosition()
        {
            var tIncTicks = new TimeSpan(0, 0, AirportStaffingControlConstants.TIncMinutes, 0).Ticks;
            var startOffsetTicks = Booking.Start.Ticks - PeriodStart.Ticks;
            double top = AirportStaffingControlConstants.VIncPx * (startOffsetTicks / tIncTicks);
            double height = AirportStaffingControlConstants.VIncPx * ((Booking.End.Ticks - Booking.Start.Ticks) / tIncTicks);
            double left = Booking.ColumnNo * (AirportStaffingControlConstants.HWidth + AirportStaffingControlConstants.HMargin);

            Booking.Length = height;                                // On viewmodel (not on model)
            Booking.Width = AirportStaffingControlConstants.HWidth; // Replace with constant in XAML?
            this.SetValue(Canvas.LeftProperty, left); 
            this.SetValue(Canvas.TopProperty, top);
        }
        #endregion 

        public AirportStaffingBookingControl()
        {
            InitializeComponent();
        }
    }

    public static class AirportStaffingControlConstants
    {
        public const int TIncMinutes = 30; // 30 minutes
        public const int VIncPx = 18;
        public const int VTextOffset = 0;
        public const int HWidth = 24;
        public const int HMargin = 1;
    }

}

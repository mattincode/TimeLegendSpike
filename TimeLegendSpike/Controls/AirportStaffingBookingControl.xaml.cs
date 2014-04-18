using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
            ctrl.UpdatePosition();  // TODO <- Do we really need this!
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
            var endOffsetTicks = Booking.End.Ticks - PeriodStart.Ticks;

            double periodBottom = ((PeriodEnd.Ticks - PeriodStart.Ticks) / tIncTicks) * AirportStaffingControlConstants.VIncPx;            
            double top = Math.Max(AirportStaffingControlConstants.VIncPx * (startOffsetTicks / tIncTicks), 0);                  // Limit to top bounds
            double bottom = Math.Min(AirportStaffingControlConstants.VIncPx * (endOffsetTicks / tIncTicks), periodBottom);      // Limit to bottom bounds
            double height = Math.Max(bottom - top, 0);
            double left = (Booking.ColumnNo - 1) * (AirportStaffingControlConstants.HWidth + AirportStaffingControlConstants.HMargin);

            this.Height = height;                        
            this.Width = AirportStaffingControlConstants.HWidth; 
            this.SetValue(Canvas.LeftProperty, left); 
            this.SetValue(Canvas.TopProperty, top);
        }
        #endregion 

        public AirportStaffingBookingControl()
        {
            InitializeComponent();
        }

        private void AirportStaffingBookingControl_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Show bookingdialog");
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


    // ------------------------- Logic to add ---------------------------------------

    // ###### Bookingdialog

        //    /// <summary>
        ///// Ctrl-click in the bookings grid to create a new booking. This handler displays the create booking
        ///// popup with pre-filled dates and planning unit.
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="mouseButtonEventArgs"></param>
        //private void OnMouseLeftButtonDownEvent(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        //{
        //    var cellCtrl = sender as FrameworkElement;
        //    if (cellCtrl != null && (Keyboard.Modifiers & ModifierKeys.Control) > 0)
        //    {
        //        var scheduleVm = this.DataContext as WeekScheduleViewModel;
        //        var cellVm = cellCtrl.DataContext as WeekScheduleBookingsCollection;
        //        if (scheduleVm != null && cellVm != null && cellVm.StartDate > cellVm.GraphService.CacheService.LatestSalaryMonthLockDate)
        //        {
        //            var newBooking = cellVm.GraphService.NewBooking(cellVm.PlanningUnit, cellVm.GraphService.IsPlanned, cellVm.StartDate, cellVm.StartDate.AddDays(1));
        //            var bookingVm = scheduleVm.NewBookingVm(cellVm.PlanningUnit, cellVm.StartDate);
        //            var ctrl = new BookingControl(bookingVm, newBooking.StartDate == newBooking.EndDate ? BookingControl.FocusElement.ShiftTimeStart : BookingControl.FocusElement.EmployeeNumber);

        //            GSPApplicationService.Current.AppState.ShowPopup(ctrl, cellCtrl, alignOverParent: true);
        //        }
        //    }
        //}

    // #### Context menu Change/delete

        //   public static RadContextMenu CreateOnBookingCell(FrameworkElement ctrl, BookingViewModel dayVm)
        //{
        //    RadContextMenu menu = new RadContextMenu();

        //    var updateBookingItem = GetUpdateBookingMenuItem(ctrl, dayVm);
        //    if (updateBookingItem != null)
        //    {
        //        menu.Items.Add(updateBookingItem);
        //        menu.Items.Add(new RadMenuItem { IsSeparator = true });
        //    }

        //    var replaceVacancyItem = GetReplaceVacancyMenuItem(ctrl, dayVm);
        //    if (replaceVacancyItem != null)
        //    {
        //        menu.Items.Add(replaceVacancyItem);
        //        menu.Items.Add(new RadMenuItem { IsSeparator = true });
        //    }

        //    if (!dayVm.IsAbsenceDay || dayVm.IsAbsenceDayBookable)
        //    {
        //        var addAbsenceItem = GetAddAbsenceMenuItem(ctrl, dayVm.StartDate, dayVm.BaseGraphService, dayVm.Employee);
        //        if (addAbsenceItem != null)
        //            menu.Items.Add(addAbsenceItem);

        //        var addPartTimeAbsenceItem = GetAddPartTimeAbsenceMenuItem(ctrl, dayVm.StartDate, dayVm.BaseGraphService, dayVm.Employee);
        //        if (addPartTimeAbsenceItem != null)
        //            menu.Items.Add(addPartTimeAbsenceItem);
        //    }

        //    if (dayVm.IsAbsenceDay)
        //    {
        //        var updateAbsenceItems = GetUpdateAbsenceMenuItems(ctrl, dayVm.StartDate, dayVm.BaseGraphService);
        //        if (updateAbsenceItems.Any())
        //        {
        //            if (menu.Items.Any())
        //                menu.Items.Add(new RadMenuItem { IsSeparator = true });

        //            foreach (var item in updateAbsenceItems)
        //                menu.Items.Add(item);
        //        }
        //    }

        //    return menu;
        //}

}

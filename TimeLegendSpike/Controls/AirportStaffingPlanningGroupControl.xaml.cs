using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Itenso.TimePeriod;
using Telerik.Windows.Controls;
using TimeLegendSpike.Annotations;
using TimeLegendSpike.ViewModels;

namespace TimeLegendSpike
{
    public partial class AirportStaffingPlanningGroupControl : UserControl, INotifyPropertyChanged
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

        private bool _initDone = false;
        private void UpdatePeriodChanged(DateTime? periodStart, DateTime? periodEnd)
        {
            if (PeriodStart != DateTime.MinValue && PeriodEnd != DateTime.MinValue && PeriodStart != PeriodEnd)
                InitControls();

            if (periodStart.HasValue)
            {
                foreach (var control in BookingCanvas.Children)
                {
                    var bookingControl = control as AirportStaffingBookingControl;
                    if (bookingControl != null && bookingControl.PeriodStart != periodStart.Value)
                        bookingControl.PeriodStart = periodStart.Value;
                }
            }
            else if (periodEnd.HasValue)
            {
                foreach (var control in BookingCanvas.Children)
                {
                    var bookingControl = control as AirportStaffingBookingControl;
                    if (bookingControl != null && bookingControl.PeriodEnd != periodEnd.Value)
                        bookingControl.PeriodEnd = periodEnd.Value;
                }
            }
        }
        #endregion

        private void InitControls()
        {
            AddGridlines();
            if (!_initDone)
            {
                UpdateColumnPositions();
                foreach (var booking in Bookings)
                {
                    AddBookingControl(booking);
                }
                _initDone = true;
            }
        }

        private void AddGridlines()
        {
            var gridlines = new List<int>();
            var count = (PeriodEnd - PeriodStart).TotalMinutes / AirportStaffingControlConstants.TIncMinutes + 1;
            for (var i = 0; i < count; i++)
            {
                gridlines.Add(i);
            }
            Gridlines = new ObservableCollection<int>(gridlines);
        }

        // Create controls and add to canvas
        private void AddBookingControl(Booking booking)
        {
            BookingCanvas.Children.Add(new AirportStaffingBookingControl()
            {
                Booking = booking,                 
                PeriodStart = PeriodStart,
                PeriodEnd = PeriodEnd,
                Margin = new Thickness(20,0,0,0)
            });        
        }

        private void RemoveBookingControl(Booking booking)
        {
            var bookingControls = BookingCanvas.ChildrenOfType<AirportStaffingBookingControl>();
            var bookingControl = bookingControls.FirstOrDefault(x => x.Booking.Equals(booking));
            if (bookingControl != null)
                BookingCanvas.Children.Remove(bookingControl);
        }

        private void UpdateColumnPositions()
        {
            var sortedBookings = Bookings.OrderBy(x => x.QualificationId);
            foreach (var booking in sortedBookings)
            {
                Booking booking1 = booking;
                var overlappingColumns = Bookings.Where(x => x.Period.OverlapsWith(booking1.Period) && x.Id != booking1.Id).OrderBy(y => y.ColumnNo).Select(z => z.ColumnNo);
                var enumerable = overlappingColumns as int[] ?? overlappingColumns.ToArray();
                if (enumerable.Count() == 1)
                {
                    booking.ColumnNo = overlappingColumns.Max() + 1;
                }
                else if (enumerable.Any())
                {
                    int min = enumerable.Min(), max = enumerable.Max();
                    if (min == 0 && max == 0)
                        booking.ColumnNo = 1;
                    else
                        booking.ColumnNo = Enumerable.Range(min, max - min + 1).Except(enumerable).First();
                }
                else
                    booking.ColumnNo = 1;
            }
            BookingCanvas.Width = 20 + Bookings.Max(x => x.ColumnNo) * AirportStaffingControlConstants.HWidth;            
        }

        public ObservableCollection<int> Gridlines
        {
            get { return _gridlines; }
            set { _gridlines = value; RaisePropertyChanged(() => Gridlines); }
        }

        // TODO -  Replace with GSP -brushes
        private readonly SolidColorBrush _gridBrushOddLines = new SolidColorBrush(Colors.LightGray); //TimeShapeHelper.GetBrush(TimeShapeHelper.VacancyBrushTypeEnum.TimeGridLineBrushOdd);
        private readonly SolidColorBrush _gridBrush = new SolidColorBrush(Colors.DarkGray); //TimeShapeHelper.GetBrush(TimeShapeHelper.VacancyBrushTypeEnum.GroupWarningBrush);
        private ObservableCollection<int> _gridlines;
        // TODO - End

        // Calculate the column to use for each booking
        //private void UpdateColumns()
        //{
        //    var sortedBookings = Bookings.OrderBy(x => x.QualificationId);
        //    foreach (var booking in sortedBookings)
        //    {
        //        var columnPositions = Bookings.Select(x => x.ColumnNo).Distinct();
        //        foreach (var columnPosition in columnPositions)
        //        {
        //            double position = columnPosition;
        //            var shapesInColumn = alreadyDrawn.Where(x => Math.Abs(x.Left - position) < FloatDiff);
        //            if (!shapesInColumn.Any(x => x.Period.OverlapsWith(shape.Period)))
        //            {
        //                // Available space found, check if we have another intersecting shape of the same type 
        //                var sameTypeIntersecting =
        //                    shapesInColumn.Where(
        //                        x => x.Period.Type == shape.Period.Type && x.Period.IntersectsWith(shape.Period));
        //                columnMatch.Add(new ShapeColumn()
        //                {
        //                    Left = columnPosition,
        //                    SameTypeIntersecting = sameTypeIntersecting.Any()
        //                });
        //            }
        //        }
        //    }


        //    double columnPos;
        //    var alreadyDrawn = shapesAlreadyDrawn as TimeShape[] ?? shapesAlreadyDrawn.ToArray();
        //    if (!alreadyDrawn.Any())
        //        columnPos = bounds.X;
        //    else
        //    {
        //        // Try to find some available space in the already drawn columns
        //        var columnPositions = alreadyDrawn.Select(x => x.Left).Distinct();
        //        var columnMatch = new List<ShapeColumn>();
        //        foreach (var columnPosition in columnPositions)
        //        {
        //            double position = columnPosition;
        //            var shapesInColumn = alreadyDrawn.Where(x => Math.Abs(x.Left - position) < FloatDiff);
        //            if (!shapesInColumn.Any(x => x.Period.OverlapsWith(shape.Period)))
        //            {
        //                // Available space found, check if we have another intersecting shape of the same type 
        //                var sameTypeIntersecting =
        //                    shapesInColumn.Where(
        //                        x => x.Period.Type == shape.Period.Type && x.Period.IntersectsWith(shape.Period));
        //                columnMatch.Add(new ShapeColumn()
        //                {
        //                    Left = columnPosition,
        //                    SameTypeIntersecting = sameTypeIntersecting.Any()
        //                });
        //            }
        //        }

        //        // Did we find any available space to draw?
        //        if (columnMatch.Any())
        //        {
        //            // Draw in the first available space
        //            var intersectingShapeColumn = columnMatch.FirstOrDefault(x => x.SameTypeIntersecting);
        //            columnPos = intersectingShapeColumn != null ? intersectingShapeColumn.Left : columnMatch.First().Left;
        //        }
        //        else // No space found, let's move to a new column
        //        {
        //            columnPos = columnPositions.Max() + TimeShape.SHAPE_SIZE + SPACING;
        //        }
        //    }
        //    //	        }
        //    shape.Column = columnPos > bounds.X ? (int)((columnPos - bounds.X) / TimeShape.SHAPE_SIZE) : 1;



        //}

        //// Calculates position for new shape based on the position of already drawn shapes
        //private Point CalculateShapePosition(TimeShape shape, IEnumerable<TimeShape> shapesAlreadyDrawn, Bounds bounds)
        //{
        //    // If the shape has been designated a column, use that!
        //    double columnPos;
        //    //if (shape.Column > 0)
        //    //{
        //    //    columnPos = bounds.X + (shape.Column - 1)*TimeShape.SHAPE_SIZE;
        //    //}
        //    //else
        //    //{
        //    // If no shapes, return upper left corner of bounds
        //    var alreadyDrawn = shapesAlreadyDrawn as TimeShape[] ?? shapesAlreadyDrawn.ToArray();
        //    if (!alreadyDrawn.Any())
        //        columnPos = bounds.X;
        //    else
        //    {
        //        // Try to find some available space in the already drawn columns
        //        var columnPositions = alreadyDrawn.Select(x => x.Left).Distinct();
        //        var columnMatch = new List<ShapeColumn>();
        //        foreach (var columnPosition in columnPositions)
        //        {
        //            double position = columnPosition;
        //            var shapesInColumn = alreadyDrawn.Where(x => Math.Abs(x.Left - position) < FloatDiff);
        //            if (!shapesInColumn.Any(x => x.Period.OverlapsWith(shape.Period)))
        //            {
        //                // Available space found, check if we have another intersecting shape of the same type 
        //                var sameTypeIntersecting =
        //                    shapesInColumn.Where(
        //                        x => x.Period.Type == shape.Period.Type && x.Period.IntersectsWith(shape.Period));
        //                columnMatch.Add(new ShapeColumn()
        //                {
        //                    Left = columnPosition,
        //                    SameTypeIntersecting = sameTypeIntersecting.Any()
        //                });
        //            }
        //        }

        //        // Did we find any available space to draw?
        //        if (columnMatch.Any())
        //        {
        //            // Draw in the first available space
        //            var intersectingShapeColumn = columnMatch.FirstOrDefault(x => x.SameTypeIntersecting);
        //            columnPos = intersectingShapeColumn != null ? intersectingShapeColumn.Left : columnMatch.First().Left;
        //        }
        //        else // No space found, let's move to a new column
        //        {
        //            columnPos = columnPositions.Max() + TimeShape.SHAPE_SIZE + SPACING;
        //        }
        //    }
        //    //	        }
        //    shape.Column = columnPos > bounds.X ? (int)((columnPos - bounds.X) / TimeShape.SHAPE_SIZE) : 1;
        //    return new Point(columnPos, bounds.Y);
        //}


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property that has a new value.</param>
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Raises this object's PropertyChanged event for each of the properties.
        /// </summary>
        /// <param name="propertyNames">The properties that have a new value.</param>
        protected void RaisePropertyChanged(params string[] propertyNames)
        {
            foreach (var name in propertyNames)
            {
                RaisePropertyChanged(name);
            }
        }

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <typeparam name="T">The type of the property that has a new value</typeparam>
        /// <param name="propertyExpression">A Lambda expression representing the property that has a new value.</param>
        protected void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            var propertyName = ExtractPropertyName(propertyExpression);
            RaisePropertyChanged(propertyName);
        }

        protected string ExtractPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException("propertyExpression");
            }

            var memberExpression = propertyExpression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ArgumentException("Property not a member", "propertyExpression");
            }

            var property = memberExpression.Member as PropertyInfo;
            if (property == null)
            {
                throw new ArgumentException("Not a propery", "propertyExpression");
            }

            var getMethod = property.GetGetMethod(true);

            if (getMethod == null)
            {
                // this shouldn't happen - the expression would reject the property before reaching this far
                throw new ArgumentException("No getter", "propertyExpression");
            }

            if (getMethod.IsStatic)
            {
                throw new ArgumentException("Static property", "propertyExpression");
            }

            return memberExpression.Member.Name;
        }

    }
}

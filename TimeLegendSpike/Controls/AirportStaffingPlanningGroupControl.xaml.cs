using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Itenso.TimePeriod;
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
            if (!_initDone)
            {
                BookingCanvas.Children.Clear();
                UpdateColumnPositions();
                DrawGridlines();
                foreach (var booking in Bookings)
                {
                    AddBookingControl(booking);
                }
                _initDone = true;
            }
        }

        // Create controls and add to canvas
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
            BookingCanvas.Width = Bookings.Max(x => x.ColumnNo) * AirportStaffingControlConstants.HWidth;            
        }

        private void DrawGridlines()
        {
            var container = BookingCanvas.Children;
            var from = new Point(0, 0);
            var to = new Point(BookingCanvas.Width, 0);
            var time = PeriodStart;
            var toggleGridline = false;

            while (time <= PeriodEnd)
            {
                DrawGridLine(container, from, to, toggleGridline);
                from.Y += AirportStaffingControlConstants.VIncPx;
                to.Y += AirportStaffingControlConstants.VIncPx;
                toggleGridline = !toggleGridline;

                time = time.AddMinutes(30);
            }
        }

        // TODO -  Replace with GSP -brushes
        private readonly SolidColorBrush _gridBrushOddLines = new SolidColorBrush(Colors.LightGray); //TimeShapeHelper.GetBrush(TimeShapeHelper.VacancyBrushTypeEnum.TimeGridLineBrushOdd);
        private readonly SolidColorBrush _gridBrush = new SolidColorBrush(Colors.DarkGray); //TimeShapeHelper.GetBrush(TimeShapeHelper.VacancyBrushTypeEnum.GroupWarningBrush);
        // TODO - End
        private void DrawGridLine(UIElementCollection container, Point from, Point to, bool oddGridLine)
        {
            var line = new Line
            {
                X1 = from.X,
                Y1 = from.Y,
                X2 = to.X,
                Y2 = to.Y,
                StrokeThickness = 1,                                
            };
            Canvas.SetZIndex(line,-1); // Remove if we want the lines to shine through
            line.Stroke = oddGridLine ? _gridBrushOddLines : _gridBrush;
            container.Add(line);
        }

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


    }
}

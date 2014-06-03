using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
                    if (bookingControl == null) return;
                    if (bookingControl.PeriodStart != periodStart.Value)
                        bookingControl.PeriodStart = periodStart.Value;
                }
            }
            else if (periodEnd.HasValue)
            {
                foreach (var control in BookingCanvas.Children)
                {
                    var bookingControl = control as AirportStaffingBookingControl;
                    if (bookingControl == null) return;
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

        static SolidColorBrush _bgBrush = new SolidColorBrush(Colors.Blue);
        static CornerRadius _radius = new CornerRadius(2);

        private void AddBookingControl(Booking booking)
        {
            var border = new Border()
            {                
                Height = booking.Length,
                Width = booking.Width,
                Background = _bgBrush,
                CornerRadius = _radius
            };
            var stack = new StackPanel(){Orientation = Orientation.Vertical};
            
            var text1 = new TextBlock() { Text = booking.Text };
            var text2 = new TextBlock() { Text = "hej", Width = booking.Width };
            var text3 = new TextBlock() { Text = "hej2", Width = booking.Width };
            var text4 = new TextBlock() { Text = "hej3", Width = booking.Width };
            stack.Children.Add(text1);
            stack.Children.Add(text2);
            stack.Children.Add(text3);
            stack.Children.Add(text4);
            
            border.Child = stack;
            border.SetValue(Canvas.TopProperty, booking.Y);
            border.SetValue(Canvas.LeftProperty, booking.X);
            BookingCanvas.Children.Add(border);



        //            <StackPanel Orientation="Vertical">
        //    <TextBlock Text="{Binding Booking.Text, RelativeSource={RelativeSource AncestorType=UserControl}}" />
        //    <TextBlock Width="{Binding Booking.Width}" Text="Hej" />
        //    <TextBlock Width="{Binding Booking.Width}" Text="Hej2" />
        //    <TextBlock Width="{Binding Booking.Width}" Text="Hej3" />
        //</StackPanel>

            
            //BookingCanvas.Children.Add(text1);



            //            <Border x:Name="MainBorder"
            //        HorizontalAlignment="Stretch"
            //        VerticalAlignment="Stretch"
            //        Background="BlueViolet"
            //        CornerRadius="2">
            //    <StackPanel Orientation="Vertical">
            //        <TextBlock Text="{Binding Booking.Text, RelativeSource={RelativeSource AncestorType=UserControl}}" />
            //        <StackPanel Orientation="Vertical">
            //            <TextBlock Width="{Binding Booking.Width}" Text="Hej" />
            //            <TextBlock Width="{Binding Booking.Width}" Text="Hej2" />
            //            <TextBlock Width="{Binding Booking.Width}" Text="Hej3" />
            //        </StackPanel>
            //    </StackPanel>
            //</Border>


            //BookingCanvas.Children.Add(new AirportStaffingBookingControl()
            //{
            //    Booking = booking,                 
            //    PeriodStart = PeriodStart,
            //    PeriodEnd = PeriodEnd
            //});        
        }

        private void RemoveBookingControl(Booking booking)
        {
            var bookingControl = BookingCanvas.Children.FirstOrDefault(x => (x as AirportStaffingBookingControl).Booking.Equals(booking));
            if (bookingControl != null)
                BookingCanvas.Children.Remove(bookingControl);
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

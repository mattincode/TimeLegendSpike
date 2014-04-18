using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Itenso.TimePeriod;

namespace TimeLegendSpike.ViewModels
{
    public class CheckPoint : BaseViewModel
    {
        private ObservableCollection<Booking> _bookings;
        private Brush _backgroundBrush;
        private double _width;

        public ObservableCollection<Booking> Bookings
        {
            get { return _bookings; }
            set { _bookings = value; RaisePropertyChanged(() => Bookings); }
        }

        public Brush BackgroundBrush
        {
            get { return _backgroundBrush; }
            set { _backgroundBrush = value; RaisePropertyChanged(() => BackgroundBrush); }
        }

        public double Width
        {
            get { return _width; }
            set { _width = value; RaisePropertyChanged(() => Width); }
        }

        public CheckPoint()
        {
            Bookings = new ObservableCollection<Booking>();
        }
    }

    public class Terminal : BaseViewModel
    {
        private ObservableCollection<CheckPoint> _checkPoints;

        public ObservableCollection<CheckPoint> CheckPoints
        {
            get { return _checkPoints; }
            set { _checkPoints = value; RaisePropertyChanged(() => CheckPoints); }
        }

        public Terminal()
        {
            CheckPoints = new ObservableCollection<CheckPoint>();
        }
    }

    public class Booking : BaseViewModel
    {
        private double _y;
        private double _x;
        private double _length;
        private double _width;
        private string _text;
        private DateTime _start;
        private DateTime _end;
        private int _columnNo;
        private int _qualificationId;
        public int Id { get; set; }

        public int ColumnNo // Zero based
        {
            get { return _columnNo; } 
            set { _columnNo = value; RaisePropertyChanged(() => ColumnNo); }
        }

        public DateTime Start
        {
            get { return _start; }
            set { _start = value; RaisePropertyChanged(() => Start);}
        }

        public DateTime End
        {
            get { return _end; }
            set { _end = value; RaisePropertyChanged(() => End); }
        }

        public string Text
        {
            get { return _text; }
            set { _text = value; RaisePropertyChanged(() => Text); }
        }

        public double Y
        {
            get { return _y; }
            set { _y = value; RaisePropertyChanged(() => Y); }
        }

        public double X
        {
            get { return _x; }
            set { _x = value; RaisePropertyChanged(() => X); }
        }

        public double Length
        {
            get { return _length; }
            set { _length = value; RaisePropertyChanged(() => Length); }
        }

        public double Width
        {
            get { return _width; }
            set { _width = value; RaisePropertyChanged(() => Width); }
        }

        public int QualificationId
        {
            get { return _qualificationId; }
            set { _qualificationId = value; RaisePropertyChanged(() => QualificationId); }
        }

        public ITimeRange Period { get {return new TimeRange(Start, End);} }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Jounce.Core.Command;
using Jounce.Framework.Command;

namespace TimeLegendSpike.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private DateTime _start;
        private DateTime _end;
        private ObservableCollection<TopLegendItem> _legendItems;
        private DrawingAreaViewModel _drawingAreaViewModel;
        private bool _useCanvas;

        public DrawingAreaViewModel DrawingAreaViewModel
        {
            get { return _drawingAreaViewModel; }
            set { _drawingAreaViewModel = value; RaisePropertyChanged(() => DrawingAreaViewModel); }
        }

        public DateTime Start
        {
            get { return _start; }
            set
            {
                _start = value; 
                RaisePropertyChanged(() => Start);
                if (NavigateBackCommand != null)
                    NavigateBackCommand.RaiseCanExecuteChanged();
            }
        }

        public DateTime End
        {
            get { return _end; }
            set { _end = value; RaisePropertyChanged(() => End); }
        }

        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }

        public ObservableCollection<TopLegendItem> LegendItems
        {
            get { return _legendItems; }
            set { _legendItems = value; RaisePropertyChanged(()=> LegendItems); }
        }

        public IActionCommand NavigateBackCommand { get; private set; }
        public IActionCommand NavigateForwardCommand { get; private set; }

        public bool UseCanvas
        {
            get { return _useCanvas; }
            set { _useCanvas = value; RaisePropertyChanged(() => UseCanvas); }
        }

        public MainViewModel()
        {
            Start = new DateTime(2014,03,1,10,00,00);
            PeriodStart = Start; //.AddMinutes(-60);
            PeriodEnd = Start.AddDays(2);
            //LegendItems = getDummyDataLegendItems();
            NavigateBackCommand = new ActionCommand<object>(OnNavigateBack, CanExecuteNavigateBack);
            NavigateForwardCommand = new ActionCommand<object>(OnNavigateForward, CanExecuteNavigateForward);
            //DrawingAreaViewModel = new DrawingAreaViewModel(Start);
        }

        public void Redraw(bool? useCanvas)
        {
            DrawingAreaViewModel = new DrawingAreaViewModel(Start){UseCanvas = useCanvas};
        }



        #region Commands
        private bool CanExecuteNavigateBack(object o)
        {
            var canExecute = (Start > PeriodStart);
            Debug.WriteLine("CanExecuteNavigateBack: {0}", canExecute);
            return canExecute;
        }

        private void OnNavigateBack(object o)
        {
            if (Start == PeriodStart)
                return;
            var newStart = Start.AddMinutes(-Constants.TIncMinutes);
            Start = newStart < PeriodStart ? PeriodStart : newStart;
        }

        private bool CanExecuteNavigateForward(object o)
        {
            bool canExecute = (End < PeriodEnd);
            Debug.WriteLine("CanExecuteNavigateForward: {0}", canExecute);
            return canExecute;
        }

        private void OnNavigateForward(object o)
        {
            if (End == PeriodEnd)
                return;
            var newEnd = End.AddMinutes(Constants.TIncMinutes);
            Start = newEnd > PeriodEnd ? PeriodStart : Start.AddMinutes(Constants.TIncMinutes);
        }
        #endregion Commands
    }

}

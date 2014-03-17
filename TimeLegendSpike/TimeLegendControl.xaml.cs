using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace TimeLegendSpike
{
    public partial class TimeLegendControl : UserControl
    {
        private SolidColorBrush _timeLegendFontBrush = new SolidColorBrush(Colors.Black);

        #region Dependency properties
        public static readonly DependencyProperty StartProperty = DependencyProperty.Register("Start", typeof(DateTime), typeof(UserControl), new PropertyMetadata(new PropertyChangedCallback(StartPeriodChanged)));
        public DateTime Start
        {
            get { return (DateTime)GetValue(StartProperty); }
            set
            {
                SetValue(StartProperty, value);
            }
        }

        public static readonly DependencyProperty EndProperty = DependencyProperty.Register("End", typeof(DateTime), typeof(UserControl), new PropertyMetadata(null));
        public DateTime End
        {
            get { return (DateTime)GetValue(EndProperty); }
            set
            {
                SetValue(EndProperty, value);
            }
        }

        public static readonly DependencyProperty ActualTextWidthProperty = DependencyProperty.Register("ActualTextWidth", typeof(double), typeof(UserControl), new PropertyMetadata(null));
        public double ActualTextWidth
        {
            get { return (double)GetValue(ActualTextWidthProperty); }
            set
            {
                SetValue(ActualTextWidthProperty, value);
            }
        }
        #endregion Dependency properties

        public TimeLegendControl()
        {
            InitializeComponent();

            // Subscribe to change notifications
            this.SizeChanged += TimeLegendControl_SizeChanged;
        }

        private static void StartPeriodChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var control = sender as TimeLegendControl;
            if (control != null)
            {
                control.DrawTime();
            }
        }

        void TimeLegendControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Debug.WriteLine("#TimeLegendControl_SizeChanged");
            DrawTime();
        }

        public void DrawTime()
        {
            double maxWidth = 0;
            double height = LayoutRoot.ActualHeight;

            // Sanity check
            if (double.IsNaN(height) || height < Constants.VIncPx)
            {
                Debug.WriteLine("#TimeLegendControl.DrawTime Height: NaN");
                return;
            }

            DateTime currentTime = Start;
            var from = new Point(0, Constants.VTextOffset);
            // Calulate the number of times we can draw based on the height
            int count = (int)height / Constants.VIncPx;
            LayoutRoot.Children.Clear();

            for (int i = 0; i < count; i++)
            {
                maxWidth = Math.Max(maxWidth, TextHelper.DrawText(LayoutRoot.Children,
                                        new Point(from.X, from.Y),
                                        MaxWidth,
                                        string.Format("{0} {1}", currentTime.ToShortDateString(),
                                        string.Format("{0:HH:mm}", currentTime)),
                                        _timeLegendFontBrush,
                                        horizontalAlignment: HorizontalAlignment.Left,
                                        verticalAlignment: VerticalAlignment.Top));

                from.Y += Constants.VIncPx;
                currentTime = currentTime.AddMinutes(Constants.TIncMinutes);
                //Debug.WriteLine("#TimeLegendControl.DrawTime Y:{0}", from.Y);
            }
            // Report back result of the draw operation
            ActualTextWidth = maxWidth;
            End = currentTime.AddMinutes(-Constants.TIncMinutes);

        }
    }
}

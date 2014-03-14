using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TimeLegendSpike
{
    public partial class TimeLegendControl : UserControl
    {
        private SolidColorBrush _timeLegendFontBrush = new SolidColorBrush(Colors.Black);
        private const int TIncMinutes = 30; // 30 minutes
        private const int VIncPx = 15;
        private const int VTextOffset = 0;

        #region Dependency properties
        public static readonly DependencyProperty StartProperty = DependencyProperty.Register("Start", typeof(DateTime), typeof(UserControl), new PropertyMetadata(null));
        public DateTime Start
        {
            get { return (DateTime)GetValue(StartProperty); }
            set
            {
                SetValue(StartProperty, value);
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
            this.SizeChanged += TimeLegendControl_SizeChanged;
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
            if (double.IsNaN(height) || height < VIncPx)
            {
                Debug.WriteLine("#TimeLegendControl.DrawTime Height: NaN");
                return;
            }

            DateTime currentTime = Start;
            var from = new Point(0, VTextOffset);
            int count = (int)height / VIncPx;
            LayoutRoot.Children.Clear();

            for (int i = 0; i < count; i++)
            {
                maxWidth = Math.Max(maxWidth, TimeLegendText.DrawText(LayoutRoot.Children,
                                        new Point(from.X, from.Y),
                                        MaxWidth,
                                        string.Format("{0} {1}", currentTime.ToShortDateString(),
                                        string.Format("{0:HH:mm}", currentTime)),
                                        _timeLegendFontBrush,
                                        verticalAlignment: VerticalAlignment.Top));

                from.Y += VIncPx;
                currentTime = currentTime.AddMinutes(TIncMinutes);
                //Debug.WriteLine("#TimeLegendControl.DrawTime Y:{0}", from.Y);
            }
            ActualTextWidth = maxWidth;

        }
    }
}

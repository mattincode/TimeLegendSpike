using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace TimeLegendSpike.Converters
{
    public class OddValueBrushConverter : IValueConverter
    {

        private readonly SolidColorBrush _gridBrushOddLines = new SolidColorBrush(Colors.LightGray); //TimeShapeHelper.GetBrush(TimeShapeHelper.VacancyBrushTypeEnum.TimeGridLineBrushOdd);
        private readonly SolidColorBrush _gridBrush = new SolidColorBrush(Colors.DarkGray); //TimeShapeHelper.GetBrush(TimeShapeHelper.VacancyBrushTypeEnum.GroupWarningBrush);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (int) value;
            if ((val%2) > 0)
                return _gridBrushOddLines;
            else
                return _gridBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

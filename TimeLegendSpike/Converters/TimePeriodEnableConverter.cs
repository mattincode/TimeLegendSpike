using System;
using System.Globalization;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace TimeLegendSpike.Converters
{

    public class TimePeriodEnableConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value.Length > 2 && value[0] != null && value[1] != null)
            {
                var date = (DateTime)value[0];
                var borderDate = (DateTime)value[1];



            }
            return false;
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            // Convert
            throw new NotImplementedException();
        }
    }


}

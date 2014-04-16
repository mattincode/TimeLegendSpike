using System;
using System.Globalization;
using TimeLegendSpike.ViewModels;

namespace TimeLegendSpike.Converters
{    
    public class AirportStaffingBookingToSizeAndPosConverter : IMultiValueConverter
    {
        public static class AirportStaffingControlConstants
        {
            public const int TIncMinutes = 30; // 30 minutes
            public const int VIncPx = 18;
            public const int VTextOffset = 0;
            public const int HWidth = 24;
            public const int HMargin = 1;
        }

        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            double val = 0;
            var param = parameter as string;
            if (value != null && value.Length > 2 && value[0] != null && value[1] != null && value[2] != null)
            {
                var booking = value[0] as Booking;
                var periodStart = value[1] as DateTime?;
                var periodEnd = value[2] as DateTime?;
                if (booking != null && periodStart.HasValue && periodEnd.HasValue)
                {
                    var tIncTicks = new TimeSpan(0, 0, AirportStaffingControlConstants.TIncMinutes, 0).Ticks;
                    switch (param)
                    {
                        case "Top": // Calulcate top position based on booking start + period                            
                            var startOffsetTicks = booking.Start.Ticks - periodStart.Value.Ticks;
                            val = AirportStaffingControlConstants.VIncPx * ( startOffsetTicks / tIncTicks );                            
                            break;

                        case "Height": // Calulcate height based on booking length, concatenate at periodEnd
                            val = AirportStaffingControlConstants.VIncPx * ((booking.End.Ticks - booking.Start.Ticks) / tIncTicks); 
                            break;

                        case "Left":
                            val = booking.ColumnNo * (AirportStaffingControlConstants.HWidth + AirportStaffingControlConstants.HMargin);
                            break;

                        case "Width":
                        val = AirportStaffingControlConstants.HWidth; 
                            break;
                    }
                }
            }
            return val;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            // Convert
            throw new NotImplementedException();
        }
    }
}

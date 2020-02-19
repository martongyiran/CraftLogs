using CraftLogs.BLL.Enums;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace CraftLogs.Views.Converters
{
    public class LogTypeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is LogTypeEnum)
            {
                return value switch
                {
                    LogTypeEnum.Quest => Color.Blue,
                    LogTypeEnum.Buy => Color.Green,
                    LogTypeEnum.Sell => Color.Red,
                    LogTypeEnum.Trade => Color.Orange,
                    LogTypeEnum.Arena => Color.IndianRed,
                    LogTypeEnum.System => Color.Purple,
                    _ => Color.White,
                };
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

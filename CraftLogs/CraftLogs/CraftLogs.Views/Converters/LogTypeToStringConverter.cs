using CraftLogs.BLL.Enums;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace CraftLogs.Views.Converters
{
    public class LogTypeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is LogTypeEnum)
            {
                return value switch
                {
                    LogTypeEnum.Quest => "Állomás",
                    LogTypeEnum.Buy => "Vásárlás",
                    LogTypeEnum.Sell => "Eladás",
                    LogTypeEnum.Trade => "Csere",
                    LogTypeEnum.Arena => "Aréna",
                    LogTypeEnum.System => "System",
                    _ => "-",
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

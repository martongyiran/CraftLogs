using CraftLogs.BLL.Enums;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace CraftLogs.Views.Converters
{
    public class ItemStateEnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ItemStateEnum)
            {
                return value switch
                {
                    ItemStateEnum.Backpack => "Hátizsákban",
                    ItemStateEnum.Equipped => "Használatban",
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

using CraftLogs.BLL.Enums;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace CraftLogs.Views.Converters
{
    public class ItemTypeEnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ItemTypeEnum)
            {
                return value switch
                {
                    ItemTypeEnum.Armor => "Páncél",
                    ItemTypeEnum.LHand => "Fegyver (Bal kéz)",
                    ItemTypeEnum.Neck => "Nyaklánc",
                    ItemTypeEnum.RHand => "Fegyver (Jobb kéz)",
                    ItemTypeEnum.Ring => "Gyűrű",
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

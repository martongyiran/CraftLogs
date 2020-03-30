using CraftLogs.BLL.Enums;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace CraftLogs.Views.Converters
{
    public class ItemRarityToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ItemRarityEnum)
            {
                return value switch
                {
                    ItemRarityEnum.Common => Color.Black,
                    ItemRarityEnum.Rare => Color.Blue,
                    ItemRarityEnum.Legendary => Color.Orange,
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

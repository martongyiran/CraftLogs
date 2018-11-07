using System;
using System.Globalization;
using CraftLogs.BLL.Enums;
using Xamarin.Forms;

namespace CraftLogs.Views.Converters
{
    public class ItemRarityEnumToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is ItemRarityEnum)
            {
                switch ((ItemRarityEnum)value)
                {
                    case ItemRarityEnum.Common:
                        return Color.Gray;
                    case ItemRarityEnum.Uncommon:
                        return Color.Green;
                    case ItemRarityEnum.Rare:
                        return Color.Blue;
                    case ItemRarityEnum.Epic:
                        return Color.Purple;
                    default:
                        return Color.White;
                }
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

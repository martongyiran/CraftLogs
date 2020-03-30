using CraftLogs.BLL.Enums;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace CraftLogs.Views.Converters
{
    public class CharacterClassEnumToCharConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CharacterClassEnum)
            {
                return value switch
                {
                    CharacterClassEnum.Mage => "M",
                    CharacterClassEnum.Rogue => "R",
                    CharacterClassEnum.Warrior => "W",
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

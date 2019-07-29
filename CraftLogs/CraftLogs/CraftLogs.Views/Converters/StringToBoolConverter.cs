using System;
using System.Globalization;
using Xamarin.Forms;

namespace CraftLogs.Views.Converters
{
    public class StringToBoolConverter : IValueConverter
    {
        /// <summary>
        /// Convert the string to bool.
        /// </summary>
        /// <returns>If the sring is empty or null it's returns false.</returns>
        /// <param name="value">Value.</param>
        /// <param name="targetType">Target type.</param>
        /// <param name="parameter">Parameter.</param>
        /// <param name="culture">Culture.</param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                if(string.IsNullOrEmpty((string)value) || string.IsNullOrWhiteSpace((string)value))
                {
                    return false;
                }
                return true;
            }
            else if(value is int)
            {
                if((int)value == 0)
                {
                    return false;
                }
                return true;
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

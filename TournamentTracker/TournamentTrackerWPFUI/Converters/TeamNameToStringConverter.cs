using System;
using System.Globalization;
using System.Windows.Data;

namespace TournamentTrackerWPFUI.Converters
{
    public class TeamNameToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == null)
            {
                return "No info";
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}

namespace Solarertrag.Converter
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    [ValueConversion(typeof(int), typeof(bool))]
    public class YearToGroupExpanderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int testInt = 0;
            bool result = Int32.TryParse(value.ToString(), out testInt);
            if (testInt == DateTime.Now.Year)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

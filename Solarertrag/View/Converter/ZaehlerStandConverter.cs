namespace Solarertrag.Converter
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Data;

    public class ZaehlerStandConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Dictionary<string, string> ertrag = (Dictionary<string,string>)value;
            return $"{ertrag.FirstOrDefault(f => f.Key == parameter.ToString()).Value}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

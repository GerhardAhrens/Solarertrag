namespace Solarertrag.Converter
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Data;

    [ValueConversion(typeof(int), typeof(string))]
    public sealed class RowNumberConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values == null)
            {
                return 0;
            }

            object item = values[0];
            ListView grid = values[1] as ListView;

            int index = -1;
            if (item != null)
            {
                index = grid.Items.IndexOf(item);
            }

            return $"{index + 1}";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
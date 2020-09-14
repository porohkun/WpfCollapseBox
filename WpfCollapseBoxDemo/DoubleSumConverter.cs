using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WpfCollapseBoxDemo
{
    public class DoubleSumConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var sum = 0d;
            foreach (var value in values)
            {
                if (value is double)
                    sum += (double)value;
            }
            return sum;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("DoubleSumConverter is a OneWay converter.");
        }
    }
}

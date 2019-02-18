using System;
using System.Globalization;
using System.Windows.Data;

namespace Swtor.Dps.StatOptimizer.Converters
{
	public class GreaterThanOrEqualToZeroConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (double) value >= 0;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

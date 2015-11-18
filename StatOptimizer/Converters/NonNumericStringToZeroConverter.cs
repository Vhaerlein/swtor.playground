using System;
using System.Globalization;
using System.Windows.Data;

namespace TorPlayground.StatOptimizer.Converters
{
	public class NonNumericStringToZeroConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return 0;
			double output;
			return !double.TryParse(value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out output) ? 0 : value;
		}
	}
}

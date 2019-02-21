using System;
using System.Globalization;
using System.Windows.Data;

namespace Swtor.Dps.StatOptimizer.Converters
{
	public class NonNumericStringToZeroConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
			=> value;

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return 0;

			return !double.TryParse(value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out _) ? 0 : value;
		}
	}
}

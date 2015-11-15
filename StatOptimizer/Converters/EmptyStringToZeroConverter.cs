using System;
using System.Globalization;
using System.Windows.Data;

namespace TorPlayground.StatOptimizer.Converters
{
	public class EmptyStringToZeroConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return 0;

			return string.IsNullOrEmpty(value.ToString()) ? 0 : value;
		}
	}
}

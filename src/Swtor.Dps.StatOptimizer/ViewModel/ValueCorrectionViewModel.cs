using System.Globalization;
using Swtor.Dps.StatOptimizer.Utils;

namespace Swtor.Dps.StatOptimizer.ViewModel
{
	public class ValueCorrectionViewModel : NotifyPropertyChangedObject
	{
		public double NewValue
		{
			get { return _newValue; }
			set
			{
				_newValue = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(NewValueString));
			}
		}
		private double _newValue;

		public string DifferenceString => $"{(Difference > 0 ? "+" : "")}{Difference.ToString(_format, CultureInfo.InvariantCulture)}";

		public double Difference
		{
			get { return _difference; }
			set
			{
				_difference = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(DifferenceString));
			}
		}
		private double _difference;

		private readonly string _format;

		public string NewValueString => NewValue.ToString(_format, CultureInfo.InvariantCulture);

		public ValueCorrectionViewModel(ValueType type)
		{
			if (type == ValueType.Integer)
				_format = "0";
			else if (type == ValueType.Percent)
				_format = "P2";
			else
				_format = "0.00";
		}
	}
}

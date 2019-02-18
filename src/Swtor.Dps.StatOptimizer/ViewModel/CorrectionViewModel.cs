using Swtor.Dps.DamageModel;
using Swtor.Dps.StatOptimizer.Utils;

namespace Swtor.Dps.StatOptimizer.ViewModel
{
	public class CorrectionViewModel : NotifyPropertyChangedObject
	{
		public bool IsOutdated
		{
			get { return _isOutdated; }
			set
			{
				if (_isOutdated != value)
				{
					_isOutdated = value;
					OnPropertyChanged();
				}
			}
		}
		private bool _isOutdated;

		public ValueCorrectionViewModel AccuracyPoints { get; }
		public ValueCorrectionViewModel ExtraMasteryPoints { get; }
		public ValueCorrectionViewModel AlacrityPoints { get; }
		public ValueCorrectionViewModel CriticalPoints { get; }
		public ValueCorrectionViewModel ExtraPowerPoints { get; }

		public ValueCorrectionViewModel Accuracy { get; }
		public ValueCorrectionViewModel Alacrity { get; }
		public ValueCorrectionViewModel Critical { get; }
		public ValueCorrectionViewModel Surge { get; }
		public ValueCorrectionViewModel BonusDamage { get; }
		public ValueCorrectionViewModel ForceTechBonusDamage { get; }

		private readonly Configuration _configuration;

		public CorrectionViewModel(Configuration configuration)
		{
			_configuration = configuration;
			AccuracyPoints = new ValueCorrectionViewModel(ValueType.Integer);
			ExtraMasteryPoints = new ValueCorrectionViewModel(ValueType.Integer);
			AlacrityPoints = new ValueCorrectionViewModel(ValueType.Integer);
			CriticalPoints = new ValueCorrectionViewModel(ValueType.Integer);
			ExtraPowerPoints = new ValueCorrectionViewModel(ValueType.Integer);

			Accuracy = new ValueCorrectionViewModel(ValueType.Percent);
			Alacrity = new ValueCorrectionViewModel(ValueType.Percent);
			Critical = new ValueCorrectionViewModel(ValueType.Percent);
			Surge = new ValueCorrectionViewModel(ValueType.Percent);
			BonusDamage = new ValueCorrectionViewModel(ValueType.Double);
			ForceTechBonusDamage = new ValueCorrectionViewModel(ValueType.Double);
		}

		public void UpdateCorrection(ConfigurationCorrection correction)
		{
			IsOutdated = false;

			AccuracyPoints.Difference = correction.AccuracyPoints - _configuration.AccuracyPoints;
			AccuracyPoints.NewValue = correction.AccuracyPoints;

			ExtraMasteryPoints.Difference = correction.MasteryPoints - _configuration.ExtraMasteryPoints;
			ExtraMasteryPoints.NewValue = correction.MasteryPoints;

			AlacrityPoints.Difference = correction.AlacrityPoints - _configuration.AlacrityPoints;
			AlacrityPoints.NewValue = correction.AlacrityPoints;

			CriticalPoints.Difference = correction.CriticalPoints - _configuration.CriticalPoints;
			CriticalPoints.NewValue = correction.CriticalPoints;

			ExtraPowerPoints.Difference = correction.PowerPoints - _configuration.ExtraPowerPoints;
			ExtraPowerPoints.NewValue = correction.PowerPoints;

			var updatedConfiguration = _configuration.Clone();

			updatedConfiguration.AccuracyPoints = correction.AccuracyPoints;
			updatedConfiguration.AlacrityPoints = correction.AlacrityPoints;
			updatedConfiguration.CriticalPoints = correction.CriticalPoints;
			updatedConfiguration.ExtraMasteryPoints = correction.MasteryPoints;
			updatedConfiguration.ExtraPowerPoints = correction.PowerPoints;

			Accuracy.NewValue = updatedConfiguration.Accuracy;
			Accuracy.Difference = Accuracy.NewValue - _configuration.Accuracy;

			Alacrity.NewValue = updatedConfiguration.Alacrity;
			Alacrity.Difference = Alacrity.NewValue - _configuration.Alacrity;

			Critical.NewValue = updatedConfiguration.Critical;
			Critical.Difference = Critical.NewValue - _configuration.Critical;

			Surge.NewValue = updatedConfiguration.Surge;
			Surge.Difference = Surge.NewValue - _configuration.Surge;

			BonusDamage.NewValue = updatedConfiguration.BonusDamage;
			BonusDamage.Difference = BonusDamage.NewValue - _configuration.BonusDamage;

			ForceTechBonusDamage.NewValue = updatedConfiguration.SpellBonusDamage;
			ForceTechBonusDamage.Difference = ForceTechBonusDamage.NewValue - _configuration.SpellBonusDamage;
		}
	}
}

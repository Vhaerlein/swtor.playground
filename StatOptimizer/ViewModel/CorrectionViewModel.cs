using TorPlayground.DamageModel;
using TorPlayground.StatOptimizer.Utils;

namespace TorPlayground.StatOptimizer.ViewModel
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
		public ValueCorrectionViewModel AugmentMasteryPoints { get; }
		public ValueCorrectionViewModel AlacrityPoints { get; }
		public ValueCorrectionViewModel CriticalPoints { get; }
		public ValueCorrectionViewModel PowerPoints { get; }

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
			AugmentMasteryPoints = new ValueCorrectionViewModel(ValueType.Integer);
			AlacrityPoints = new ValueCorrectionViewModel(ValueType.Integer);
			CriticalPoints = new ValueCorrectionViewModel(ValueType.Integer);
			PowerPoints = new ValueCorrectionViewModel(ValueType.Integer);

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

			AugmentMasteryPoints.Difference = correction.MasteryPoints - _configuration.AugmentMasteryPoints;
			AugmentMasteryPoints.NewValue = correction.MasteryPoints;

			AlacrityPoints.Difference = correction.AlacrityPoints - _configuration.AlacrityPoints;
			AlacrityPoints.NewValue = correction.AlacrityPoints;

			CriticalPoints.Difference = correction.CriticalPoints - _configuration.CriticalPoints;
			CriticalPoints.NewValue = correction.CriticalPoints;

			PowerPoints.Difference = correction.PowerPoints - _configuration.PowerPoints;
			PowerPoints.NewValue = correction.PowerPoints;

			Accuracy.NewValue = DpsUtils.GetAccuracy(_configuration.Level, correction.AccuracyPoints) + _configuration.BaseAccuracy;
			Accuracy.Difference = Accuracy.NewValue - _configuration.Accuracy;

			Alacrity.NewValue = DpsUtils.GetAlacrity(_configuration.Level, correction.AlacrityPoints) + _configuration.BaseAlacrity;
			Alacrity.Difference = Alacrity.NewValue - _configuration.Alacrity;

			var newMasteryTotal = (_configuration.BaseMasteryPoints + correction.MasteryPoints) * _configuration.MasteryMultiplier;

			Critical.NewValue = DpsUtils.GetCriticalFromRating(_configuration.Level, correction.CriticalPoints) 
				+ DpsUtils.GetCriticalFromMastery(_configuration.Level, newMasteryTotal) + _configuration.BaseCritical;
			Critical.Difference = Critical.NewValue - _configuration.Critical;

			Surge.NewValue = _configuration.BaseSurge + DpsUtils.GetCriticalFromRating(_configuration.Level, correction.CriticalPoints);
			Surge.Difference = Surge.NewValue - _configuration.Surge;

			BonusDamage.NewValue = DpsUtils.GetBonusDamage(correction.PowerPoints, newMasteryTotal) * _configuration.BonusDamageMultiplier;
			BonusDamage.Difference = BonusDamage.NewValue - _configuration.BonusDamage;

			ForceTechBonusDamage.NewValue = DpsUtils.GetSpellBonusDamage(correction.PowerPoints, _configuration.MainHand.Power + _configuration.OffHand.Power, newMasteryTotal) * _configuration.BonusDamageMultiplier;
			ForceTechBonusDamage.Difference = ForceTechBonusDamage.NewValue - _configuration.SpellBonusDamage;
		}
	}
}

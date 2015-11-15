namespace TorPlayground.DamageModel
{
	public partial class Configuration
	{
		public double Alacrity
		{
			get
			{
				if (_alacrity == null)
					_alacrity = DpsUtils.GetAlacrity(Level, AlacrityPoints);
				return _alacrity.Value + BaseAlacrity;
			}
		}
		private double? _alacrity;

		public double Accuracy
		{
			get
			{
				if (_accuracy == null)
					_accuracy = DpsUtils.GetAccuracy(Level, AccuracyPoints);
				return _accuracy.Value + BaseAccuracy;
			}
		}
		private double? _accuracy;

		public double OffHandAccuracy
		{
			get
			{
				if (_accuracy == null)
					_accuracy = DpsUtils.GetAccuracy(Level, AccuracyPoints);
				return _accuracy.Value + BaseOffHandAccuracy;
			}
		}

		public double MasteryCritical
		{
			get
			{
				if (_masteryCritical == null)
					_masteryCritical = DpsUtils.GetCriticalFromMastery(Level, MasteryPoints * MasteryMultiplier);
				return _masteryCritical.Value;
			}
		}
		private double? _masteryCritical;

		public double CriticalCritical
		{
			get
			{
				if (_criticalCritical == null)
					_criticalCritical = DpsUtils.GetCriticalFromRating(Level, CriticalPoints);
				return _criticalCritical.Value;
			}
		}
		private double? _criticalCritical;

		public double BonusDamage
		{
			get
			{
				if (_bonusDamage == null)
					_bonusDamage = DpsUtils.GetBonusDamage(PowerPoints, MasteryPoints * MasteryMultiplier) * BonusDamageMultiplier;
				return _bonusDamage.Value;
			}
		}
		private double? _bonusDamage;

		public double SpellBonusDamage
		{
			get
			{
				if (_spellBonusDamage == null)
					_spellBonusDamage = DpsUtils.GetSpellBonusDamage(PowerPoints, MainHand.Power + OffHand.Power, MasteryPoints * MasteryMultiplier) * BonusDamageMultiplier;
				return _spellBonusDamage.Value;
			}
		}
		private double? _spellBonusDamage;

		public double Critical => MasteryCritical + CriticalCritical + BaseCritical;
		public double Surge => BaseSurge + CriticalCritical;
		public int MasteryPoints => BaseMasteryPoints + AugmentMasteryPoints;
		public int BuffedMasteryPoints => (int) ((BaseMasteryPoints + AugmentMasteryPoints) * MasteryMultiplier);
		public int Budget => AlacrityPoints + CriticalPoints + PowerPoints + AugmentMasteryPoints + AccuracyPoints;
	}
}

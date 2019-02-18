using Newtonsoft.Json;

namespace Swtor.Dps.DamageModel
{
	public partial class Configuration
	{
		[JsonIgnore]
		public double Alacrity
		{
			get
			{
				if (_alacrity == null)
					_alacrity = this.GetAlacrity();
				return _alacrity.Value + BaseAlacrity;
			}
		}
		private double? _alacrity;

		[JsonIgnore]
		public double Accuracy
		{
			get
			{
				if (_accuracy == null)
					_accuracy = this.GetAccuracy();
				return _accuracy.Value + BaseAccuracy;
			}
		}
		private double? _accuracy;

		[JsonIgnore]
		public double OffHandAccuracy
		{
			get
			{
				if (_accuracy == null)
					_accuracy = this.GetAccuracy();
				return _accuracy.Value + BaseOffHandAccuracy;
			}
		}

		[JsonIgnore]
		public double MasteryCritical
		{
			get
			{
				if (_masteryCritical == null)
					_masteryCritical = this.GetCriticalFromMastery();
				return _masteryCritical.Value;
			}
		}
		private double? _masteryCritical;

		[JsonIgnore]
		public double CriticalCritical
		{
			get
			{
				if (_criticalCritical == null)
					_criticalCritical = this.GetCriticalFromRating();
				return _criticalCritical.Value;
			}
		}
		private double? _criticalCritical;

		[JsonIgnore]
		public double CriticalSurge
		{
			get
			{
				if (_criticalSurge == null)
					_criticalSurge = this.GetSurge();
				return _criticalSurge.Value;
			}
		}
		private double? _criticalSurge;

		[JsonIgnore]
		public double BonusDamage
		{
			get
			{
				if (_bonusDamage == null)
					_bonusDamage = this.GetBonusDamage() * BonusDamageMultiplier;
				return _bonusDamage.Value;
			}
		}
		private double? _bonusDamage;

		[JsonIgnore]
		public double SpellBonusDamage
		{
			get
			{
				if (_spellBonusDamage == null)
					_spellBonusDamage = this.GetSpellBonusDamage() * BonusDamageMultiplier;
				return _spellBonusDamage.Value;
			}
		}
		private double? _spellBonusDamage;

		[JsonIgnore]
		public double Critical => MasteryCritical + CriticalCritical + BaseCritical;

		[JsonIgnore]
		public double Surge => BaseSurge + CriticalSurge;

		[JsonIgnore]
		public int MasteryPoints => BaseMasteryPoints + ExtraMasteryPoints;

		[JsonIgnore]
		public int PowerPoints => BasePowerPoints + ExtraPowerPoints;

		[JsonIgnore]
		public double BuffedMasteryPoints => MasteryPoints * MasteryMultiplier;

		[JsonIgnore]
		public int Budget => AlacrityPoints + CriticalPoints + ExtraPowerPoints + ExtraMasteryPoints + AccuracyPoints;
	}
}

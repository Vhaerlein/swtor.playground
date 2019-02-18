namespace Swtor.Dps.DamageModel
{
	public partial class Configuration
	{
		public int Level { get; set; }

		public delegate void DualWieldUpdatedHandler();

		public event DualWieldUpdatedHandler DualWieldUpdated;

		public bool DualWield
		{
			get { return _dualWield; }
			set
			{
				if (_dualWield != value)
				{
					_dualWield = value;
					var handler = DualWieldUpdated;
					handler?.Invoke();
				}
			}
		}

		private bool _dualWield;

		public Hand MainHand
		{
			get { return _mainHand; }
			set
			{
				if (_mainHand != value)
				{
					if (_mainHand != null)
						_mainHand.HandPowerUpdated -= UpdateBonusDamages;

					_mainHand = value;
					_mainHand.HandPowerUpdated += UpdateBonusDamages;
					UpdateBonusDamages();
				}
			}
		}

		private Hand _mainHand;

		public Hand OffHand
		{
			get { return _offHand; }
			set
			{
				if (_offHand != value)
				{
					if (_offHand != null)
						_offHand.HandPowerUpdated -= UpdateBonusDamages;

					_offHand = value;
					_offHand.HandPowerUpdated += UpdateBonusDamages;
					UpdateBonusDamages();
				}
			}
		}

		private Hand _offHand;

		public int StandardDamage
		{
			get
			{
				if (_standardDamage <= 0 && Data.DataManager.StandardDamage.ContainsKey(Level))
					return Data.DataManager.StandardDamage[Level];

				return _standardDamage;
			}
			set { _standardDamage = value; }
		}

		private int _standardDamage;

		public int StandardHealth
		{
			get
			{
				if (_standardHealth <= 0 && Data.DataManager.StandardDamage.ContainsKey(Level))
					return Data.DataManager.StandardHealth[Level];

				return _standardHealth;
			}
			set { _standardHealth = value; }
		}

		private int _standardHealth;

		public int BaseMasteryPoints
		{
			get { return _baseMasteryPoints; }
			set
			{
				_baseMasteryPoints = value;
				_masteryCritical = null;
				UpdateBonusDamages();
			}
		}

		private int _baseMasteryPoints;

		public int BasePowerPoints
		{
			get { return _basePowerPoints; }
			set
			{
				_basePowerPoints = value;
				UpdateBonusDamages();
			}
		}

		private int _basePowerPoints;

		public int ExtraMasteryPoints
		{
			get { return _extraMasteryPoints; }
			set
			{
				_extraMasteryPoints = value;
				_masteryCritical = null;
				UpdateBonusDamages();
			}
		}

		private int _extraMasteryPoints;

		public int AlacrityPoints
		{
			get { return _alacrityPoints; }
			set
			{
				if (_alacrityPoints != value)
				{
					_alacrityPoints = value;
					_alacrity = null;
				}
			}
		}

		private int _alacrityPoints;

		public int CriticalPoints
		{
			get { return _criticalPoints; }
			set
			{
				if (_criticalPoints != value)
				{
					_criticalPoints = value;
					_criticalCritical = null;
					_criticalSurge = null;
				}
			}
		}

		private int _criticalPoints;

		public int AccuracyPoints
		{
			get { return _accuracyPoints; }
			set
			{
				if (_accuracyPoints != value)
				{
					_accuracyPoints = value;
					_accuracy = null;
				}
			}
		}

		private int _accuracyPoints;

		public int ExtraPowerPoints
		{
			get { return _extraPowerPoints; }
			set
			{
				if (_extraPowerPoints != value)
				{
					_extraPowerPoints = value;
					UpdateBonusDamages();
				}
			}
		}

		private int _extraPowerPoints;

		public double MasteryMultiplier
		{
			get { return _masteryMultiplier; }
			set
			{
				_masteryMultiplier = value;
				_masteryCritical = null;
				UpdateBonusDamages();
			}
		}

		private double _masteryMultiplier;

		public double BonusDamageMultiplier
		{
			get { return _bonusDamageMultiplier; }
			set
			{
				_bonusDamageMultiplier = value;
				UpdateBonusDamages();
			}
		}

		private double _bonusDamageMultiplier;

		public double BaseAccuracy { get; set; }
		public double BaseAlacrity { get; set; }
		public double BaseSurge { get; set; }
		public double BaseOffHandAccuracy { get; set; }
		public double BaseCritical { get; set; }

		public Configuration()
		{
			MasteryMultiplier = 1.05;
			BonusDamageMultiplier = 1.05;
			Level = 65;
			BaseSurge = .51;
			BaseAlacrity = .03;
			BaseAccuracy = .01;
			BaseCritical = (5.0 + 5.0 + 1.0) / 100.0;
		}

		public Configuration Clone()
		{
			return new Configuration
			{
				AccuracyPoints = AccuracyPoints,
				BaseAccuracy = BaseAccuracy,
				Level = Level,
				AlacrityPoints = AlacrityPoints,
				ExtraMasteryPoints = ExtraMasteryPoints,
				BaseAlacrity = BaseAlacrity,
				BaseCritical = BaseCritical,
				BaseMasteryPoints = BaseMasteryPoints,
				BasePowerPoints = BasePowerPoints,
				BaseOffHandAccuracy = BaseOffHandAccuracy,
				BaseSurge = BaseSurge,
				BonusDamageMultiplier = BonusDamageMultiplier,
				CriticalPoints = CriticalPoints,
				DualWield = DualWield,
				MainHand = MainHand,
				OffHand = OffHand,
				MasteryMultiplier = MasteryMultiplier,
				ExtraPowerPoints = ExtraPowerPoints,
				StandardDamage = StandardDamage,
				StandardHealth = StandardHealth
			};
		}

		private void UpdateBonusDamages()
		{
			_bonusDamage = null;
			_spellBonusDamage = null;
		}
	}
}

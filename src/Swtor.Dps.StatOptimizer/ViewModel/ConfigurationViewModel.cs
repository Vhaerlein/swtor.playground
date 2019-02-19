using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Swtor.Dps.DamageModel;
using TorSimulator.StatOptimizer.Annotations;

namespace Swtor.Dps.StatOptimizer.ViewModel
{
	public class ConfigurationViewModel : INotifyPropertyChanged
	{
		private readonly Dictionary<int, int> _minDamage = new Dictionary<int, int>
		{
			{216, 780},
			{220, 794},
			{224, 808},
			{246, 1169},
			{252, 1270}
		};
		private readonly Dictionary<int, int> _maxDamage = new Dictionary<int, int>
		{
			{216, 1171},
			{220, 1191},
			{224, 1212},
			{246, 1754},
			{252, 1904}
		};
		private readonly Dictionary<int, int> _power = new Dictionary<int, int>
		{
			{216, 1561},
			{220, 1589},
			{224, 1616},
			{246, 2339},
			{252, 2540}
		};

		public CorrectionViewModel CorrectionViewModel { get; }

		public int Level
		{
			get => _configuration.Level;
			set
			{
				if (_configuration.Level != value)
				{
					_configuration.Level = value;
					OnPropertyChanged();
					RaiseUpdateOnCalculatedProperties();
				}
			}
		}

		public double BaseAccuracy
		{
			get => _configuration.BaseAccuracy;
			set
			{
				_configuration.BaseAccuracy = value;
				OnPropertyChanged();
				RaiseUpdateOnCalculatedProperties();
			}
		}

		public double BaseOffHandAccuracy
		{
			get => _configuration.BaseOffHandAccuracy;
			set
			{
				_configuration.BaseOffHandAccuracy = value;
				OnPropertyChanged();
				RaiseUpdateOnCalculatedProperties();
			}
		}

		public double BaseAlacrity
		{
			get => _configuration.BaseAlacrity;
			set
			{
				_configuration.BaseAlacrity = value;
				OnPropertyChanged();
				RaiseUpdateOnCalculatedProperties();
			}
		}

		public double BaseCritical
		{
			get => _configuration.BaseCritical;
			set
			{
				_configuration.BaseCritical = value;
				OnPropertyChanged();
				RaiseUpdateOnCalculatedProperties();
			}
		}

		public double BaseSurge
		{
			get => _configuration.BaseSurge;
			set
			{
				_configuration.BaseSurge = value;
				OnPropertyChanged();
				RaiseUpdateOnCalculatedProperties();
			}
		}

		public int BaseMasteryPoints
		{
			get => _configuration.BaseMasteryPoints;
			set
			{
				if (_configuration.BaseMasteryPoints != value)
				{
					_configuration.BaseMasteryPoints = value;
					OnPropertyChanged();
					RaiseUpdateOnCalculatedProperties();
				}
			}
		}

		public int MainHandMin
		{
			get => _configuration.MainHand.DamageMin;
			set
			{
				if (_configuration.MainHand.DamageMin != value)
				{
					_configuration.MainHand.DamageMin = value;
					OnPropertyChanged();
					RaiseUpdateOnCalculatedProperties();
				}
			}
		}

		public int MainHandMax
		{
			get => _configuration.MainHand.DamageMax;
			set
			{
				if (_configuration.MainHand.DamageMax != value)
				{
					_configuration.MainHand.DamageMax = value;
					OnPropertyChanged();
					RaiseUpdateOnCalculatedProperties();
				}
			}
		}

		public int MainHandPower
		{
			get => _configuration.MainHand.Power;
			set
			{
				if (_configuration.MainHand.Power != value)
				{
					_configuration.MainHand.Power = value;
					OnPropertyChanged();
					RaiseUpdateOnCalculatedProperties();
				}
			}
		}

		public int OffHandMin
		{
			get => _configuration.OffHand.DamageMin;
			set
			{
				if (_configuration.OffHand.DamageMin != value)
				{
					_configuration.OffHand.DamageMin = value;
					OnPropertyChanged();
					RaiseUpdateOnCalculatedProperties();
				}
			}
		}

		public int OffHandMax
		{
			get => _configuration.OffHand.DamageMax;
			set
			{
				if (_configuration.OffHand.DamageMax != value)
				{
					_configuration.OffHand.DamageMax = value;
					OnPropertyChanged();
					RaiseUpdateOnCalculatedProperties();
				}
			}
		}

		public int OffHandPower
		{
			get => _configuration.OffHand.Power;
			set
			{
				if (_configuration.OffHand.Power != value)
				{
					_configuration.OffHand.Power = value;
					OnPropertyChanged();
					RaiseUpdateOnCalculatedProperties();
				}
			}
		}

		public double OffHandMultiplier
		{
			get => _configuration.OffHand.Multiplier;
			set
			{
				_configuration.OffHand.Multiplier = value;
				OnPropertyChanged();
				RaiseUpdateOnCalculatedProperties();
			}
		}

		public int ExtraMasteryPoints
		{
			get => _configuration.ExtraMasteryPoints;
			set
			{
				if (_configuration.ExtraMasteryPoints != value)
				{
					_configuration.ExtraMasteryPoints = value;
					OnPropertyChanged();
					RaiseUpdateOnCalculatedProperties();
				}
			}
		}

		public double MasteryPoints => _configuration.BuffedMasteryPoints;

		public int AccuracyPoints
		{
			get => _configuration.AccuracyPoints;
			set
			{
				if (_configuration.AccuracyPoints != value)
				{
					_configuration.AccuracyPoints = value;
					OnPropertyChanged();
					RaiseUpdateOnCalculatedProperties();
				}
			}
		}

		public int AlacrityPoints
		{
			get => _configuration.AlacrityPoints;
			set
			{
				if (_configuration.AlacrityPoints != value)
				{
					_configuration.AlacrityPoints = value;
					OnPropertyChanged();
					RaiseUpdateOnCalculatedProperties();
				}
			}
		}

		public int CriticalPoints
		{
			get => _configuration.CriticalPoints;
			set
			{
				if (_configuration.CriticalPoints != value)
				{
					_configuration.CriticalPoints = value;
					OnPropertyChanged();
					RaiseUpdateOnCalculatedProperties();
				}
			}
		}

		public int ExtraPowerPoints
		{
			get => _configuration.ExtraPowerPoints;
			set
			{
				if (_configuration.ExtraPowerPoints != value)
				{
					_configuration.ExtraPowerPoints = value;
					OnPropertyChanged();
					RaiseUpdateOnCalculatedProperties();
				}
			}
		}

		public int BasePowerPoints
		{
			get => _configuration.BasePowerPoints;
			set
			{
				if (_configuration.BasePowerPoints != value)
				{
					_configuration.BasePowerPoints = value;
					OnPropertyChanged();
					RaiseUpdateOnCalculatedProperties();
				}
			}
		}

		public int PowerPoints => _configuration.PowerPoints;

		public bool DualWield
		{
			get => _configuration.DualWield;
			set
			{
				if (_configuration.DualWield != value)
				{
					_configuration.DualWield = value;
					OnPropertyChanged();
					RaiseUpdateOnCalculatedProperties();
				}
			}
		}

		public delegate void VoidEventHandler();
		public event VoidEventHandler ConfigurationUpdated;
		private void RaiseUpdateOnCalculatedProperties()
		{
			OnPropertyChanged(nameof(Accuracy));
			OnPropertyChanged(nameof(Alacrity));
			OnPropertyChanged(nameof(Critical));
			OnPropertyChanged(nameof(Surge));
			OnPropertyChanged(nameof(BonusDamage));
			OnPropertyChanged(nameof(SpellBonusDamage));
			OnPropertyChanged(nameof(Budget));
			OnPropertyChanged(nameof(MasteryPoints));
			OnPropertyChanged(nameof(PowerPoints));

			var handler = ConfigurationUpdated;
			handler?.Invoke();
		}

		public double Accuracy => _configuration.Accuracy;
		public double Alacrity => _configuration.Alacrity;
		public double Critical => _configuration.Critical;
		public double Surge => _configuration.Surge;
		public double BonusDamage => _configuration.BonusDamage;
		public double SpellBonusDamage => _configuration.SpellBonusDamage;
		public int Budget => _configuration.Budget;

		public CommandHandler SetMainHandCommand { get; private set; }
		public CommandHandler SetOffHandCommand { get; private set; }

		private readonly Configuration _configuration;

		public ConfigurationViewModel(Configuration configuration)
		{
			_configuration = configuration;
			CorrectionViewModel = new CorrectionViewModel(configuration);
			SetMainHandCommand = new CommandHandler(p =>
			{
				int rating = int.Parse(p.ToString());
				MainHandMin = _minDamage[rating];
				MainHandMax = _maxDamage[rating];
				MainHandPower = _power[rating];
			});
			SetOffHandCommand = new CommandHandler(p =>
			{
				int rating = int.Parse(p.ToString());
				OffHandMin = _minDamage[rating];
				OffHandMax = _maxDamage[rating];
				OffHandPower = _power[rating];
			});
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		private void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TorPlayground.DamageModel;
using TorSimulator.StatOptimizer.Annotations;

namespace TorPlayground.StatOptimizer.ViewModel
{
	public class ConfigurationViewModel : INotifyPropertyChanged
	{
		private readonly Dictionary<int, int> _minDamage = new Dictionary<int, int>
		{
			{216, 780},
			{220, 794},
			{224, 808}
		};
		private readonly Dictionary<int, int> _maxDamage = new Dictionary<int, int>
		{
			{216, 1171},
			{220, 1191},
			{224, 1212}
		};
		private readonly Dictionary<int, int> _power = new Dictionary<int, int>
		{
			{216, 1561},
			{220, 1589},
			{224, 1616}
		};

		public CorrectionViewModel CorrectionViewModel { get; private set; }

		public int Level
		{
			get { return _configuration.Level; }
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
			get { return _configuration.BaseAccuracy; }
			set
			{
				_configuration.BaseAccuracy = value;
				OnPropertyChanged();
				RaiseUpdateOnCalculatedProperties();
			}
		}

		public double BaseOffHandAccuracy
		{
			get { return _configuration.BaseOffHandAccuracy; }
			set
			{
				_configuration.BaseOffHandAccuracy = value;
				OnPropertyChanged();
				RaiseUpdateOnCalculatedProperties();
			}
		}

		public double BaseCritical 
		{
			get { return _configuration.BaseCritical; }
			set
			{
				_configuration.BaseCritical = value;
				OnPropertyChanged();
				RaiseUpdateOnCalculatedProperties();
			}
		}

		public double BaseSurge
		{
			get { return _configuration.BaseSurge; }
			set
			{
				_configuration.BaseSurge = value;
				OnPropertyChanged();
				RaiseUpdateOnCalculatedProperties();
			}
		}

		public int BaseMasteryPoints
		{
			get { return _configuration.BaseMasteryPoints; }
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
			get { return _configuration.MainHand.DamageMin; }
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
			get { return _configuration.MainHand.DamageMax; }
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
			get { return _configuration.MainHand.Power; }
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
			get { return _configuration.OffHand.DamageMin; }
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
			get { return _configuration.OffHand.DamageMax; }
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
			get { return _configuration.OffHand.Power; }
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
			get { return _configuration.OffHand.Multiplier; }
			set
			{
				_configuration.OffHand.Multiplier = value;
				OnPropertyChanged();
				RaiseUpdateOnCalculatedProperties();
			}
		}

		public int AugmentMasteryPoints
		{
			get { return _configuration.AugmentMasteryPoints; }
			set
			{
				if (_configuration.AugmentMasteryPoints != value)
				{
					_configuration.AugmentMasteryPoints = value;
					OnPropertyChanged();
					RaiseUpdateOnCalculatedProperties();
				}
			}
		}

		public double MasteryPoints => _configuration.MasteryPoints * _configuration.MasteryMultiplier;

		public int AccuracyPoints
		{
			get { return _configuration.AccuracyPoints; }
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
			get { return _configuration.AlacrityPoints; }
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
			get { return _configuration.CriticalPoints; }
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

		public int PowerPoints
		{
			get { return _configuration.PowerPoints; }
			set
			{
				if (_configuration.PowerPoints != value)
				{
					_configuration.PowerPoints = value;
					OnPropertyChanged();
					RaiseUpdateOnCalculatedProperties();
				}
			}
		}

		public bool DualWield
		{
			get { return _configuration.DualWield; }
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
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}

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
		public Configuration Configuration { get; }

		public int Level
		{
			get { return Configuration.Level; }
			set
			{
				if (Configuration.Level != value)
				{
					Configuration.Level = value;
					OnPropertyChanged();
					RaiseUpdateOnCalculatedProperties();
				}
			}
		}

		public double BaseAccuracy 
		{
			get { return Configuration.BaseAccuracy; }
			set
			{
				Configuration.BaseAccuracy = value;
				OnPropertyChanged();
				RaiseUpdateOnCalculatedProperties();
			}
		}

		public double BaseOffHandAccuracy
		{
			get { return Configuration.BaseOffHandAccuracy; }
			set
			{
				Configuration.BaseOffHandAccuracy = value;
				OnPropertyChanged();
				RaiseUpdateOnCalculatedProperties();
			}
		}

		public double BaseCritical 
		{
			get { return Configuration.BaseCritical; }
			set
			{
				Configuration.BaseCritical = value;
				OnPropertyChanged();
				RaiseUpdateOnCalculatedProperties();
			}
		}

		public double BaseSurge
		{
			get { return Configuration.BaseSurge; }
			set
			{
				Configuration.BaseSurge = value;
				OnPropertyChanged();
				RaiseUpdateOnCalculatedProperties();
			}
		}

		public int BaseMasteryPoints
		{
			get { return Configuration.BaseMasteryPoints; }
			set
			{
				if (Configuration.BaseMasteryPoints != value)
				{
					Configuration.BaseMasteryPoints = value;
					OnPropertyChanged();
					RaiseUpdateOnCalculatedProperties();
				}
			}
		}

		public int MainHandMin
		{
			get { return Configuration.MainHand.DamageMin; }
			set
			{
				if (Configuration.MainHand.DamageMin != value)
				{
					Configuration.MainHand.DamageMin = value;
					OnPropertyChanged();
					RaiseUpdateOnCalculatedProperties();
				}
			}
		}

		public int MainHandMax
		{
			get { return Configuration.MainHand.DamageMax; }
			set
			{
				if (Configuration.MainHand.DamageMax != value)
				{
					Configuration.MainHand.DamageMax = value;
					OnPropertyChanged();
					RaiseUpdateOnCalculatedProperties();
				}
			}
		}

		public int MainHandPower
		{
			get { return Configuration.MainHand.Power; }
			set
			{
				if (Configuration.MainHand.Power != value)
				{
					Configuration.MainHand.Power = value;
					OnPropertyChanged();
					RaiseUpdateOnCalculatedProperties();
				}
			}
		}

		public int OffHandMin
		{
			get { return Configuration.OffHand.DamageMin; }
			set
			{
				if (Configuration.OffHand.DamageMin != value)
				{
					Configuration.OffHand.DamageMin = value;
					OnPropertyChanged();
					RaiseUpdateOnCalculatedProperties();
				}
			}
		}

		public int OffHandMax
		{
			get { return Configuration.OffHand.DamageMax; }
			set
			{
				if (Configuration.OffHand.DamageMax != value)
				{
					Configuration.OffHand.DamageMax = value;
					OnPropertyChanged();
					RaiseUpdateOnCalculatedProperties();
				}
			}
		}

		public int OffHandPower
		{
			get { return Configuration.OffHand.Power; }
			set
			{
				if (Configuration.OffHand.Power != value)
				{
					Configuration.OffHand.Power = value;
					OnPropertyChanged();
					RaiseUpdateOnCalculatedProperties();
				}
			}
		}

		public double OffHandMultiplier
		{
			get { return Configuration.OffHand.Multiplier; }
			set
			{
				Configuration.OffHand.Multiplier = value;
				OnPropertyChanged();
				RaiseUpdateOnCalculatedProperties();
			}
		}

		public int AugmentMasteryPoints
		{
			get { return Configuration.AugmentMasteryPoints; }
			set
			{
				if (Configuration.AugmentMasteryPoints != value)
				{
					Configuration.AugmentMasteryPoints = value;
					OnPropertyChanged();
					RaiseUpdateOnCalculatedProperties();
				}
			}
		}

		public int MasteryPoints => (int) Math.Round(Configuration.MasteryPoints * Configuration.MasteryMultiplier);

		public int AccuracyPoints
		{
			get { return Configuration.AccuracyPoints; }
			set
			{
				if (Configuration.AccuracyPoints != value)
				{
					Configuration.AccuracyPoints = value;
					OnPropertyChanged();
					RaiseUpdateOnCalculatedProperties();
				}
			}
		}

		public int AlacrityPoints
		{
			get { return Configuration.AlacrityPoints; }
			set
			{
				if (Configuration.AlacrityPoints != value)
				{
					Configuration.AlacrityPoints = value;
					OnPropertyChanged();
					RaiseUpdateOnCalculatedProperties();
				}
			}
		}

		public int CriticalPoints
		{
			get { return Configuration.CriticalPoints; }
			set
			{
				if (Configuration.CriticalPoints != value)
				{
					Configuration.CriticalPoints = value;
					OnPropertyChanged();
					RaiseUpdateOnCalculatedProperties();
				}
			}
		}

		public int PowerPoints
		{
			get { return Configuration.PowerPoints; }
			set
			{
				if (Configuration.PowerPoints != value)
				{
					Configuration.PowerPoints = value;
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

		public double Accuracy => Configuration.Accuracy;
		public double Alacrity => Configuration.Alacrity;
		public double Critical => Configuration.Critical;
		public double Surge => Configuration.Surge;
		public double BonusDamage => Configuration.BonusDamage;
		public double SpellBonusDamage => Configuration.SpellBonusDamage;
		public int Budget => Configuration.Budget;

		public CommandHandler SetMainHandCommand { get; private set; }
		public CommandHandler SetOffHandCommand { get; private set; }

		public ConfigurationViewModel(Configuration configuration)
		{
			Configuration = configuration;
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

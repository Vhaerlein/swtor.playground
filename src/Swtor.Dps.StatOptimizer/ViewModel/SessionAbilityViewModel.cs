using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;
using Swtor.Dps.DamageModel;
using Swtor.Dps.Resources.Abilities;
using Swtor.Dps.StatOptimizer.Utils;

namespace Swtor.Dps.StatOptimizer.ViewModel
{
	public class SessionAbilityViewModel : NotifyPropertyChangedObject
	{
		public string Name => _sessionAbility.Ability.Name;

		public int Activations
		{
			get => _sessionAbility.Activations;
			set
			{
				if (_sessionAbility.Activations != value)
				{
					_sessionAbility.Activations = value;
					OnSessionAbilityUpdated();
				}
			}
		}

		public double DamageMultiplier
		{
			get => _sessionAbility.DamageMultiplier;
			set
			{
				_sessionAbility.DamageMultiplier = value;
				OnSessionAbilityUpdated();
			}
		}

		public double Autocrit
		{
			get => _sessionAbility.Autocrit;
			set
			{
				_sessionAbility.Autocrit = value;
				OnSessionAbilityUpdated();
			}
		}

		public double ArmorReduction
		{
			get => _sessionAbility.ArmorReduction;
			set
			{
				_sessionAbility.ArmorReduction = value;
				OnSessionAbilityUpdated();
			}
		}

		public double SurgeBonus
		{
			get => _sessionAbility.SurgeBonus;
			set
			{
				_sessionAbility.SurgeBonus = value;
				OnSessionAbilityUpdated();
			}
		}

		public bool ForceOffHand
		{
			get => _sessionAbility.ForceOffHand;
			set
			{
				_sessionAbility.ForceOffHand = value;
				OnSessionAbilityUpdated();
			}
		}

		public bool CanForceOffHand
		{
			get => _canForceOffHand;
			set
			{
				_canForceOffHand = value;
				OnPropertyChanged();
			}
		}
		private bool _canForceOffHand;

		public bool DualWield
		{
			get => _dualWield;
			set
			{
				_dualWield = value;
				OnPropertyChanged();
			}
		}
		private bool _dualWield;

		public double PlainDamageMin => _sessionAbility.Ability.GetAbilityDamageMin(_configuration);
		public double PlainDamageMax => _sessionAbility.Ability.GetAbilityDamageMax(_configuration);
		public double SessionDamageAvg => _sessionAbility.GetAbilityDamageAvg(_configuration, _session);

		public ImageSource Icon => _sessionAbility.Ability.Icon.ToImageSource();

		public string Info
		{
			get
			{
				if (string.IsNullOrEmpty(_info))
				{
					var sb = new StringBuilder("Type: ");
					sb.Append(string.Join(", ",
						(
							from token in _sessionAbility.Ability.Tokens.Where(t => t.Type == TokenType.Damage)
							from action in token.Coefficients
							select action.Type
						).Distinct()));

					sb.Append("\nDamage type: ");
					sb.Append(string.Join(", ",
						(
							from token in _sessionAbility.Ability.Tokens.Where(t => t.Type == TokenType.Damage)
							from action in token.Coefficients
							select action.DamageType
						).Distinct()));

					if (_sessionAbility.Ability.CooldownTime.HasValue)
						sb.AppendFormat("\nCooldown: {0:0.#}", _sessionAbility.Ability.CooldownTime);
					if (_sessionAbility.Ability.IgnoresAlacrity)
						sb.Append("\nIgnores alacrity");

					sb.AppendFormat("\n\n{0}", _sessionAbility.Ability.Description.Replace("<<1>>", $"{PlainDamageMin:0}-{PlainDamageMax:0}"));
					_info = sb.ToString();
				}

				return _info;
			}
		}
		private string _info;

		public string ParseInfo => _sessionAbility.Info;

		private readonly SessionAbility _sessionAbility;
		private readonly Configuration _configuration;
		private readonly Session _defaultValues;
		private readonly Session _session;

		public delegate void SessionAbilityUpdatedHandler();
		public event SessionAbilityUpdatedHandler SessionAbilityUpdated;

		public ICommand SaveAsDefaultCommand { get; set; }

		public SessionAbilityViewModel(SessionAbility sessionAbility, Configuration configuration, Session session, Session defaultValues)
		{
			_sessionAbility = sessionAbility;
			_configuration = configuration;
			_defaultValues = defaultValues;
			_session = session;
			UpdateCanForceOffHand();
			configuration.DualWieldUpdated += UpdateCanForceOffHand;
			SaveAsDefaultCommand = new CommandHandler(SaveAsDefault);
		}

		private void SaveAsDefault()
		{
			var ability = _defaultValues.Abilities.FirstOrDefault(a => a.Ability.NameId == _sessionAbility.Ability.NameId);
			if (ability == null)
			{
				ability = new SessionAbility {Id = _sessionAbility.Ability.NameId};
				_defaultValues.Abilities.Add(ability);
			}
			ability.SurgeBonus = SurgeBonus;
			ability.Autocrit = Autocrit;
			ability.DamageMultiplier = DamageMultiplier;
			ability.ArmorReduction = ArmorReduction;
			ability.ForceOffHand = ForceOffHand;
		}

		private void UpdateCanForceOffHand()
		{
			DualWield = _configuration.DualWield;
			CanForceOffHand = !_sessionAbility.HasOffHandActions && _configuration.DualWield;
		}

		private void OnSessionAbilityUpdated()
		{
			OnPropertyChanged(nameof(SessionDamageAvg));
			var handler = SessionAbilityUpdated;
			handler?.Invoke();
		}

		public void UpdateNumbers()
		{
			OnPropertyChanged(nameof(Info));
			OnPropertyChanged(nameof(PlainDamageMin));
			OnPropertyChanged(nameof(PlainDamageMax));
			OnPropertyChanged(nameof(SessionDamageAvg));
		}
	}
}

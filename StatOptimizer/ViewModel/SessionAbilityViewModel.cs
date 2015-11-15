using System.Linq;
using System.Text;
using System.Windows.Media;
using TorPlayground.DamageModel;
using TorPlayground.Resources.Abilities;
using TorPlayground.StatOptimizer.Utils;

namespace TorPlayground.StatOptimizer.ViewModel
{
	public class SessionAbilityViewModel
	{
		public string Name => _sessionAbility.Ability.Name;

		public int Activations
		{
			get { return _sessionAbility.Activations; }
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
			get { return _sessionAbility.DamageMultiplier; }
			set
			{
				_sessionAbility.DamageMultiplier = value;
				OnSessionAbilityUpdated();
			}
		}

		public double Autocrit
		{
			get { return _sessionAbility.Autocrit; }
			set
			{
				_sessionAbility.Autocrit = value;
				OnSessionAbilityUpdated();
			}
		}

		public double ArmorReduction
		{
			get { return _sessionAbility.ArmorReduction; }
			set
			{
				_sessionAbility.ArmorReduction = value;
				OnSessionAbilityUpdated();
			}
		}

		public double DamageMin => _sessionAbility.Ability.GetAbilityDamageMin(_configuration);
		public double DamageMax => _sessionAbility.Ability.GetAbilityDamageMax(_configuration);
		public double DamageAvg => (DamageMax + DamageMin) / 2.0;

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
					_info = sb.ToString();
				}

				return _info;
			}
		}
		private string _info;

		private readonly SessionAbility _sessionAbility;
		private readonly Configuration _configuration;

		public delegate void SessionAbilityUpdatedHandler();
		public event SessionAbilityUpdatedHandler SessionAbilityUpdated;

		public SessionAbilityViewModel(SessionAbility sessionAbility, Configuration configuration)
		{
			_sessionAbility = sessionAbility;
			_configuration = configuration;
		}

		private void OnSessionAbilityUpdated()
		{
			var handler = SessionAbilityUpdated;
			handler?.Invoke();
		}
	}
}

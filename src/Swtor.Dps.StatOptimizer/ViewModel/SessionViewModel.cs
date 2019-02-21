using System.Collections.ObjectModel;
using Swtor.Dps.DamageModel;

namespace Swtor.Dps.StatOptimizer.ViewModel
{
	public class SessionViewModel
	{
		public Session Session { get; }

		public string Name
		{
			get => Session.Name;
			set => Session.Name = value;
		}

		public string Description
		{
			get => Session.Description;
			set => Session.Description = value;
		}

		public double Duration
		{
			get => Session.Duration;
			set
			{
				Session.Duration = value;
				OnSessionUpdated();
			}
		}

		public double EnergyKineticDamageReduction
		{
			get => Session.EnergyKineticDamageReduction;
			set
			{
				Session.EnergyKineticDamageReduction = value;
				OnSessionUpdated();
			}
		}

		public double ElementalInternalDamageReduction
		{
			get => Session.ElementalInternalDamageReduction;
			set
			{
				Session.ElementalInternalDamageReduction = value;
				OnSessionUpdated();
			}
		}

		public double DefenseChance
		{
			get => Session.DefenseChance;
			set
			{
				Session.DefenseChance = value;
				OnSessionUpdated();
			}
		}

		public ObservableCollection<SessionAbilityViewModel> Abilities { get; }

		public bool Active
		{
			get => Session.Active;
			set => Session.Active = value;
		}

		public delegate void SessionUpdatedHandler();
		public event SessionUpdatedHandler SessionUpdated;

		public SessionViewModel(Session session, Configuration configuration, Session defaultValues)
		{
			Session = session;
			Abilities = new ObservableCollection<SessionAbilityViewModel>();
			foreach (var sessionAbility in session.Abilities)
			{
				var ability = new SessionAbilityViewModel(sessionAbility, configuration, session, defaultValues);
				ability.SessionAbilityUpdated += OnSessionUpdated;
				Abilities.Add(ability);
			}
		}

		private void OnSessionUpdated()
		{
			var handler = SessionUpdated;
			handler?.Invoke();
		}
	}
}

using System.Collections.ObjectModel;
using TorPlayground.DamageModel;

namespace TorPlayground.StatOptimizer.ViewModel
{
	public class SessionViewModel
	{
		public Session Session { get; }

		public string Name
		{
			get { return Session.Name; }
			set { Session.Name = value; }
		}

		public double Duration
		{
			get { return Session.Duration; }
			set
			{
				Session.Duration = value;
				OnSessionUpdated();
			}
		}

		public double EnergyKineticDamageReduction
		{
			get { return Session.EnergyKineticDamageReduction; }
			set
			{
				Session.EnergyKineticDamageReduction = value;
				OnSessionUpdated();
			}
		}

		public double ElementalInternalDamageReduction
		{
			get { return Session.ElementalInternalDamageReduction; }
			set
			{
				Session.ElementalInternalDamageReduction = value;
				OnSessionUpdated();
			}
		}

		public double DefenseChance
		{
			get { return Session.DefenseChance; }
			set
			{
				Session.DefenseChance = value;
				OnSessionUpdated();
			}
		}

		public ObservableCollection<SessionAbilityViewModel> Abilities { get; }

		public bool Active 
		{
			get { return Session.Active; } 
			set { Session.Active = value; } 
		}

		public delegate void SessionUpdatedHandler();
		public event SessionUpdatedHandler SessionUpdated;

		public SessionViewModel(Session session, Configuration configuration, Session defaultValues)
		{
			Session = session;
			Abilities = new ObservableCollection<SessionAbilityViewModel>();
			foreach (var sessionAbility in session.Abilities)
			{
				var ability = new SessionAbilityViewModel(sessionAbility, configuration, defaultValues);
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

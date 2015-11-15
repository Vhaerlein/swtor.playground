using System.Collections.ObjectModel;
using TorPlayground.DamageModel;

namespace TorPlayground.StatOptimizer.ViewModel
{
	public class SessionViewModel
	{
		private readonly Session _session;

		public string Name
		{
			get { return _session.Name; }
			set { _session.Name = value; }
		}

		public double Duration
		{
			get { return _session.Duration; }
			set
			{
				_session.Duration = value;
				OnSessionUpdated();
			}
		}

		public double DamageReduction
		{
			get { return _session.EnergyKineticDamageReduction; }
			set
			{
				_session.EnergyKineticDamageReduction = value;
				OnSessionUpdated();
			}
		}

		public ObservableCollection<SessionAbilityViewModel> Abilities { get; }

		public bool Active 
		{
			get { return _session.Active; } 
			set { _session.Active = value; } 
		}

		public delegate void SessionUpdatedHandler();
		public event SessionUpdatedHandler SessionUpdated;

		public SessionViewModel(Session session, Configuration configuration)
		{
			_session = session;
			Abilities = new ObservableCollection<SessionAbilityViewModel>();
			foreach (var sessionAbility in session.Abilities)
			{
				var ability = new SessionAbilityViewModel(sessionAbility, configuration);
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

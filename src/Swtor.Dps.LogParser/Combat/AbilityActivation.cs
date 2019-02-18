using System.Collections.Generic;
using Swtor.Dps.LogParser.Log;

namespace Swtor.Dps.LogParser.Combat
{
	public class AbilityActivation : TargetGroup
	{
		public bool TriggeredActivation { get; }
		public IReadOnlyList<AbilityAction> Actions
		{
			get
			{
				if (_actions == null || _actions.Count != _actionsInternal.Count)
					_actions = _actionsInternal.AsReadOnly();
				return _actions;
			}
		}
		private IReadOnlyList<AbilityAction> _actions;
		private readonly List<AbilityAction> _actionsInternal = new List<AbilityAction>();

		internal AbilityActivation(bool triggeredActivation = false)
		{
			TriggeredActivation = triggeredActivation;
		}

		internal override void OnLogEntryAdded(LogEntry entry)
		{
			base.OnLogEntryAdded(entry);

			if (entry.Type == EntryType.ApplyEffect && (entry.Action == LogEntry.DamageAction || entry.Action == LogEntry.HealAction))
				_actionsInternal.Add(new AbilityAction(entry.Who, entry.Amount?.Value ?? 0, entry.Amount?.IsCritical ?? false, entry.Action == LogEntry.HealAction));
		}
	}
}

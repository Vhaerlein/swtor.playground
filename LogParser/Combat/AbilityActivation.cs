using System.Collections.Generic;
using TorPlayground.LogParser.Log;

namespace TorPlayground.LogParser.Combat
{
	public class AbilityActivation : TargetGroup
	{
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

		internal override void OnLogEntryAdded(LogEntry entry)
		{
			base.OnLogEntryAdded(entry);

			if (entry.Type == EntryType.ApplyEffect && (entry.Action == LogEntry.DamageAction || entry.Action == LogEntry.HealAction))
				_actionsInternal.Add(new AbilityAction(entry.Who, entry.Amount?.Value ?? 0, entry.Action == LogEntry.HealAction));
		}
	}
}

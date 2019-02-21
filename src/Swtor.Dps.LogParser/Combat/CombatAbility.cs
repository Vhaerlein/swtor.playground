using System.Collections.Generic;
using System.Linq;
using Swtor.Dps.LogParser.Log;

namespace Swtor.Dps.LogParser.Combat
{
	public class CombatAbility : TargetGroup
	{
		public string Name { get; internal set; }
		public string Id { get; internal set; }

		public IReadOnlyList<AbilityActivation> Activations
		{
			get
			{
				if (_activations == null || _activations.Count != _activationsInternal.Count)
					_activations = _activationsInternal.AsReadOnly();
				return _activations;
			}
		}
		private IReadOnlyList<AbilityActivation> _activations;

		private readonly List<AbilityActivation> _activationsInternal = new List<AbilityActivation>();

		internal override void OnLogEntryAdded(LogEntry entry)
		{
			base.OnLogEntryAdded(entry);

			if (entry.Type == EntryType.Event && entry.Action == LogEntry.AbilityActivateAction)
				_activationsInternal.Add(new AbilityActivation());

			var activation = _activationsInternal.LastOrDefault();

			if (activation != null && activation.TriggeredActivation
				&& entry.Type == EntryType.ApplyEffect && entry.Action == entry.AbilityName && entry.AbilityId == entry.ActionId)
			{
				activation = new AbilityActivation(true);
				_activationsInternal.Add(activation);
			}

			if (activation == null)
			{
				activation = new AbilityActivation(true);
				_activationsInternal.Add(activation);
			}

			activation.AddLogEntry(entry);
		}
	}
}

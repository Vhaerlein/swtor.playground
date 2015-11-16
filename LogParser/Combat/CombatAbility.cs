﻿using System.Collections.Generic;
using System.Linq;
using TorPlayground.LogParser.Log;

namespace TorPlayground.LogParser.Combat
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
			
			// for some reason there was no ability activate event
			if (activation == null)
			{
				activation = new AbilityActivation();
				_activationsInternal.Add(activation);
			}

			activation.AddLogEntry(entry);
		}
	}
}

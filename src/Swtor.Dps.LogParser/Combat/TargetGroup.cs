using System.Collections.Generic;
using System.Linq;
using Swtor.Dps.LogParser.Log;

namespace Swtor.Dps.LogParser.Combat
{
	public abstract class TargetGroup : LogEntryGroup
	{
		public IReadOnlyList<Target> Targets
		{
			get
			{
				if (_targets == null || _targets.Count != _targetsInternal.Count)
					_targets = _targetsInternal.AsReadOnly();
				return _targets;
			}
		}
		private IReadOnlyList<Target> _targets;
		private readonly List<Target> _targetsInternal = new List<Target>();

		internal override void OnLogEntryAdded(LogEntry entry)
		{
			var target = _targetsInternal.FirstOrDefault(t => t.Character == entry.Target);
			if (target == null)
			{
				target = new Target(entry.Target);
				_targetsInternal.Add(target);
			}
			target.AddLogEntry(entry);
		}
	}
}

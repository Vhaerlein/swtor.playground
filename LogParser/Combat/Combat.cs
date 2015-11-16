using System;
using System.Collections.Generic;
using TorPlayground.LogParser.Log;

namespace TorPlayground.LogParser.Combat
{
	public class Combat
	{
		public DateTime StartTime { get; internal set; }
		public DateTime EndTime { get; internal set; }
		public double Duration => (EndTime - StartTime).TotalMilliseconds / 1000.0;
		public double DamageDone { get; internal set; }

		public IReadOnlyList<LogEntry> LogEntries { get; internal set; }

		public List<CombatAbility> Abilities { get; internal set; }

		internal Combat()
		{
		}
	}
}

using System.Collections.Generic;
using TorPlayground.LogParser.Log;

namespace TorPlayground.LogParser.Combat
{
	public class CombatAbility
	{
		public string Name { get; internal set; }
		public string Id { get; internal set; }

		public int DamageTotal { get; internal set; }
		public int HealingTotal { get; internal set; }

		public IReadOnlyList<AbilityActivation> Activations { get; internal set; }

		public IReadOnlyList<LogEntry> LogEntries { get; internal set; }
	}
}

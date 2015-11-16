using System.Linq;
using TorPlayground.LogParser.Log;

namespace TorPlayground.LogParser.Combat
{
	public abstract class DamageHealingGroup : LogEntryGroup
	{
		public int DamageDone { get; private set; }
		public int HealingDone { get; private set; }

		internal override void OnLogEntryAdded(LogEntry entry)
		{
			DamageDone = LogEntries.Where(e => e.Type == EntryType.ApplyEffect && e.Action == LogEntry.DamageAction && e.Amount != null).Sum(e => e.Amount.Value);
			HealingDone = LogEntries.Where(e => e.Type == EntryType.ApplyEffect && e.Action == LogEntry.HealAction && e.Amount != null).Sum(e => e.Amount.Value);
		}
	}
}

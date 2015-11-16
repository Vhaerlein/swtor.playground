using System.Collections.Generic;
using TorPlayground.LogParser.Log;

namespace TorPlayground.LogParser.Combat
{
	public class AbilityActivation
	{
		public IReadOnlyList<LogEntry> LogEntries { get; internal set; }
	}
}

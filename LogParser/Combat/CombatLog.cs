using System;
using System.Collections.Generic;
using System.IO;
using TorPlayground.LogParser.Log;

namespace TorPlayground.LogParser.Combat
{
	public class CombatLog
	{
		public IReadOnlyList<LogEntry> Entries { get; }

		public IReadOnlyList<Combat> Combats { get; }

		private CombatLog(IReadOnlyList<LogEntry> entries)
		{
			Entries = entries;
			var combats = new List<Combat>();

			Combat combat = null;

			foreach (var entry in Entries)
			{
				if (entry.Type == EntryType.Event && entry.Action == LogEntry.EnterCombatAction)
				{
					combat = new Combat(entry.Who);
					combats.Add(combat);
				}

				combat?.AddLogEntry(entry);

				if (entry.Type == EntryType.Event && entry.Action == LogEntry.ExitCombatAction)
					combat = null;
			}

			Combats = combats.AsReadOnly();
		}

		public static CombatLog Load(string filePath)
		{
			if (!File.Exists(filePath))
				return null;

			var fileTime = File.GetLastWriteTime(filePath);
			return Parse(File.ReadAllLines(filePath), fileTime);
		}

		public static CombatLog Parse(string logString)
		{
			if (string.IsNullOrEmpty(logString))
				return null;

			return Parse(logString.Split('\n'), DateTime.Now);
		}

		private static CombatLog Parse(string[] logLines, DateTime time)
		{
			var entries = new List<LogEntry>();
			LogEntry lastEntry = null;
			foreach (var line in logLines)
			{
				var entry = LogEntry.Parse(line.Trim(), time);

				if (lastEntry != null && entry.Time < lastEntry.Time)
					entries.ForEach(e => e.Time = e.Time.AddDays(-1));

				lastEntry = entry;
				entries.Add(LogEntry.Parse(line));
			}
			return new CombatLog(entries.AsReadOnly());
		}
	}
}

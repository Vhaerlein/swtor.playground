using System;
using System.Collections.Generic;
using System.IO;

namespace TorPlayground.LogParser.Log
{
	public class CombatLog
	{
		public IReadOnlyList<LogEntry> LogEntries { get; private set; }

		private CombatLog()
		{
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
			return new CombatLog
			{
				LogEntries = entries.AsReadOnly()
			};
		}
	}
}

using System.Collections.Generic;

namespace TorPlayground.LogParser.Log
{
	public abstract class LogEntryGroup
	{
		public IReadOnlyList<LogEntry> LogEntries
		{
			get
			{
				if (_logEntries == null || _logEntries.Count != _logEntriesInternal.Count)
					_logEntries = _logEntriesInternal.AsReadOnly();
				return _logEntries;
			}
		}
		private IReadOnlyList<LogEntry> _logEntries;

		private readonly List<LogEntry> _logEntriesInternal = new List<LogEntry>();

		internal virtual void AddLogEntry(LogEntry entry)
		{
			if (entry != null)
			{
				_logEntriesInternal.Add(entry);
				OnLogEntryAdded(entry);
			}
		}

		internal abstract void OnLogEntryAdded(LogEntry entry);
	}
}

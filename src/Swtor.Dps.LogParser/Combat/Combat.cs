using System;
using System.Collections.Generic;
using System.Linq;
using Swtor.Dps.LogParser.Log;

namespace Swtor.Dps.LogParser.Combat
{
	public class Combat : TargetGroup
	{
		public DateTime StartTime { get; private set; }
		public DateTime EndTime { get; private set; }
		public Character Player { get; }

		public string Description { get; }

		/// <summary>
		/// Combat duration in seconds.
		/// </summary>
		public double Duration => (EndTime - StartTime).TotalMilliseconds / 1000.0;

		public int DamageDone
		{
			get
			{
				if (_damageDoneEntriesCount != LogEntries.Count)
				{
					_damageDoneEntriesCount = LogEntries.Count;
					_damageDone = LogEntries
						.Where(e => e.Who == Player && e.Type == EntryType.ApplyEffect && e.Action == LogEntry.DamageAction && e.Amount != null)
						.Sum(e => e.Amount.Value);
				}
				return _damageDone;
			}
		}
		private int _damageDone;
		private int _damageDoneEntriesCount;

		public double Dps => DamageDone / Duration;

		public IReadOnlyList<CombatAbility> Abilities
		{
			get
			{
				if (_abilities == null || _abilities.Count != _abilitiesInternal.Count)
					_abilities = _abilitiesInternal.AsReadOnly();
				return _abilities;
			}
		}
		private IReadOnlyList<CombatAbility> _abilities;

		private readonly List<CombatAbility> _abilitiesInternal = new List<CombatAbility>();

		internal Combat(Character player, string description)
		{
			Player = player;
			Description = description;
		}

		internal override void OnLogEntryAdded(LogEntry entry)
		{
			StartTime = LogEntries.Min(e => e.Time);
			EndTime = LogEntries.Max(e => e.Time);

			if (entry.Who != Player)
				return;

			base.OnLogEntryAdded(entry);

			if (string.IsNullOrEmpty(entry.AbilityId))
				return;

			var ability = _abilitiesInternal.FirstOrDefault(a => a.Id == entry.AbilityId);
			if (ability == null)
			{
				ability = new CombatAbility
				{
					Id = entry.AbilityId,
					Name = entry.AbilityName
				};
				_abilitiesInternal.Add(ability);
			}

			ability.AddLogEntry(entry);
		}
	}
}

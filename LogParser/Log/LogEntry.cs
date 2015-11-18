using System;
using System.Text.RegularExpressions;

namespace TorPlayground.LogParser.Log
{
	public class LogEntry
	{
		public static readonly string DamageAction = "Damage";
		public static readonly string HealAction = "Heal";
		public static readonly string AbilityActivateAction = "AbilityActivate";
		public static readonly string EnterCombatAction = "EnterCombat";
		public static readonly string ExitCombatAction = "ExitCombat";

		public static readonly Regex LogEntryRegex = new Regex
		(
			@"^\[(?<Hour>\d{2}):(?<Minute>\d{2}):(?<Second>\d{2})\.(?<Millisecond>\d{3})\]\s
			\[((?<IsPlayer>@)?(?<Who>[^\{]+)?(?<WhoId>\s\{\d+\}(:\d+)?)?)?\]\s
			\[((?<TargetIsPlayer>@)?(?<Target>[^\{]+)?(?<TargetId>\s\{\d+\}(:\d+)?)?)?\]\s
			\[((?<AbilityName>[^\{]+)?\s(\{(?<AbilityId>\d+)\}))?\]\s
			\[(?<EntryType>[^\{]+)\s(\{(?<EntryTypeId>\d+)\}):(\s(?<Action>[^\{]+)?)(\s\{(?<ActionId>\d+)\})\]\s
			\((
				(?<Amount>\d+)(?<IsCritical>\*)?(\s((?<AmountType>[^\{\-]+)\s\{(?<AmountTypeId>\d+)\})?)?
				(\s-((?<AmountModifier>[^\{]+)\s\{(?<AmountModifierTypeId>\d+)\})?)?
				(\s?\(((?<Mitigation>\d+)\s)?(?<MitigationType>[^\{]+)\s\{(?<MitigationTypeId>\d+)\}\))?
			)?\)
			(\s<(?<Threat>-?\d+)>)?$",
			RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace
		);

		public DateTime Time { get; internal set; }
		public Character Who { get; private set; }
		public Character Target { get; private set; }
		public string AbilityName { get; private set; }
		public string AbilityId { get; private set; }
		public EntryType Type { get; private set; }
		public string Action { get; private set; }
		public string ActionId { get; private set; }
		public Amount Amount { get; private set; }
		public Mitigation Mitigation { get; private set; }

		private LogEntry() { }

		public static LogEntry Parse(string input)
		{
			return Parse(input, DateTime.UtcNow);
		}

		public static LogEntry Parse(string input, DateTime day)
		{
			var match = LogEntryRegex.Match(input);
			if (!match.Success)
				throw new Exception($"Can't parse combat log string \"{input}\".");
			try
			{
				var entry = new LogEntry
				{
					Time = new DateTime
					(
						day.Year,
						day.Month,
						day.Day,
						int.Parse(match.Groups["Hour"].Value),
						int.Parse(match.Groups["Minute"].Value),
						int.Parse(match.Groups["Second"].Value),
						int.Parse(match.Groups["Millisecond"].Value)
					)
				};

				if (match.Groups["Who"].Success || match.Groups["WhoId"].Success)
				{
					entry.Who = new Character
					(
						match.Groups["Who"].Success ? match.Groups["Who"].Value : match.Groups["WhoId"].Value,
						match.Groups["IsPlayer"].Success
					);
				}

				if (match.Groups["Target"].Success || match.Groups["TargetId"].Success)
				{
					entry.Target = new Character
					(
						match.Groups["Target"].Success ? match.Groups["Target"].Value : match.Groups["TargetId"].Value,
						match.Groups["TargetIsPlayer"].Success
					);
				}

				if (match.Groups["AbilityName"].Success)
				{
					entry.AbilityName = match.Groups["AbilityName"].Value;
					entry.AbilityId = match.Groups["AbilityId"].Value;
				}

				EntryType type;
				entry.Type = Enum.TryParse(match.Groups["EntryType"].Value, true, out type) ? type : EntryType.Unknown;

				entry.Action = match.Groups["Action"].Value;
				entry.ActionId = match.Groups["ActionId"].Value;

				if (match.Groups["Amount"].Success)
				{
					AmountType amountType;
					AmountModifier amountModifier;
					entry.Amount = new Amount
					{
						Value = int.Parse(match.Groups["Amount"].Value),
						Type = match.Groups["AmountType"].Success && Enum.TryParse(match.Groups["AmountType"].Value, true, out amountType) ? amountType : AmountType.Generic,
						Modifier = match.Groups["AmountModifier"].Success && Enum.TryParse(match.Groups["AmountModifier"].Value, true, out amountModifier)
							? amountModifier
							: AmountModifier.None,
						IsCritical = match.Groups["IsCritical"].Success
					};
				}

				if (match.Groups["Mitigation"].Success)
				{
					MitigationType mitigationType;
					entry.Mitigation = new Mitigation
					{
						Value = int.Parse(match.Groups["Mitigation"].Value),
						Type = match.Groups["MitigationType"].Success && Enum.TryParse(match.Groups["MitigationType"].Value, true, out mitigationType)
							? mitigationType
							: MitigationType.Unknown
					};
				}

				return entry;
			}
			catch (Exception ex)
			{
				throw new Exception($"Can't parse combat log string \"{input}\".\nError message: \"{ex.Message}\".");
			}
		}
	}
}

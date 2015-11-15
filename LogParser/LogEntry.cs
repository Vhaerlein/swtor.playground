using System;
using System.Text.RegularExpressions;

namespace TorPlayground.LogParser
{
	// [01:09:17.454] [@Vhaerlein] [@Vhaerlein] [] [Event {836045448945472}: EnterCombat {836045448945489}] ()
	// [01:09:25.388] [@Vhaerlein] [@Vhaerlein] [Force Leap {812105301229568}] [Event {836045448945472}: AbilityActivate {836045448945479}] ()
	// [01:09:26.022] [@Vhaerlein] [Eternal Warden {1784013450641408}:13604008676603] [Force Leap {812105301229568}] [ApplyEffect {836045448945477}: Damage {836045448945501}] (2833* energy {836045448940874}) <2125>
	// [01:09:26.828] [@Vhaerlein] [@Vhaerlein] [Zealous Strike {996200484438016}] [Event {836045448945472}: AbilityActivate {836045448945479}] ()
	// [01:09:27.334] [@Vhaerlein] [Eternal Warden {1784013450641408}:13604008676603] [Zealous Strike {996200484438016}] [ApplyEffect {836045448945477}: Damage {836045448945501}] (1558* energy {836045448940874}) <1168>
	// [01:09:27.462] [@Vhaerlein] [Eternal Warden {1784013450641408}:13604008676603] [Zealous Strike {996200484438016}] [ApplyEffect {836045448945477}: Damage {836045448945501}] (375* energy {836045448940874}) <281>
	// [01:09:29.007] [@Vhaerlein] [Eternal Warden {1784013450641408}:13604008676603] [Burning Slices {3630205142827008}] [ApplyEffect {836045448945477}: Burning Slices {3630205142827008}] ()
	// [01:09:29.008] [@Vhaerlein] [Eternal Warden {1784013450641408}:13604008676603] [Blade Dance {3619557918900224}] [ApplyEffect {836045448945477}: Damage {836045448945501}] (0 -miss {836045448945502}) <1>
	// [01:09:29.008] [@Vhaerlein] [Eternal Warden {1784013450641408}:13604008676603] [Blade Dance {3619557918900224}] [ApplyEffect {836045448945477}: Damage {836045448945501}] (2692 energy {836045448940874} -shield {836045448945509}) <2019>
	// [01:09:30.737] [@Vhaerlein] [Eternal Warden {1784013450641408}:13604008676603] [Blade Dance {3619557918900224}] [ApplyEffect {836045448945477}: Damage {836045448945501}] (8228 energy {836045448940874}) <6171>
	// [01:09:30.737] [@Vhaerlein] [Eternal Warden {1784013450641408}:13604008676603] [Blade Dance {3619557918900224}] [ApplyEffect {836045448945477}: Damage {836045448945501}] (0 -miss {836045448945502}) <1>
	// [01:09:30.647] [@Hхly] [@Vhaerlein] [Successive Treatment {3393470840438784}] [ApplyEffect {836045448945477}: Heal {836045448945500}] (6068*) <1900>
	// [01:09:31.679] [@Vhaerlein] [@Vhaerlein] [Precision {2209167968305152}] [RemoveEffect {836045448945478}: Precision {2209167968305152}] ()
	// [01:09:32.742] [@Vhaerlein] [Eternal Warden {1784013450641408}:13604008676603] [Dispatch {3461554662014976}] [ApplyEffect {836045448945477}: Damage {836045448945501}] (6320 energy {836045448940874}) <4740>
	// [01:09:59.676] [@Vhaerlein] [@Vhaerlein] [] [Event {836045448945472}: ExitCombat {836045448945490}] ()
	// [01:12:11.209] [@Vhaerlein] [Escaped Infernal Thrall {2018269556899840}:13604009668170] [] [Event {836045448945472}: Death {836045448945493}] ()
	// [00:56:54.579] [Annihilation Droid XRR-3 {2034573252755456}:13604008912435] [@Vhaerlein] [Missile Salvo {2508441289490432}] [ApplyEffect {836045448945477}: Damage {836045448945501}] (34085 energy {836045448940874}) <34085>
	// [00:56:54.579] [Annihilation Droid XRR-3 {2034573252755456}:13604008912435] [@Vhaerlein] [] [Event {836045448945472}: Death {836045448945493}] ()
	// [00:57:10.666] [@Vhaerlein] [@Vhaerlein] [] [Event {836045448945472}: Revived {836045448945494}] ()
	// [01:31:27.209] [@Vhaerlein] [Proto Acklay {2021018335969280}:13604008684537] [Focused Slash {3478012976693248}] [ApplyEffect {836045448945477}: Beat Down (Physical) {3478012976693516}] ()
	// [01:38:18.469] [@Vhaerlein] [@Vhaerlein] [Rebuke {2480816059842560}] [Event {836045448945472}: AbilityActivate {836045448945479}] ()
	// [01:38:18.470] [@Vhaerlein] [@Vhaerlein] [Rebuke {2480816059842560}] [ApplyEffect {836045448945477}: Rebuke {2480816059842560}] ()
	// [00:51:27.032] [@Vhaerlein] [@Vhaerlein] [Strike {947362411315200}] [Event {836045448945472}: AbilityActivate {836045448945479}] ()
	// [00:51:27.033] [@Vhaerlein] [Annihilation Droid XRR-3 {2034573252755456}:13604008912435] [Strike {947362411315200}] [ApplyEffect {836045448945477}: Damage {836045448945501}] (709 energy {836045448940874}) <709>

	public class LogEntry
	{
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

		public DateTime Time { get; private set; }
		public Character Who { get; private set; }
		public Character Target { get; private set; }
		public string AbilityName { get; private set; }
		public string AbilityId { get; private set; }
		public EntryType Type { get; private set; }
		public string Action { get; private set; }
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
					{
						Name = match.Groups["Who"].Success ? match.Groups["Who"].Value : match.Groups["WhoId"].Value,
						IsPlayer = match.Groups["IsPlayer"].Success
					};
				}

				if (match.Groups["Target"].Success || match.Groups["TargetId"].Success)
				{
					entry.Target = new Character
					{
						Name = match.Groups["Target"].Success ? match.Groups["Target"].Value : match.Groups["TargetId"].Value,
						IsPlayer = match.Groups["TargetIsPlayer"].Success
					};
				}

				if (match.Groups["AbilityName"].Success)
				{
					entry.AbilityName = match.Groups["AbilityName"].Value;
					entry.AbilityId = match.Groups["AbilityId"].Value;
				}

				EntryType type;
				entry.Type = Enum.TryParse(match.Groups["EntryType"].Value, true, out type) ? type : EntryType.Unknown;

				entry.Action = match.Groups["Action"].Value;

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
							: AmountModifier.None
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
				throw new Exception($"Can't parse combat log string \"{input}\".\nError message: {ex.Message}.");
			}
		}
	}
}

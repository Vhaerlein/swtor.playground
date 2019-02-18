using System;
using System.Xml.Serialization;

namespace Swtor.Dps.Resources.Abilities
{
	[Serializable]
	public class Action
	{
		[XmlAttribute("Name")]
		public ActionType Type { get; set; }

		[XmlElement]
		public DamageType DamageType { get; set; }

		[XmlElement]
		public int Slot { get; set; }

		[XmlElement]
		public SpellType SpellType { get; set; }

		[XmlElement]
		public bool IsSpecialAbility { get; set; }

		[XmlElement]
		public bool IgnoreDualWieldModifier { get; set; }

		[XmlElement]
		public int FlurryBlowsMin { get; set; }

		[XmlElement]
		public int FlurryBlowsMax { get; set; }

		[XmlElement]
		public double ThreatPercent { get; set; }

		[XmlElement]
		public double StandardHealthPercentMin { get; set; }

		[XmlElement]
		public double StandardHealthPercentMax { get; set; }

		public double StandardHealthPercentAvg => (StandardHealthPercentMin + StandardHealthPercentMax) / 2.0;

		[XmlElement]
		public int AmountModifierFixedMin { get; set; }

		[XmlElement]
		public int AmountModifierFixedMax { get; set; }

		[XmlElement]
		public double AmountModifierMin { get; set; }

		[XmlElement]
		public double AmountModifierMax { get; set; }

		[XmlElement]
		public double AmountModifierPercent { get; set; }

		[XmlElement]
		public double Coefficient { get; set; }

		[XmlElement]
		public double HealingPowerCoefficient { get; set; }

		[XmlElement]
		public double HealthStealPercentage { get; set; }

		public Action()
		{
			IgnoreDualWieldModifier = true;
		}
	}
}

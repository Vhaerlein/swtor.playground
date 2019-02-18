using System;
using System.Xml.Serialization;

namespace Swtor.Dps.Resources.Abilities
{
	[Serializable]
	public class AbsorbCoefficient
	{
		[XmlAttribute]
		public int Id { get; set; }

		[XmlElement]
		public int AmountMax { get; set; }

		[XmlElement]
		public int AmountMin { get; set; }

		[XmlElement]
		public double AmountPercent { get; set; }

		[XmlElement]
		public double StandardHealthPercentMin { get; set; }

		[XmlElement]
		public double StandardHealthPercentMax { get; set; }

		[XmlElement]
		public int MaxDamageAbsorbed { get; set; }

		[XmlElement]
		public double HealingPowerCoefficient { get; set; }
	}
}

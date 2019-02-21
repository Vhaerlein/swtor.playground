using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Serialization;

namespace Swtor.Dps.Resources.Abilities
{
	[Serializable]
	public class Ability
	{
		[XmlAttribute]
		public string Id { get; set; }

		[XmlAttribute]
		public string Sid { get; set; }

		[XmlAttribute]
		public string NameId { get; set; }

		[XmlElement]
		public string Name { get; set; }

		[XmlElement]
		public string Description { get; set; }

		[XmlElement]
		public bool IsPassive { get; set; }

		[XmlElement]
		public bool IsHidden { get; set; }

		[XmlArray]
		[XmlArrayItem(ElementName = "Token")]
		public List<Token> Tokens { get; set; }

		public Bitmap Icon => _icon ?? (_icon = DataManager.GetIcon(IconName));
		private Bitmap _icon;

		[XmlElement("Icon")]
		public string IconName { get; set; }

		[XmlElement]
		public double MinRange { get; set; }

		[XmlElement]
		public double MaxRange { get; set; }

		[XmlElement]
		public int EnergyCost { get; set; }

		[XmlElement]
		public int ForceCost { get; set; }

		[XmlElement]
		public int AmmoHeatCost { get; set; }

		[XmlElement]
		public double CastingTime { get; set; }

		[XmlElement]
		public double? CooldownTime { get; set; }

		[XmlElement]
		public string SharedCooldown { get; set; }

		[XmlElement]
		public double ChannelingTime { get; set; }

		[XmlElement]
		public double Gcd { get; set; }

		[XmlElement]
		public bool OverridesGcd { get; set; }

		[XmlElement]
		public bool UsesPushback { get; set; }

		[XmlElement]
		public bool IgnoresAlacrity { get; set; }

		[XmlElement]
		public bool LoSCheck { get; set; }

		[XmlElement]
		public int TargetArc { get; set; }

		[XmlElement]
		public int TargetArcOffset { get; set; }

		[XmlElement]
		public TargetRule TargetRule { get; set; }

		[XmlArray]
		[XmlArrayItem(ElementName = "AbsorbCoefficient")]
		public List<AbsorbCoefficient> AbsorbCoefficients { get; set; }

		[XmlElement]
		public string ModalGroup { get; set; }
	}
}

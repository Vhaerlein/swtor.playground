using System;
using System.Xml.Serialization;

namespace TorPlayground.Resources.Abilities
{
	[Serializable]
	public enum SpellType
	{
		[XmlEnum("1")]
		Heal,

		[XmlEnum("2")]
		Tech,

		[XmlEnum("3")]
		Force
	}
}

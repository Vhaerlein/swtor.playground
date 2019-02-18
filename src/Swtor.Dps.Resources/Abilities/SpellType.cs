using System;
using System.Xml.Serialization;

namespace Swtor.Dps.Resources.Abilities
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

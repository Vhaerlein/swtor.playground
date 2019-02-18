using System;
using System.Xml.Serialization;

namespace Swtor.Dps.Resources.Abilities
{
	[Serializable]
	public enum DamageType
	{
		Weapon = 0,

		[XmlEnum("1")]
		Kinetic,

		[XmlEnum("2")]
		Energy,

		[XmlEnum("3")]
		Elemental,

		[XmlEnum("4")]
		Internal
	}
}

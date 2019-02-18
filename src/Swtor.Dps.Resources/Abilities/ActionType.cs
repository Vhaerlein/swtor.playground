using System;
using System.Xml.Serialization;

namespace Swtor.Dps.Resources.Abilities
{
	[Serializable]
	public enum ActionType
	{
		Unknown = 0,

		[XmlEnum]
		SpellDamage,

		[XmlEnum]
		WeaponDamage,

		[XmlEnum]
		Heal,

		[XmlEnum]
		GodDamage
	}
}

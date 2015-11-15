using System;
using System.Xml.Serialization;

namespace TorPlayground.Resources.Abilities
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

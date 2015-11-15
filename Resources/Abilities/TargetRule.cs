using System;
using System.Xml.Serialization;

namespace TorPlayground.Resources.Abilities
{
	[Serializable]
	public enum TargetRule
	{
		Unknown = 0,

		[XmlEnum]
		Self,

		[XmlEnum]
		Companion,

		[XmlEnum]
		Attackable,

		[XmlEnum]
		GroundSelf,

		[XmlEnum]
		Friendly,

		[XmlEnum]
		Ground,

		[XmlEnum]
		Any,

		[XmlEnum]
		FriendlyDead,

		[XmlEnum]
		PBAoE,

		[XmlEnum]
		Grouped,

		[XmlEnum]
		SelfDead,

		[XmlEnum]
		SelfStunned,
	
		[XmlEnum]
		LootHopper,

		[XmlEnum]
		SelfDeadorAlive,

		[XmlEnum]
		Vendor,

		[XmlEnum]
		Vehicle,

		[XmlEnum]
		VehicleOwner,

		[XmlEnum]
		CoverObjectLeft,

		[XmlEnum]
		CoverObjectRight,

		[XmlEnum]
		CoverObject
	}
}

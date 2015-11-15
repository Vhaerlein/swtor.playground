using System;
using System.Xml.Serialization;

namespace TorPlayground.Resources.Abilities
{
	[Serializable]
	public enum TokenType
	{
		Unknown = 0,

		[XmlEnum]
		Damage,

		[XmlEnum]
		Duration,

		[XmlEnum]
		Rank,

		[XmlEnum]
		Healing,

		[XmlEnum]
		Bindpoint
	}
}

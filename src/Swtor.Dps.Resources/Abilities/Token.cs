using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Swtor.Dps.Resources.Abilities
{
	[Serializable]
	public class Token
	{
		[XmlAttribute]
		public int Id { get; set; }

		[XmlArray]
		[XmlArrayItem(ElementName = "Action")]
		public List<Action> Coefficients { get; set; }

		[XmlElement]
		public string DescriptionToken { get; set; }

		[XmlElement]
		public double Multiplier { get; set; }

		[XmlElement]
		public TokenType Type { get; set; }

		[XmlElement]
		public int Effect { get; set; }

		[XmlElement]
		public int SubEffect { get; set; }

		public Token()
		{
			Multiplier = 1;
		}
	}
}

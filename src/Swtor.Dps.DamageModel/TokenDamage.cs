using System.Collections.Generic;

namespace Swtor.Dps.DamageModel
{
	public class TokenDamage : List<ActionDamage>
	{
		public double Multiplier { get; set; }
	}
}

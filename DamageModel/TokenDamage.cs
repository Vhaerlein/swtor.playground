using System.Collections.Generic;

namespace TorPlayground.DamageModel
{
	public class TokenDamage : List<ActionDamage>
	{
		public double Multiplier { get; set; }
	}
}

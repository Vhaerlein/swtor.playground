using TorPlayground.LogParser.Log;

namespace TorPlayground.LogParser.Combat
{
	public class AbilityAction
	{
		public Character Target { get; }
		public int Amount { get; }
		public bool IsHealing { get; }

		public AbilityAction(Character target, int amount, bool isHealing = false)
		{
			Target = target;
			Amount = amount;
			IsHealing = isHealing;
		}
	}
}

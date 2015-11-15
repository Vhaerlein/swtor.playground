using TorPlayground.Resources.Abilities;

namespace TorPlayground.DamageModel
{
	public struct ActionDamage
	{
		public bool IsOffHand { get; }
		public double Damage { get; }
		public DamageType DamageType { get; }
		public ActionType Type { get; }

		public ActionDamage(bool isOffHand, double damage, DamageType damageType, ActionType type) : this()
		{
			IsOffHand = isOffHand;
			Damage = damage;
			DamageType = damageType;
			Type = type;
		}
	}
}

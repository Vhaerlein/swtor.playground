﻿using Swtor.Dps.Resources.Abilities;

namespace Swtor.Dps.DamageModel
{
	public struct ActionDamage
	{
		public bool IsOffHand { get; }
		public double Damage { get; }
		public DamageType DamageType { get; }

		public ActionDamage(bool isOffHand, double damage, DamageType damageType) : this()
		{
			IsOffHand = isOffHand;
			Damage = damage;
			DamageType = damageType;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using Swtor.Dps.Resources.Abilities;
using Action = Swtor.Dps.Resources.Abilities.Action;

namespace Swtor.Dps.DamageModel
{
	public static class DpsUtils
	{
		private static readonly object Lock = new object();

		public static ConfigurationCorrection FindHighestDpsCorrection(Profile profile)
		{
			return FindHighestDpsCorrection(profile, profile.Configuration.Budget);
		}

		public static ConfigurationCorrection FindHighestDpsCorrection(Profile profile, Session session)
		{
			var budget = profile.Configuration.Budget;
			return FindHighestDpsCorrection(session, profile.Configuration.Clone(), new ConfigurationCorrection(), budget, budget / 10, budget);
		}

		public static ConfigurationCorrection FindHighestDpsCorrection(Profile profile, int budget)
		{
			return FindHighestDpsCorrection(profile.ActiveSession, profile.Configuration.Clone(), new ConfigurationCorrection(), budget, budget / 10, budget);
		}

		private static ConfigurationCorrection FindHighestDpsCorrection(Session session, Configuration configuration, ConfigurationCorrection correction, int budget, int step, int lastStep)
		{
			// ncalc is not thread-safe, we need lock here since we're using static instance of formulas
			lock (Lock)
			{
				correction.Dps = 0;
				var range = (lastStep + 1) * 2 + 1;
				var al = Math.Max(correction.AlacrityPoints - lastStep - 1, 0);
				var pw = Math.Max(correction.PowerPoints - lastStep - 1, 0);
				var cr = Math.Max(correction.CriticalPoints - lastStep - 1, 0);
				var ac = Math.Max(correction.AccuracyPoints - lastStep - 1, 0);

				for
				(
					configuration.ExtraPowerPoints = pw;
					configuration.ExtraPowerPoints <= Math.Min(budget, pw + range);
					configuration.ExtraPowerPoints += step
				)
				{
					for
					(
						configuration.CriticalPoints = cr;
						configuration.CriticalPoints <= Math.Min(budget - configuration.ExtraPowerPoints, cr + range);
						configuration.CriticalPoints += step
					)
					{
						for
						(
							configuration.AlacrityPoints = al;
							configuration.AlacrityPoints <= Math.Min(budget - configuration.ExtraPowerPoints - configuration.CriticalPoints, al + range);
							configuration.AlacrityPoints += step
						)
						{
							for
							(
								configuration.AccuracyPoints = ac;
								configuration.AccuracyPoints <= Math.Min(budget - configuration.ExtraPowerPoints - configuration.CriticalPoints - configuration.AlacrityPoints, ac + range);
								configuration.AccuracyPoints += step
							)
							{
								configuration.ExtraMasteryPoints = budget - configuration.ExtraPowerPoints - configuration.CriticalPoints - configuration.AlacrityPoints - configuration.AccuracyPoints;
								var dps = CalculateDps(session, configuration);

								if (correction.Dps < dps)
								{
									correction.Dps = dps;
									correction.AccuracyPoints = configuration.AccuracyPoints;
									correction.CriticalPoints = configuration.CriticalPoints;
									correction.MasteryPoints = configuration.ExtraMasteryPoints;
									correction.AlacrityPoints = configuration.AlacrityPoints;
									correction.PowerPoints = configuration.ExtraPowerPoints;
								}
							}
						}
					}
				}

				if (step == 1)
					return correction;

				return FindHighestDpsCorrection(session, configuration, correction, budget, Math.Max(step / 2, 1), step);
			}
		}

		public static double CalculateDps(Profile profile)
		{
			return CalculateDps(profile.ActiveSession, profile.Configuration);
		}

		public static double CalculateDps(Session session, Configuration configuration)
		{
			lock (Lock)
			{
				if (session == null)
					return 0;

				return session.Abilities.Sum
					(
						a => GetAbilityDamage(configuration, a, session.EnergyKineticDamageReduction, session.ElementalInternalDamageReduction, session.DefenseChance)
							* a.Activations
							* a.DamageMultiplier
							* (a.Ability.IgnoresAlacrity ? 1 : 1 + configuration.Alacrity)
					)
					/ session.Duration;
			}
		}

		public static double GetAbilityDamageMin(this Ability ability, Configuration configuration, bool forceOffhand = false)
		{
			return ability?.GetAbilityTokenDamageList(configuration, DamageRange.Minimum, forceOffhand).Sum(d => d.Sum(t => t.Damage) * d.Multiplier) ?? 0;
		}

		public static double GetAbilityDamageMax(this Ability ability, Configuration configuration, bool forceOffhand = false)
		{
			return ability?.GetAbilityTokenDamageList(configuration, DamageRange.Maximum, forceOffhand).Sum(d => d.Sum(t => t.Damage) * d.Multiplier) ?? 0;
		}

		public static double GetAbilityDamageAvg(this SessionAbility ability, Configuration configuration, Session session)
		{
			return GetAbilityDamage(configuration, ability, session.EnergyKineticDamageReduction, session.ElementalInternalDamageReduction, session.DefenseChance);
		}

		private static List<TokenDamage> GetAbilityTokenDamageList(this Ability ability, Configuration configuration, DamageRange range, bool forceOffhand = false)
		{
			lock (Lock)
			{
				if (ability == null)
					return null;

				var damage = new List<TokenDamage>();

				foreach (var token in ability.Tokens.Where(t => t.Type == TokenType.Damage && t.Coefficients.Count > 0))
				{
					// Main hand
					var mainHandActions = token.Coefficients.Where(a => a.IgnoreDualWieldModifier).ToList();
					if (mainHandActions.Count == 0)
						mainHandActions = token.Coefficients;

					var tokenDamage = new TokenDamage {Multiplier = token.Multiplier};
					tokenDamage.AddRange
					(
						mainHandActions.Select
						(
							action => new ActionDamage
							(
								false, GetActionDamage(action, configuration, range),
								action.DamageType == DamageType.Weapon ? configuration.MainHand.DamageType : action.DamageType
							)
						)
					);

					// Offhand (only for weapon damage type)
					if (configuration.DualWield)
					{
						var offHandActions = token.Coefficients.Where(a => !a.IgnoreDualWieldModifier && a.Type == ActionType.WeaponDamage).ToList();

						if (offHandActions.Count > 0)
							tokenDamage.AddRange
							(
								offHandActions.Select
								(
									action => new ActionDamage
									(
										true, GetActionDamage(action, configuration, range, true),
										action.DamageType == DamageType.Weapon ? configuration.OffHand.DamageType : action.DamageType
									)
								)
							);
						else if (forceOffhand)
						{
							tokenDamage.AddRange
							(
								mainHandActions.Where(a => a.Type == ActionType.WeaponDamage).Select
								(
									action =>
										new ActionDamage
										(
											true, GetActionDamage(action, configuration, range, true, true),
											action.DamageType == DamageType.Weapon ? configuration.OffHand.DamageType : action.DamageType
										)
								)
							);
						}
					}

					damage.Add(tokenDamage);
				}

				return damage;
			}
		}

		private static double GetActionDamage(Action action, Configuration configuration, DamageRange range, bool offHand = false, bool forcedOffhand = false)
		{
			var hand = offHand ? configuration.OffHand : configuration.MainHand;
			double damage = 0;

			if (action.Type == ActionType.WeaponDamage)
			{
				damage += (1 + action.AmountModifierPercent)
					* hand.Multiplier
					* (range == DamageRange.Minimum
						? hand.DamageMin
						: (range == DamageRange.Maximum
							? hand.DamageMax
							: hand.DamageAvg));
			}

			if (!forcedOffhand)
			{
				damage += action.Coefficient * (action.Type == ActionType.WeaponDamage ? configuration.BonusDamage : configuration.SpellBonusDamage)
					+ (range == DamageRange.Minimum
						? action.StandardHealthPercentMin
						: (range == DamageRange.Maximum
							? action.StandardHealthPercentMax
							: action.StandardHealthPercentAvg))
					* configuration.StandardDamage;
			}

			return damage;
		}

		public static double GetAbilityDamage(Configuration configuration, SessionAbility ability, double energyKineticReduction, double internalElementalReduction, double defenseChance)
		{
			double damage =
			(
				from token in ability.Ability.GetAbilityTokenDamageList(configuration, DamageRange.Average, ability.ForceOffHand)
				let tokenDamage = token.Sum
				(
					action =>
					{
						var modifier = Math.Min(1 - internalElementalReduction, 1);
						if (action.DamageType == DamageType.Kinetic || action.DamageType == DamageType.Energy)
							modifier = Math.Min(1, action.IsOffHand ? configuration.OffHandAccuracy : configuration.Accuracy)
								* Math.Min(1 - Math.Min(defenseChance, 1 + defenseChance - (action.IsOffHand ? configuration.OffHandAccuracy : configuration.Accuracy)), 1)
								* Math.Min(1 + ability.ArmorReduction - energyKineticReduction, 1);

						return action.Damage * modifier;
					}
				)
				select
					tokenDamage
					* token.Multiplier
			).Sum();

			damage = damage
				+ damage * ability.Autocrit * (configuration.Surge + ability.SurgeBonus) * (1 + configuration.Critical)
				+ damage * (1 - ability.Autocrit) * configuration.Critical * (configuration.Surge + ability.SurgeBonus);

			return damage;
		}
	}
}

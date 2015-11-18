using System.Globalization;
using System.Linq;
using TorPlayground.DamageModel;
using TorPlayground.LogParser.Combat;

namespace TorPlayground.StatOptimizer.Utils
{
	public static class SessionUtils
	{
		public static Session ToSession(this Combat combat, Session defaultValues)
		{
			if (combat == null)
				return null;

			var target = combat.Targets.OrderBy(t => t.DamageDone).Last().Character;
			int additionalTargetNumber = combat.Targets.Count(t => t.Character == null || !t.Character.IsPlayer) - 1;

			Session session = new Session
			{
				Duration = combat.Duration,
				Name = $"{target}{(additionalTargetNumber > 0 ? $" (+{additionalTargetNumber})" : "")}, {combat.Duration.ToString("0.###", CultureInfo.InvariantCulture)}s, {combat.Dps.ToString("0.###", CultureInfo.InvariantCulture)}dps"
			};

			foreach (var ability in combat.Abilities.Where(a => a.Targets.Any(t => t.DamageDone > 0 && t.Character != combat.Player)))
			{
				var sessionAbility = new SessionAbility { Id = ability.Id };

				// not in db
				if (sessionAbility.Ability == null)
					continue;

				var hittingActions = ability.Activations.SelectMany(activation => activation.Actions.Where(a => a.Amount > 0 && !a.IsCritical)).ToList();
				var criticalActions = ability.Activations.SelectMany(activation => activation.Actions.Where(a => a.Amount > 0 && a.IsCritical)).ToList();
				int maximumCritical = criticalActions.Count > 0 ? criticalActions.Max(aa => aa.Amount) : 0;
				int maximum = hittingActions.Count > 0 ? hittingActions.Max(aa => aa.Amount) : 0;
				int minimum = hittingActions.Count > 0 ? hittingActions.Min(aa => aa.Amount) : 0;
				int average = hittingActions.Count > 0 ? (int) hittingActions.Average(aa => aa.Amount) : 0;
				double surge = maximum > 0 && maximumCritical > 0 ? (maximumCritical - maximum) / (double) maximum : 0;
				
				sessionAbility.Activations = ability.Activations.Count;
				sessionAbility.Info =
						$"Activations: {ability.Activations.Count}\n" +
						$"Total hits: {ability.Activations.Sum(a => a.Actions.Count)}\n" +
						$"Hits per activation: {((double) ability.Activations.Sum(a => a.Actions.Count)/ability.Activations.Count)} [{string.Join("/", ability.Activations.Select(a => a.Actions.Count).Distinct().OrderBy(n => n))}]\n" +
						$"Maximum critical hit damage: {maximumCritical}\n" +
						$"Surge: {surge:P}\n" +
						$"Maximum hit damage: {maximum}\n" +
						$"Minimum hit damage: {minimum}\n" +
						$"Average hit damage: {average}\n" +
						$"Total Damage: {ability.Targets.Sum(t => t.DamageDone)}";

				var abilityDefauls = defaultValues?.Abilities.FirstOrDefault(a => a.Id == ability.Id);
				if (abilityDefauls != null)
				{
					sessionAbility.SurgeBonus = abilityDefauls.SurgeBonus;
					sessionAbility.Autocrit = abilityDefauls.Autocrit;
					sessionAbility.DamageMultiplier = abilityDefauls.DamageMultiplier;
					sessionAbility.ArmorReduction = abilityDefauls.ArmorReduction;
					sessionAbility.ForceOffHand = abilityDefauls.ForceOffHand;
				}

				session.Abilities.Add(sessionAbility);
			}
			session.Abilities.Sort((a1, a2) => string.CompareOrdinal(a1.Ability.Name, a2.Ability.Name));
			return session;
		}
	}
}

using System.Linq;
using TorPlayground.Resources;
using TorPlayground.Resources.Abilities;

namespace TorPlayground.DamageModel
{
	public class SessionAbility
	{
		public string Id { get; set; }

		public Ability Ability => _ability ?? (_ability = DataManager.GetAbility(Id));
		private Ability _ability;

		public int Activations { get; set; }
		public double DamageMultiplier { get; set; }
		public double Autocrit { get; set; }
		public double SurgeBonus { get; set; }
		public bool ForceOffHand { get; set; }

		public bool HasOffHandActions => Ability.Tokens.Any(t => t.Type == TokenType.Damage && t.Coefficients.Any(a => a.Type == ActionType.WeaponDamage && !a.IgnoreDualWieldModifier || a.Type == ActionType.SpellDamage));

		/// <summary>
		/// Basic target armor rating multiplier is 1 - 39% = 0.61. 
		/// If ability ignores 20% armor, set it to .2, if ignores completely set it to 1.
		/// </summary>
		public double ArmorReduction { get; set; }

		public SessionAbility()
		{
			DamageMultiplier = 1;
		}
	}
}

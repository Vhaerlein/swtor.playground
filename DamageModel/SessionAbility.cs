using TorPlayground.Resources;
using TorPlayground.Resources.Abilities;

namespace TorPlayground.DamageModel
{
	public class SessionAbility
	{
		public string Id { get; set; }

		public Ability Ability => _ability ?? (_ability = DataManager.GetAbilityByNameId(Id) ?? DataManager.GetAbility(Id) ?? DataManager.GetAbilityBySid(Id));
		private Ability _ability;

		public int Activations { get; set; }
		public double DamageMultiplier { get; set; }
		public double Autocrit { get; set; }
		public double SurgeBonus { get; set; }
		public bool ForceOffhand { get; set; }

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

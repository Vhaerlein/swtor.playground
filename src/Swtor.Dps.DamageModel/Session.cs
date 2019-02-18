using System.Collections.Generic;

namespace Swtor.Dps.DamageModel
{
	public class Session
	{
		public string Name { get; set; }
		public bool Active { get; set; }
		public double Duration { get; set; }
		public double EnergyKineticDamageReduction { get; set; }
		public double ElementalInternalDamageReduction { get; set; }

		public List<SessionAbility> Abilities
		{
			get { return _abilities ?? (_abilities = new List<SessionAbility>()); }
			set { _abilities = value; }
		}
		private List<SessionAbility> _abilities;

		public double DefenseChance { get; set; }

		public Session()
		{
			Duration = -1;
			DefenseChance = 0.1;
			EnergyKineticDamageReduction = 0.39;
			ElementalInternalDamageReduction = 0.0;
		}
	}
}

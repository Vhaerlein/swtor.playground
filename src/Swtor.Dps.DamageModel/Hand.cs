using Newtonsoft.Json;
using Swtor.Dps.Resources.Abilities;

namespace Swtor.Dps.DamageModel
{
	public class Hand
	{
		public int DamageMin { get; set; }
		public int DamageMax { get; set; }

		[JsonIgnore]
		public double DamageAvg => (DamageMin + DamageMax) / 2.0;

		public int Power
		{
			get { return _power; }
			set
			{
				if (_power != value)
				{
					_power = value;
					OnHandPowerUpdated();
				}
			}
		}
		private int _power;

		public double Multiplier { get; set; } = 1;

		public DamageType DamageType
		{
			get { return _damageType; }
			set 
			{
				_damageType = value == DamageType.Weapon ? DamageType.Energy : value;
			}
		}
		private DamageType _damageType = DamageType.Energy;

		private void OnHandPowerUpdated()
		{
			var handler = HandPowerUpdated;
			handler?.Invoke();
		}

		public delegate void HandPowerUpdatedHandler();
		public event HandPowerUpdatedHandler HandPowerUpdated;
	}
}

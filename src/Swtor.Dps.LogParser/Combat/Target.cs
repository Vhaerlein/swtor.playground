using Swtor.Dps.LogParser.Log;

namespace Swtor.Dps.LogParser.Combat
{
	public class Target : DamageHealingGroup
	{
		public Character Character { get; }

		internal Target(Character character)
		{
			Character = character;
		}
	}
}

using TorPlayground.LogParser.Log;

namespace TorPlayground.LogParser.Combat
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

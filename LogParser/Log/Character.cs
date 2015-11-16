using System;

namespace TorPlayground.LogParser.Log
{
	public class Character : IEquatable<Character>
	{
		public string Name { get; }
		public bool IsPlayer { get; }

		internal Character(string name, bool isPlayer)
		{
			Name = name;
			IsPlayer = isPlayer;
		}

		public static bool operator ==(Character character1, Character character2)
		{
			return ReferenceEquals(character1, null) ? ReferenceEquals(character2, null) : character1.Equals(character2);
		}

		public static bool operator !=(Character character1, Character character2)
		{
			return !(character1 == character2);
		}

		public bool Equals(Character other)
		{
			if (other == null)
				return false;

			return other.Name == Name && other.IsPlayer == IsPlayer;
		}

		public override bool Equals(object obj)
		{
			var character = obj as Character;
			return character != null && Equals(character);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ((Name?.GetHashCode() ?? 0) * 397) ^ IsPlayer.GetHashCode();
			}
		}

		public override string ToString()
		{
			return $"{(IsPlayer ? "@" : "")}{Name}";
		}
	}
}

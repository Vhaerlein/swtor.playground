using TorPlayground.LogParser.Combat;

namespace TorPlayground.StatOptimizer.ViewModel
{
	public class CombatViewModel
	{
		public Combat Combat { get; }

		public CombatViewModel(Combat combat)
		{
			Combat = combat;
		}
	}
}

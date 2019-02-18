using Swtor.Dps.LogParser.Combat;

namespace Swtor.Dps.StatOptimizer.ViewModel
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

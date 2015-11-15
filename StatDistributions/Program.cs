using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using TorPlayground.DamageModel;
using TorPlayground.DamageModel.Data;

namespace TorPlayground.StatDistributions
{
	class Program
	{
		static void Main()
		{
			const double budgetMin = 2000.0;
			const double budgetMax = 8000.0;
			const double budgetStep = (budgetMax - budgetMin) / 30.0;

			List<ConfigurationCorrection> budgetCorrections = new List<ConfigurationCorrection>();
			Profile profile = DataManager.Profile;

			for (int i = 0; i <= 30; i ++)
			{
				var budget = budgetMin + budgetStep * i;

				Console.Write("Budget [{0}]:", budget);
				profile.Configuration.BaseMasteryPoints = (int) budget + profile.Configuration.AccuracyPoints;
				var correction = DpsUtils.FindHighestDpsCorrection(profile, (int) budget);
				budgetCorrections.Add(correction);

				Console.Write(" {{a: {0}, c: {1}, m: {2}, p {3}, d: {4}}}, ", correction.AlacrityPoints, correction.CriticalPoints, correction.MasteryPoints, correction.PowerPoints, correction.Dps);
				Console.WriteLine();
			}
			WriteBudget("Optimal distribution:", budgetCorrections);

			List<ConfigurationCorrection> badBudgetCorrections = new List<ConfigurationCorrection>();


			#region All power
			badBudgetCorrections.Clear();
			profile = DataManager.Profile;
			for (int i = 0; i <= 30; i ++)
			{
				var budget = budgetMin + budgetStep*i;

				Console.Write("All power budget [{0}]:", budget);

				profile.Configuration.BaseMasteryPoints = (int) budget + profile.Configuration.AccuracyPoints;
				profile.Configuration.AlacrityPoints = budgetCorrections[i].AlacrityPoints;
				profile.Configuration.CriticalPoints = budgetCorrections[i].CriticalPoints;
				profile.Configuration.PowerPoints = budgetCorrections[i].PowerPoints + budgetCorrections[i].MasteryPoints;
				profile.Configuration.AugmentMasteryPoints = 0;

				var correction = new ConfigurationCorrection
				{
					Dps = DpsUtils.CalculateDps(profile),
					PowerPoints = profile.Configuration.PowerPoints,
					AlacrityPoints = profile.Configuration.AlacrityPoints,
					CriticalPoints = profile.Configuration.CriticalPoints,
					MasteryPoints = profile.Configuration.AugmentMasteryPoints
				};
				badBudgetCorrections.Add(correction);

				Console.Write(" {{a: {0}, c: {1}, m: {2}, p {3}, d: {4}}}, ", correction.AlacrityPoints, correction.CriticalPoints,
					correction.MasteryPoints, correction.PowerPoints, correction.Dps);
				Console.WriteLine();
			}
			WriteBudget("Optimal (all power):", badBudgetCorrections, true);
			#endregion

			#region All mastery
			badBudgetCorrections.Clear();
			profile = DataManager.Profile;
			for (int i = 0; i <= 30; i ++)
			{
				var budget = budgetMin + budgetStep*i;

				Console.Write("All mastery budget [{0}]:", budget);

				profile.Configuration.BaseMasteryPoints = (int) budget + profile.Configuration.AccuracyPoints;
				profile.Configuration.AlacrityPoints = budgetCorrections[i].AlacrityPoints;
				profile.Configuration.CriticalPoints = budgetCorrections[i].CriticalPoints;
				profile.Configuration.AugmentMasteryPoints = budgetCorrections[i].PowerPoints + budgetCorrections[i].MasteryPoints;
				profile.Configuration.PowerPoints = 0;

				var correction = new ConfigurationCorrection
				{
					Dps = DpsUtils.CalculateDps(profile),
					PowerPoints = profile.Configuration.PowerPoints,
					AlacrityPoints = profile.Configuration.AlacrityPoints,
					CriticalPoints = profile.Configuration.CriticalPoints,
					MasteryPoints = profile.Configuration.AugmentMasteryPoints
				};
				badBudgetCorrections.Add(correction);

				Console.Write(" {{a: {0}, c: {1}, m: {2}, p {3}, d: {4}}}, ", correction.AlacrityPoints, correction.CriticalPoints,
					correction.MasteryPoints, correction.PowerPoints, correction.Dps);
				Console.WriteLine();
			}
			WriteBudget("Optimal (all mastery):", badBudgetCorrections, true);
			#endregion

			#region More critical/alacrity
			badBudgetCorrections.Clear();
			profile = DataManager.Profile;
			for (int i = 0; i <= 30; i ++)
			{
				var budget = budgetMin + budgetStep*i;

				Console.Write("More critical/alacrity budget [{0}]:", budget);
				int alacrity = budgetCorrections[i].AlacrityPoints/2;
				int critical = budgetCorrections[i].CriticalPoints/2;

				profile.Configuration.BaseMasteryPoints = (int) budget + profile.Configuration.AccuracyPoints;
				profile.Configuration.AlacrityPoints = alacrity*3;
				profile.Configuration.CriticalPoints = critical*3;
				profile.Configuration.AugmentMasteryPoints = budgetCorrections[i].MasteryPoints - (alacrity + critical) / 2;
				profile.Configuration.PowerPoints = budgetCorrections[i].PowerPoints - (alacrity + critical) / 2;
				if (profile.Configuration.AugmentMasteryPoints < 0)
				{
					profile.Configuration.PowerPoints += profile.Configuration.AugmentMasteryPoints;
					profile.Configuration.AugmentMasteryPoints = 0;
				}
				if (profile.Configuration.PowerPoints < 0)
				{
					profile.Configuration.AugmentMasteryPoints += profile.Configuration.PowerPoints;
					profile.Configuration.PowerPoints = 0;
				}

				var correction = new ConfigurationCorrection
				{
					Dps = DpsUtils.CalculateDps(profile),
					PowerPoints = profile.Configuration.PowerPoints,
					AlacrityPoints = profile.Configuration.AlacrityPoints,
					CriticalPoints = profile.Configuration.CriticalPoints,
					MasteryPoints = profile.Configuration.AugmentMasteryPoints
				};
				badBudgetCorrections.Add(correction);

				Console.Write(" {{a: {0}, c: {1}, m: {2}, p {3}, d: {4}}}, ", correction.AlacrityPoints, correction.CriticalPoints,
					correction.MasteryPoints, correction.PowerPoints, correction.Dps);
				Console.WriteLine();
			}
			WriteBudget("More critical/alacrity:", badBudgetCorrections, true);
			#endregion

			#region More power
			badBudgetCorrections.Clear();
			profile = DataManager.Profile;
			for (int i = 0; i <= 30; i ++)
			{
				var budget = budgetMin + budgetStep*i;

				Console.Write("More power budget [{0}]:", budget);
				int alacrity = budgetCorrections[i].AlacrityPoints/2;
				int critical = budgetCorrections[i].CriticalPoints/2;
				int mastery = budgetCorrections[i].MasteryPoints;

				profile.Configuration.BaseMasteryPoints = (int) budget + profile.Configuration.AccuracyPoints;
				profile.Configuration.AlacrityPoints = alacrity;
				profile.Configuration.CriticalPoints = critical;
				profile.Configuration.PowerPoints = budgetCorrections[i].PowerPoints + alacrity + critical;
				profile.Configuration.AugmentMasteryPoints = mastery;

				var correction = new ConfigurationCorrection
				{
					Dps = DpsUtils.CalculateDps(profile),
					PowerPoints = profile.Configuration.PowerPoints,
					AlacrityPoints = profile.Configuration.AlacrityPoints,
					CriticalPoints = profile.Configuration.CriticalPoints,
					MasteryPoints = profile.Configuration.AugmentMasteryPoints
				};
				badBudgetCorrections.Add(correction);

				Console.Write(" {{a: {0}, c: {1}, m: {2}, p {3}, d: {4}}}, ", correction.AlacrityPoints, correction.CriticalPoints,
					correction.MasteryPoints, correction.PowerPoints, correction.Dps);
				Console.WriteLine();
			}
			WriteBudget("More power:", badBudgetCorrections, true);
			#endregion

			Console.WriteLine("Done.");
			Console.ReadLine();
		}

		private static void WriteBudget(string title, List<ConfigurationCorrection> budgetCorrections, bool append = false)
		{
			const string fileName = @"data/budget.distribution.txt";
			Directory.CreateDirectory("data");
			if (!append)
				File.WriteAllText(fileName, $"{title}\n");
			else
				File.AppendAllText(fileName, $"\n\n{title}\n");

			for (int i = 0; i < budgetCorrections.Count; i++)
			{
				var budget = budgetCorrections[i];
				if (i > 0)
					File.AppendAllText(fileName, "\t");
				File.AppendAllText(fileName, budget.MasteryPoints.ToString());
			}
			File.AppendAllText(fileName, "\n");

			for (int i = 0; i < budgetCorrections.Count; i++)
			{
				var budget = budgetCorrections[i];
				if (i > 0)
					File.AppendAllText(fileName, "\t");
				File.AppendAllText(fileName, budget.AlacrityPoints.ToString());
			}
			File.AppendAllText(fileName, "\n");

			for (int i = 0; i < budgetCorrections.Count; i++)
			{
				var budget = budgetCorrections[i];
				if (i > 0)
					File.AppendAllText(fileName, "\t");
				File.AppendAllText(fileName, budget.CriticalPoints.ToString());
			}
			File.AppendAllText(fileName, "\n");

			for (int i = 0; i < budgetCorrections.Count; i++)
			{
				var budget = budgetCorrections[i];
				if (i > 0)
					File.AppendAllText(fileName, "\t");
				File.AppendAllText(fileName, budget.PowerPoints.ToString());
			}
			File.AppendAllText(fileName, "\n");

			for (int i = 0; i < budgetCorrections.Count; i++)
			{
				var budget = budgetCorrections[i];
				if (i > 0)
					File.AppendAllText(fileName, "\t");
				File.AppendAllText(fileName, budget.Dps.ToString(CultureInfo.InvariantCulture));
			}
		}
	}
}

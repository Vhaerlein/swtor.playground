using System;
using System.Collections.Generic;
using NCalc;
using TorPlayground.DamageModel.Config;

namespace TorPlayground.DamageModel
{
	public static class Formulas
	{
		public static readonly string LevelParameterName = "level";

		public static readonly string AccuracyParameterName = "accuracy";
		public static readonly string AlacrityParameterName = "alacrity";
		public static readonly string CriticalParameterName = "critical";
		public static readonly string MasteryParameterName = "mastery";
		public static readonly string PowerParameterName = "power";
		public static readonly string SpellPowerParameterName = "spellPower";

		private static readonly Expression AccuracyExpression;
		private static readonly Expression AlacrityExpression;
		private static readonly Expression CriticalExpression;
		private static readonly Expression SurgeExpression;
		private static readonly Expression MasteryCriticalExpression;
		private static readonly Expression BonusDamageExpression;
		private static readonly Expression SpellBonusDamageExpression;

		public static bool HasErrors { get; }
		public static List<string> Errors { get; } = new List<string>();

		static Formulas()
		{
			if (DamageModelConfig.Formulas != null)
			{
				if (!string.IsNullOrEmpty(DamageModelConfig.Formulas.Accuracy))
				{
					AccuracyExpression = new Expression(DamageModelConfig.Formulas.Accuracy, EvaluateOptions.IgnoreCase);
					if (AccuracyExpression.HasErrors())
					{
						HasErrors = true;
						Errors.Add($"Accuracy expression error: {AccuracyExpression.Error}.");
						AccuracyExpression = null;
					}
					else
						AccuracyExpression.CutOffUnknownParameters();
				}

				if (!string.IsNullOrEmpty(DamageModelConfig.Formulas.Alacrity))
				{
					AlacrityExpression = new Expression(DamageModelConfig.Formulas.Alacrity, EvaluateOptions.IgnoreCase);
					if (AlacrityExpression.HasErrors())
					{
						HasErrors = true;
						Errors.Add($"Alacrity expression error: {AlacrityExpression.Error}.");
						AlacrityExpression = null;
					}
					else
						AlacrityExpression.CutOffUnknownParameters();
				}

				if (!string.IsNullOrEmpty(DamageModelConfig.Formulas.Critical))
				{
					CriticalExpression = new Expression(DamageModelConfig.Formulas.Critical, EvaluateOptions.IgnoreCase);
					if (CriticalExpression.HasErrors())
					{
						HasErrors = true;
						Errors.Add($"Critical expression error: {CriticalExpression.Error}.");
						CriticalExpression = null;
					}
					else
						CriticalExpression.CutOffUnknownParameters();
				}

				if (!string.IsNullOrEmpty(DamageModelConfig.Formulas.Surge))
				{
					SurgeExpression = new Expression(DamageModelConfig.Formulas.Surge, EvaluateOptions.IgnoreCase);
					if (SurgeExpression.HasErrors())
					{
						HasErrors = true;
						Errors.Add($"Surge expression error: {SurgeExpression.Error}.");
						SurgeExpression = null;
					}
					else
						SurgeExpression.CutOffUnknownParameters();
				}

				if (!string.IsNullOrEmpty(DamageModelConfig.Formulas.MasteryCritical))
				{
					MasteryCriticalExpression = new Expression(DamageModelConfig.Formulas.MasteryCritical, EvaluateOptions.IgnoreCase);
					if (MasteryCriticalExpression.HasErrors())
					{
						HasErrors = true;
						Errors.Add($"Mastery critical expression error: {MasteryCriticalExpression.Error}.");
						MasteryCriticalExpression = null;
					}
					else
						MasteryCriticalExpression.CutOffUnknownParameters();
				}

				if (!string.IsNullOrEmpty(DamageModelConfig.Formulas.BonusDamage))
				{
					BonusDamageExpression = new Expression(DamageModelConfig.Formulas.BonusDamage, EvaluateOptions.IgnoreCase);
					if (BonusDamageExpression.HasErrors())
					{
						HasErrors = true;
						Errors.Add($"Bonus damage expression error: {BonusDamageExpression.Error}.");
						BonusDamageExpression = null;
					}
					else
						BonusDamageExpression.CutOffUnknownParameters();
				}

				if (!string.IsNullOrEmpty(DamageModelConfig.Formulas.SpellBonusDamage))
				{
					SpellBonusDamageExpression = new Expression(DamageModelConfig.Formulas.SpellBonusDamage, EvaluateOptions.IgnoreCase);
					if (SpellBonusDamageExpression.HasErrors())
					{
						HasErrors = true;
						Errors.Add($"Spell bonus damage expression error: {SpellBonusDamageExpression.Error}.");
						SpellBonusDamageExpression = null;
					}
					else
						SpellBonusDamageExpression.CutOffUnknownParameters();
				}
			}
		}

		private static void CutOffUnknownParameters(this Expression expression)
		{
			expression.EvaluateParameter += (name, args) =>
			{
				args.Result = 0;
			};
		}

		public static double? Evaluate(this Expression expression, Configuration configuration)
		{
			if (expression != null)
			{
				expression.FillParameters(configuration);
				var result = expression.Evaluate() as IConvertible;
				return result != null ? Convert.ToDouble(result) : 0;
			}

			return null;
		}

		public static void FillParameters(this Expression expression, Configuration configuration)
		{
			if (expression == null || configuration == null)
				return;

			expression.Parameters[LevelParameterName] = configuration.Level;
			expression.Parameters[AccuracyParameterName] = configuration.AccuracyPoints;
			expression.Parameters[AlacrityParameterName] = configuration.AlacrityPoints;
			expression.Parameters[CriticalParameterName] = configuration.CriticalPoints;
			expression.Parameters[MasteryParameterName] = configuration.BuffedMasteryPoints;
			expression.Parameters[PowerParameterName] = configuration.PowerPoints;
			expression.Parameters[SpellPowerParameterName] = configuration.MainHand.Power + configuration.OffHand.Power;
		}

		public static double GetAccuracy(this Configuration configuration)
		{
			if (configuration == null)
				return 0;

			var result = AccuracyExpression.Evaluate(configuration);
			if (result != null)
				return result.Value;

			return 30 * (1 - Math.Pow(1 - 0.01 / 0.3, configuration.AccuracyPoints / Math.Max(configuration.Level, 20.0) / 1)) / 100;
		}

		public static double GetAlacrity(this Configuration configuration)
		{
			if (configuration == null)
				return 0;

			var result = AlacrityExpression.Evaluate(configuration);
			if (result != null)
				return result.Value;

			return 30 * (1 - Math.Pow(1 - 0.01 / 0.3, configuration.AlacrityPoints / Math.Max(configuration.Level, 20.0) / 1.25)) / 100;
		}

		public static double GetCriticalFromRating(this Configuration configuration)
		{
			if (configuration == null)
				return 0;

			var result = CriticalExpression.Evaluate(configuration);
			if (result != null)
				return result.Value;

			return 30 * (1 - Math.Pow(1 - 0.01 / 0.3, configuration.CriticalPoints / Math.Max(configuration.Level, 20.0) / 0.8)) / 100;
		}

		public static double GetSurge(this Configuration configuration)
		{
			if (configuration == null)
				return 0;

			var result = SurgeExpression.Evaluate(configuration);
			if (result != null)
				return result.Value;

			return 30 * (1 - Math.Pow(1 - 0.01 / 0.3, configuration.CriticalPoints / Math.Max(configuration.Level, 20.0) / 0.8)) / 100;
		}

		public static double GetCriticalFromMastery(this Configuration configuration)
		{
			if (configuration == null)
				return 0;

			var result = MasteryCriticalExpression.Evaluate(configuration);
			if (result != null)
				return result.Value;

			return 20 * (1 - Math.Pow(1 - 0.01 / 0.2, configuration.BuffedMasteryPoints / Math.Max(configuration.Level, 20.0) / 5.5)) / 100;
		}

		public static double GetBonusDamage(this Configuration configuration)
		{
			if (configuration == null)
				return 0;

			var result = BonusDamageExpression.Evaluate(configuration);
			if (result != null)
				return result.Value;

			return configuration.PowerPoints * 0.23 + configuration.BuffedMasteryPoints * 0.2;
		}

		public static double GetSpellBonusDamage(this Configuration configuration)
		{
			if (configuration == null)
				return 0;

			var result = SpellBonusDamageExpression.Evaluate(configuration);
			if (result != null)
				return result.Value;

			return configuration.PowerPoints * 0.23 + (configuration.MainHand.Power + configuration.OffHand.Power) * 0.23 + configuration.BuffedMasteryPoints * 0.2;
		}
	}
}

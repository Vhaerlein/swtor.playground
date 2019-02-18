using System.Configuration;

namespace Swtor.Dps.DamageModel.Config
{
	public static class DamageModelConfig
	{
		public const string SectionName = "formulas";
		public static FormulasConfigurationSection Formulas { get; }

		static DamageModelConfig()
		{
			Formulas = ConfigurationManager.GetSection(SectionName) as FormulasConfigurationSection;
		}
	}
}

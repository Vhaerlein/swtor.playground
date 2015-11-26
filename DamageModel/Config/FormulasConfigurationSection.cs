using System.Configuration;

namespace TorPlayground.DamageModel.Config
{
	public class FormulasConfigurationSection : ConfigurationSection
	{
		public const string AccuracyPropertyName = "accuracy";
		public const string AlacrityPropertyName = "alacrity";
		public const string CriticalPropertyName = "critical";
		public const string SurgePropertyName = "surge";
		public const string MasteryCriticalPropertyName = "masteryCritical";
		public const string BonusDamagePropertyName = "bonusDamage";
		public const string SpellBonusDamagePropertyName = "spellBonusDamage";

		[ConfigurationProperty(AccuracyPropertyName)]
		public string Accuracy {
			get { return (string) this[AccuracyPropertyName]; }
			set { this[AccuracyPropertyName] = value; }
		}

		[ConfigurationProperty(AlacrityPropertyName)]
		public string Alacrity {
			get { return (string) this[AlacrityPropertyName]; }
			set { this[AlacrityPropertyName] = value; }
		}

		[ConfigurationProperty(CriticalPropertyName)]
		public string Critical {
			get { return (string) this[CriticalPropertyName]; }
			set { this[CriticalPropertyName] = value; }
		}

		[ConfigurationProperty(SurgePropertyName)]
		public string Surge {
			get { return (string) this[SurgePropertyName]; }
			set { this[SurgePropertyName] = value; }
		}

		[ConfigurationProperty(MasteryCriticalPropertyName)]
		public string MasteryCritical {
			get { return (string) this[MasteryCriticalPropertyName]; }
			set { this[MasteryCriticalPropertyName] = value; }
		}

		[ConfigurationProperty(BonusDamagePropertyName)]
		public string BonusDamage {
			get { return (string) this[BonusDamagePropertyName]; }
			set { this[BonusDamagePropertyName] = value; }
		}

		[ConfigurationProperty(SpellBonusDamagePropertyName)]
		public string SpellBonusDamage {
			get { return (string) this[SpellBonusDamagePropertyName]; }
			set { this[SpellBonusDamagePropertyName] = value; }
		}
	}
}

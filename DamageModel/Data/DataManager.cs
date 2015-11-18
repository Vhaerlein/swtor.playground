using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace TorPlayground.DamageModel.Data
{
	public static class DataManager
	{
		public static ReadOnlyDictionary<int, int> StandardDamage { get; }
		public static ReadOnlyDictionary<int, int> StandardHealth { get; private set; }

		static DataManager()
		{
			var standardHealth = new Dictionary<int, int>
			{
				{1, 375},
				{2, 440},
				{3, 505},
				{4, 585},
				{5, 675},
				{6, 770},
				{7, 880},
				{8, 1005},
				{9, 1140},
				{10, 1290},
				{11, 1405},
				{12, 1535},
				{13, 1660},
				{14, 1785},
				{15, 1910},
				{16, 2050},
				{17, 2135},
				{18, 2280},
				{19, 2420},
				{20, 2545},
				{21, 2670},
				{22, 2775},
				{23, 2895},
				{24, 3020},
				{25, 3080},
				{26, 3175},
				{27, 3265},
				{28, 3390},
				{29, 3495},
				{30, 3630},
				{31, 3875},
				{32, 4130},
				{33, 4325},
				{34, 4560},
				{35, 4810},
				{36, 5040},
				{37, 5250},
				{38, 5520},
				{39, 5705},
				{40, 5945},
				{41, 6040},
				{42, 6165},
				{43, 6225},
				{44, 6375},
				{45, 6405},
				{46, 6545},
				{47, 6540},
				{48, 6630},
				{49, 6625},
				{50, 6385},
				{51, 7125},
				{52, 8085},
				{53, 8885},
				{54, 9700},
				{55, 10425},
				{56, 11285},
				{57, 12075},
				{58, 12900},
				{59, 13740},
				{60, 14520},
				{65, 27510}
			};

			var standardDamage = new Dictionary<int, int>
			{
				{1, 170},
				{2, 189},
				{3, 226},
				{4, 263},
				{5, 298},
				{6, 333},
				{7, 368},
				{8, 402},
				{9, 435},
				{10, 467},
				{11, 499},
				{12, 531},
				{13, 561},
				{14, 591},
				{15, 621},
				{16, 649},
				{17, 677},
				{18, 705},
				{19, 732},
				{20, 758},
				{21, 783},
				{22, 808},
				{23, 833},
				{24, 856},
				{25, 879},
				{26, 902},
				{27, 923},
				{28, 944},
				{29, 965},
				{30, 985},
				{31, 1004},
				{32, 1022},
				{33, 1040},
				{34, 1057},
				{35, 1074},
				{36, 1090},
				{37, 1105},
				{38, 1120},
				{39, 1134},
				{40, 1148},
				{41, 1160},
				{42, 1173},
				{43, 1184},
				{44, 1195},
				{45, 1205},
				{46, 1215},
				{47, 1224},
				{48, 1232},
				{49, 1233},
				{50, 1235},
				{51, 1430},
				{52, 1657},
				{53, 1816},
				{54, 1933},
				{55, 2035},
				{56, 2147},
				{57, 2296},
				{58, 2507},
				{59, 2806},
				{60, 3185},
				{65, 4465}
			};
			StandardDamage = new ReadOnlyDictionary<int, int>(standardDamage);
			StandardHealth = new ReadOnlyDictionary<int, int>(standardHealth);
		}

		public static Profile DefaultProfile => Profile.Parse(Properties.Resources.Profile);

		public static Profile LoadProfile(string filePath)
		{
			if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
				return null;

			try
			{
				var profile = Profile.Parse(File.ReadAllText(filePath));
				if (!StandardDamage.ContainsKey(profile.Configuration.Level) && profile.Configuration.StandardDamage <= 0
					|| !StandardDamage.ContainsKey(profile.Configuration.Level) && profile.Configuration.StandardDamage <= 0)
					throw new Exception($"App doesn't contain StandardDamage and/or StandardHealth values for level {profile.Configuration.Level}. Try to do calculations for other level or include StandardDamage and/or StandardHealth in profile configuration.");

				return profile;
			}
			catch
			{
				return null;
			}
		}

		public static bool SaveProfile(Profile profile, string filePath)
		{
			if (profile == null)
				return false;

			try
			{
				File.WriteAllText(filePath, profile.ToJson());
			}
			catch (Exception)
			{
				return false;
			}

			return true;
		}
	}
}

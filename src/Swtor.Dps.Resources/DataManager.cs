using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Xml.Linq;
using System.Xml.Serialization;
using Swtor.Dps.Resources.Abilities;

namespace Swtor.Dps.Resources
{
	public static class DataManager
	{
		private static readonly List<string> ResourceKeys;
		private static readonly XDocument Abilities;

		public static bool ExternalXml { get; }

		static DataManager()
		{
			ResourceManager mgr = Properties.Resources.ResourceManager;
			ResourceKeys = (from DictionaryEntry o in mgr.GetResourceSet(CultureInfo.CurrentCulture, true, true) select (string) o.Key).ToList();
			mgr.ReleaseAllResources();

			var abilities = Properties.Resources.Abilities;
			if (File.Exists("xml/abilities.xml"))
			{
				abilities = File.ReadAllText("xml/abilities.xml");
				ExternalXml = true;
			}

			Abilities = XDocument.Parse(abilities);
		}

		public static Bitmap GetIcon(string iconName)
		{
			if (string.IsNullOrEmpty(iconName))
				return Properties.Resources._default;

			string resourceKey = ResourceKeys.FirstOrDefault(k => string.Equals(k, iconName, StringComparison.InvariantCultureIgnoreCase));
			Bitmap bitmap = null;

			if (resourceKey != null)
				bitmap = Properties.Resources.ResourceManager.GetObject(resourceKey) as Bitmap;

			return bitmap ?? Properties.Resources._default;
		}

		private static readonly Dictionary<string, XElement> Cache = new Dictionary<string, XElement>();

		public static Ability GetAbility(string id)
		{
			if (!Cache.ContainsKey(id))
				Cache[id] = Abilities.Descendants("Ability").FirstOrDefault(a => a.Attribute("Id").Value == id || a.Attribute("NameId").Value == id || a.Attribute("Sid").Value == id);
			if (Cache[id] != null)
				return (Ability) new XmlSerializer(typeof (Ability)).Deserialize(Cache[id].CreateReader());

			return null;
		}

		public static Ability GetAbilityById(string id)
		{
			var node = Abilities.Descendants("Ability").FirstOrDefault(a => a.Attribute("Id").Value == id);
			if (node != null)
				return (Ability) new XmlSerializer(typeof (Ability)).Deserialize(node.CreateReader());

			return null;
		}

		public static Ability GetAbilityByNameId(string id)
		{
			var node = Abilities.Descendants("Ability").FirstOrDefault(a => a.Attribute("NameId").Value == id);
			if (node != null)
				return (Ability) new XmlSerializer(typeof (Ability)).Deserialize(node.CreateReader());

			return null;
		}

		public static Ability GetAbilityBySid(string sid)
		{
			var node = Abilities.Descendants("Ability").FirstOrDefault(a => a.Attribute("Sid").Value == sid);

			if (node != null)
				return (Ability) new XmlSerializer(typeof (Ability)).Deserialize(node.CreateReader());

			return null;
		}
	}
}

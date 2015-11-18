using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Xml.Linq;
using System.Xml.Serialization;
using TorPlayground.Resources.Abilities;

namespace TorPlayground.Resources
{
	public static class DataManager
	{
		private static readonly List<string> ResourceKeys;
		private static readonly XDocument Abilities;

		static DataManager()
		{
			ResourceManager mgr = Properties.Resources.ResourceManager;
			ResourceKeys = (from DictionaryEntry o in mgr.GetResourceSet(CultureInfo.CurrentCulture, true, true) select (string) o.Key).ToList();
			mgr.ReleaseAllResources();

			Abilities = XDocument.Parse(Properties.Resources.Abilities);
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

		public static Ability GetAbility(string id)
		{
			var node = Abilities.Descendants("Ability").FirstOrDefault(a => a.Attribute("Id").Value == id || a.Attribute("NameId").Value == id || a.Attribute("Sid").Value == id);
			if (node != null)
				return (Ability) new XmlSerializer(typeof (Ability)).Deserialize(node.CreateReader());

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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable PossibleNullReferenceException

namespace TorPlayground.ClassAbilityXmlMerger
{
	class Program
	{
		private static bool _debugMode;

		static void Main(string[] args)
		{
			if (args.Length > 0 && args[0] == "/debug")
				_debugMode = true;

			File.Delete(@"Xml\FullAbilities.latest.xml");
			File.Delete(@"Xml\abilities.xml");
			var files = Directory.GetFiles(@"Xml", "*.xml", SearchOption.TopDirectoryOnly);

			var xml = GetClassAbilitiesXmlDocument(files[0]);

			Console.WriteLine("Start merging...");

			for (int i = 1; i < files.Length; i ++)
			{
				var updateXml = GetClassAbilitiesXmlDocument(files[i]);

				foreach (XmlElement node in updateXml.DocumentElement.ChildNodes)
				{
					var fqn = node.SelectSingleNode("Fqn");
					var id = fqn.Attributes["NodeId"].Value;
					var nodeToUpdate = xml.DocumentElement.SelectSingleNode(string.Format("/Abilities/Ability[Fqn[@NodeId = '{0}']]", id));

					try
					{
						UpdateNode(xml, xml.DocumentElement, nodeToUpdate, node);
					}
					catch (Exception ex)
					{
						Console.WriteLine("{0} {1} {2}", ex.Message, id, files[i]);
//						throw;
					}
				}
			}

			xml.Save(@"Xml\FullAbilities.latest.xml");

			Console.WriteLine("Done.");

			foreach (var r in xml.DocumentElement.SelectNodes("//*[@Status]").Cast<XmlElement>().ToList())
				r.Attributes.Remove(r.Attributes["Status"]);

			Console.WriteLine("Removed statuses...");

			foreach (var r in xml.DocumentElement.SelectNodes("//effParam_LevelCap|//DBURL|//Base62Id").Cast<XmlElement>().ToList())
				r.ParentNode.RemoveChild(r);

			Console.WriteLine("Removed junk nodes...");

			foreach (XmlElement node in xml.DocumentElement.ChildNodes)
			{
				var fqn = node.SelectSingleNode("Fqn");
				node.Attributes["Id"].Value = fqn.Attributes["NodeId"].Value;
				node.Attributes.Append(xml.CreateAttribute("Sid"));
				node.Attributes["Sid"].Value = fqn.InnerText;
				node.RemoveChild(fqn);

				var name = node.SelectSingleNode("Name");
				node.Attributes.Append(xml.CreateAttribute("NameId"));
				node.Attributes["NameId"].Value = name.Attributes["Id"].Value;
				name.Attributes.Remove(name.Attributes["Id"]);

				var desc = node.SelectSingleNode("Description");
				desc.Attributes.Remove(desc.Attributes["Id"]);
			}

			Console.WriteLine("Updated id, nameid and sid...");

			using (var stringWriter = new StringWriter())
			using (var xmlTextWriter = XmlWriter.Create(stringWriter))
			{
				xml.WriteTo(xmlTextWriter);
				xmlTextWriter.Flush();
				var str = stringWriter.GetStringBuilder().ToString()
					.Replace(">ablDescriptionTokenType", ">")
					.Replace("effAction_", "").Replace("ablParsedDescriptionToken", "DescriptionToken")
					.Replace("ablDescriptionToken", "").Replace("effParam_", "").Replace("CoEfficient", "Coefficient").Replace("ablCoefficients", "Coefficients")
					.Replace(">false<", ">0<").Replace(">true<", ">1<").Replace(">False<", ">0<").Replace(">True<", ">1<");
				xml = new XmlDocument();
				xml.LoadXml(str);
			}

			xml.Save(@"Xml\abilities.xml");

			Console.WriteLine("Done.");
			Console.ReadLine();
		}

		private static void UpdateNode(XmlDocument xml, XmlNode parent, XmlNode nodeToUpdate, XmlElement nodeWithChanges)
		{
			if (nodeWithChanges.Name == "DBURL")
				return;

			var status = nodeWithChanges.Attributes["Status"] != null ? nodeWithChanges.Attributes["Status"].Value : null;

			if (status == "Removed")
			{
				if (nodeToUpdate == null)
					throw new Exception("Can't find node to remove.");
				parent.RemoveChild(nodeToUpdate);
			}
			else if (status == "New")
			{
				parent.AppendChild(xml.ImportNode(nodeWithChanges, true));
			}
			else
			{
				if (nodeToUpdate == null)
					throw new Exception("No node to update.");

				if (nodeWithChanges.ChildNodes.Count == 0 || nodeWithChanges.ChildNodes.Count == 1 && nodeWithChanges.ChildNodes[0].NodeType == XmlNodeType.Text)
				{
					if (nodeWithChanges.Name != "Description" && nodeWithChanges.Attributes["OldValue"] != null)
					{
						var oldValue = nodeWithChanges.Attributes["OldValue"].Value.Replace("\r\n", "\n").Trim();
						var actualValue = nodeToUpdate.InnerText.Replace("\r\n", "\n").Trim();
						if (oldValue != actualValue && _debugMode)
						{
							var attrName = nodeToUpdate.InnerXml == nodeWithChanges.InnerXml ? "update-semi-fuckup" : "update-fuckup";

							var attr = nodeToUpdate.Attributes[attrName];
							if (attr == null)
							{
								attr = xml.CreateAttribute(attrName);
								nodeToUpdate.Attributes.Append(attr);
							}
							attr.Value = string.Format("{3}{4}Old value expected: {0}, actual value was: {1}, new value: {2}", oldValue, actualValue, nodeWithChanges.InnerXml, attr.Value, string.IsNullOrEmpty(attr.Value) ? "" : "; ");
						}
					}
					nodeToUpdate.InnerXml = nodeWithChanges.InnerXml;
				}
				else
				{
					var nodes = nodeWithChanges.ChildNodes.Cast<XmlElement>().Where(n => n.Name != "DBURL").ToList();
					var nodesToUpdate = nodeToUpdate.ChildNodes.Cast<XmlElement>().ToList();

					foreach (var newNode in nodes.Where(n => n.Attributes["Status"] != null && n.Attributes["Status"].Value == "New").ToList())
					{
						UpdateNode(xml, nodeToUpdate, null, newNode);
						nodes.Remove(newNode);
					}

					while (nodes.Count > 0)
					{
						var singleChangeNode = nodes.First();
						var childNodesToUpdate = nodesToUpdate.Where(n => n.Name == singleChangeNode.Name).ToList();
						if (childNodesToUpdate.Count == 0)
						{
							throw new Exception(string.Format("Can't find single matching node to update. (status: {0}, node: {1})", singleChangeNode.Attributes["Status"] != null ? singleChangeNode.Attributes["Status"].Value : "none", singleChangeNode.Name));
						}

						var matchedChilds = GetMatchedChilds(childNodesToUpdate, singleChangeNode);
						
						if (matchedChilds.Count != 1)
						{
							var changedNodes = GetMatchedChilds(nodes, singleChangeNode);
							if (changedNodes.Count != matchedChilds.Count)
							{
								throw new Exception(string.Format("Update nodes count doesn't match. ({0})", singleChangeNode.Name));
							}

							for (int i = 0; i < changedNodes.Count; i ++)
							{
								UpdateNode(xml, nodeToUpdate, matchedChilds[i], changedNodes[i]);
								nodes.Remove(changedNodes[i]);
								nodesToUpdate.Remove(matchedChilds[i]);
							}
						}
						else
						{
							UpdateNode(xml, nodeToUpdate, matchedChilds[0], singleChangeNode);
							nodes.Remove(singleChangeNode);
							nodesToUpdate.Remove(matchedChilds[0]);
						}
					}
				}
			}
		}

		private static List<XmlElement> GetMatchedChilds(List<XmlElement> childNodes, XmlElement child)
		{
			var matchedChilds = new List<XmlElement>();
			foreach (XmlElement childToUpdate in childNodes)
			{
				if (child.Attributes.Cast<XmlAttribute>().Count(a => a.Name != "OldValue" && a.Name != "Status") > 0)
				{
					if (childToUpdate.Attributes.Count > 0)
					{
						if (child.Attributes.Cast<XmlAttribute>().All(a => a.Name == "OldValue" || a.Name == "Status"
							|| childToUpdate.HasAttribute(a.Name) && childToUpdate.Attributes[a.Name].Value == a.Value))
							matchedChilds.Add(childToUpdate);
					}
				}
				else
					matchedChilds.Add(childToUpdate);
			}
			return matchedChilds;
		}

		private static XmlDocument GetClassAbilitiesXmlDocument(string filePath)
		{
			var xml = new XmlDocument();
			xml.Load(filePath);

			// removing second description
			foreach (XmlElement ability in xml.DocumentElement.ChildNodes)
			{
				var dsc = ability.SelectNodes("Description");
				if (dsc.Count > 1)
				{
					for (int i = 1; i < dsc.Count; i ++)
						ability.RemoveChild(dsc[i]);
				}
			}

			return xml;
		}
	}
}

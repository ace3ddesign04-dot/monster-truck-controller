using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Gaia
{
	public class GaiaExtensionManager
	{
		private Dictionary<string, GaiaCompatiblePublisher> m_extensions = new Dictionary<string, GaiaCompatiblePublisher>();

		public void ScanForExtensions()
		{
			m_extensions.Clear();
			string empty = string.Empty;
			string empty2 = string.Empty;
			string packageImageName = string.Empty;
			string packageDescription = string.Empty;
			string packageURL = string.Empty;
			List<Type> typesInNamespace = GetTypesInNamespace("Gaia.GX.");
			for (int i = 0; i < typesInNamespace.Count; i++)
			{
				string[] array = typesInNamespace[i].FullName.Split('.');
				empty = Regex.Replace(array[2], "(\\B[A-Z])", " $1");
				empty2 = Regex.Replace(array[3], "(\\B[A-Z])", " $1");
				MethodInfo[] methods = typesInNamespace[i].GetMethods(BindingFlags.Static | BindingFlags.Public);
				List<MethodInfo> list = new List<MethodInfo>();
				foreach (MethodInfo methodInfo in methods)
				{
					if (methodInfo.Name.StartsWith("GX_"))
					{
						list.Add(methodInfo);
					}
					else if (methodInfo.Name == "GetPublisherName")
					{
						empty = (string)methodInfo.Invoke(null, null);
					}
					else if (methodInfo.Name == "GetPackageName")
					{
						empty2 = (string)methodInfo.Invoke(null, null);
					}
				}
				GaiaCompatiblePublisher value = null;
				if (!m_extensions.TryGetValue(empty, out value))
				{
					value = new GaiaCompatiblePublisher();
					value.m_publisherName = empty;
					value.m_compatibleFoldedOut = false;
					value.m_installedFoldedOut = false;
					m_extensions.Add(empty, value);
				}
				GaiaCompatiblePackage gaiaCompatiblePackage = value.GetPackage(empty2);
				if (gaiaCompatiblePackage == null)
				{
					gaiaCompatiblePackage = new GaiaCompatiblePackage();
					gaiaCompatiblePackage.m_compatibleFoldedOut = false;
					gaiaCompatiblePackage.m_installedFoldedOut = false;
					gaiaCompatiblePackage.m_packageName = empty2;
					value.AddPackage(gaiaCompatiblePackage);
				}
				if (list.Count > 0)
				{
					gaiaCompatiblePackage.m_isInstalled = true;
				}
				else
				{
					gaiaCompatiblePackage.m_isInstalled = false;
				}
				gaiaCompatiblePackage.m_methods = new List<MethodInfo>(list);
			}
			typesInNamespace = GetTypesInNamespace("Gaia.GXC.");
			for (int k = 0; k < typesInNamespace.Count; k++)
			{
				string[] array = typesInNamespace[k].FullName.Split('.');
				empty = Regex.Replace(array[2], "(\\B[A-Z])", " $1");
				empty2 = Regex.Replace(array[3], "(\\B[A-Z])", " $1");
				MethodInfo[] methods = typesInNamespace[k].GetMethods(BindingFlags.Static | BindingFlags.Public);
				foreach (MethodInfo methodInfo in methods)
				{
					if (methodInfo.Name == "GetPublisherName")
					{
						empty = (string)methodInfo.Invoke(null, null);
					}
					else if (methodInfo.Name == "GetPackageName")
					{
						empty2 = (string)methodInfo.Invoke(null, null);
					}
					else if (methodInfo.Name == "GetPackageImage")
					{
						packageImageName = (string)methodInfo.Invoke(null, null);
					}
					else if (methodInfo.Name == "GetPackageDescription")
					{
						packageDescription = (string)methodInfo.Invoke(null, null);
					}
					else if (methodInfo.Name == "GetPackageURL")
					{
						packageURL = (string)methodInfo.Invoke(null, null);
					}
				}
				GaiaCompatiblePublisher value2 = null;
				if (!m_extensions.TryGetValue(empty, out value2))
				{
					value2 = new GaiaCompatiblePublisher();
					value2.m_publisherName = empty;
					value2.m_compatibleFoldedOut = false;
					value2.m_installedFoldedOut = false;
					m_extensions.Add(empty, value2);
				}
				GaiaCompatiblePackage gaiaCompatiblePackage2 = value2.GetPackage(empty2);
				if (gaiaCompatiblePackage2 == null)
				{
					gaiaCompatiblePackage2 = new GaiaCompatiblePackage();
					gaiaCompatiblePackage2.m_compatibleFoldedOut = false;
					gaiaCompatiblePackage2.m_installedFoldedOut = false;
					gaiaCompatiblePackage2.m_packageName = empty2;
					value2.AddPackage(gaiaCompatiblePackage2);
				}
				gaiaCompatiblePackage2.m_isCompatible = true;
				gaiaCompatiblePackage2.m_packageDescription = packageDescription;
				gaiaCompatiblePackage2.m_packageImageName = packageImageName;
				gaiaCompatiblePackage2.m_packageURL = packageURL;
			}
		}

		public int GetInstalledExtensionCount()
		{
			int num = 0;
			foreach (GaiaCompatiblePublisher value in m_extensions.Values)
			{
				num += value.InstalledPackages();
			}
			return num;
		}

		public List<GaiaCompatiblePublisher> GetPublishers()
		{
			List<GaiaCompatiblePublisher> list = new List<GaiaCompatiblePublisher>(m_extensions.Values);
			list.Sort((GaiaCompatiblePublisher a, GaiaCompatiblePublisher b) => a.m_publisherName.CompareTo(b.m_publisherName));
			return list;
		}

		public List<Type> GetTypesInNamespace(string nameSpace)
		{
			List<Type> list = new List<Type>();
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			for (int i = 0; i < assemblies.Length; i++)
			{
				if (!assemblies[i].FullName.StartsWith("Assembly"))
				{
					continue;
				}
				Type[] types = assemblies[i].GetTypes();
				for (int j = 0; j < types.Length; j++)
				{
					if (!string.IsNullOrEmpty(types[j].Namespace) && types[j].Namespace.StartsWith(nameSpace))
					{
						list.Add(types[j]);
					}
				}
			}
			return list;
		}
	}
}

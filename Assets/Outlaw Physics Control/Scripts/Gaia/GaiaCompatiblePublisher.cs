using System.Collections.Generic;

namespace Gaia
{
	public class GaiaCompatiblePublisher
	{
		public string m_publisherName;

		public bool m_installedFoldedOut;

		public bool m_compatibleFoldedOut;

		private Dictionary<string, GaiaCompatiblePackage> m_packages = new Dictionary<string, GaiaCompatiblePackage>();

		public GaiaCompatiblePackage GetPackage(string packageName)
		{
			if (m_packages.TryGetValue(packageName, out GaiaCompatiblePackage value))
			{
				return value;
			}
			return null;
		}

		public List<GaiaCompatiblePackage> GetPackages()
		{
			List<GaiaCompatiblePackage> list = new List<GaiaCompatiblePackage>(m_packages.Values);
			list.Sort((GaiaCompatiblePackage a, GaiaCompatiblePackage b) => a.m_packageName.CompareTo(b.m_packageName));
			return list;
		}

		public int InstalledPackages()
		{
			int num = 0;
			foreach (KeyValuePair<string, GaiaCompatiblePackage> package in m_packages)
			{
				if (package.Value.m_isInstalled)
				{
					num++;
				}
			}
			return num;
		}

		public int CompatiblePackages()
		{
			int num = 0;
			foreach (KeyValuePair<string, GaiaCompatiblePackage> package in m_packages)
			{
				if (package.Value.m_isCompatible)
				{
					num++;
				}
			}
			return num;
		}

		public void AddPackage(GaiaCompatiblePackage package)
		{
			m_packages.Add(package.m_packageName, package);
		}
	}
}

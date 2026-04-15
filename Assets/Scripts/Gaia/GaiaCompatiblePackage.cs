using System.Collections.Generic;
using System.Reflection;

namespace Gaia
{
	public class GaiaCompatiblePackage
	{
		public string m_packageName;

		public string m_packageDescription;

		public string m_packageImageName;

		public string m_packageURL;

		public bool m_isCompatible;

		public bool m_isInstalled;

		public bool m_installedFoldedOut;

		public bool m_compatibleFoldedOut;

		public List<MethodInfo> m_methods = new List<MethodInfo>();

		public Dictionary<string, bool> m_methodGroupFoldouts = new Dictionary<string, bool>();
	}
}

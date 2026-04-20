using UnityEngine;

namespace Gaia
{
	public class GaiaSettings : ScriptableObject
	{
		[Tooltip("Current defaults object.")]
		public GaiaDefaults m_currentDefaults;

		[Tooltip("Current resources object.")]
		public GaiaResource m_currentResources;

		[Tooltip("Publisher name for exported extensions.")]
		public string m_publisherName = string.Empty;

		[Tooltip("Default prefab name for the player object.")]
		public string m_playerPrefabName = "FPSController";

		[Tooltip("Default prefab name for the water object.")]
		public string m_waterPrefabName = "Water4Advanced";

		[Tooltip("Show or hide tooltips in all custom editors.")]
		public bool m_showTooltips = true;
	}
}

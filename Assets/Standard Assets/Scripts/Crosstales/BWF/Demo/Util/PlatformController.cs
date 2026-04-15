using Crosstales.BWF.Util;
using System.Collections.Generic;
using UnityEngine;

namespace Crosstales.BWF.Demo.Util
{
	[HelpURL("https://www.crosstales.com/media/data/assets/badwordfilter/api/class_crosstales_1_1_b_w_f_1_1_demo_1_1_util_1_1_platform_controller.html")]
	public class PlatformController : MonoBehaviour
	{
		[Header("Configuration")]
		[Tooltip("Selected platforms for the controller.")]
		public List<Platform> Platforms;

		[Tooltip("Enable or disable the 'Objects' for the selected 'Platforms' (default: true).")]
		public bool Active = true;

		[Header("Objects")]
		[Tooltip("Selected objects for the controller.")]
		public GameObject[] Objects;

		private Platform currentPlatform;

		public void Start()
		{
			if (Helper.isWindowsPlatform)
			{
				currentPlatform = Platform.Windows;
			}
			else if (Helper.isMacOSPlatform)
			{
				currentPlatform = Platform.OSX;
			}
			else if (Helper.isAndroidPlatform)
			{
				currentPlatform = Platform.Android;
			}
			else if (Helper.isIOSPlatform)
			{
				currentPlatform = Platform.IOS;
			}
			else if (Helper.isWSAPlatform)
			{
				currentPlatform = Platform.WSA;
			}
			else if (Helper.isWebPlatform)
			{
				currentPlatform = Platform.Web;
			}
			else
			{
				currentPlatform = Platform.Unsupported;
			}
			bool active = (!Platforms.Contains(currentPlatform)) ? (!Active) : Active;
			GameObject[] objects = Objects;
			foreach (GameObject gameObject in objects)
			{
				if (gameObject != null)
				{
					gameObject.SetActive(active);
				}
			}
		}
	}
}

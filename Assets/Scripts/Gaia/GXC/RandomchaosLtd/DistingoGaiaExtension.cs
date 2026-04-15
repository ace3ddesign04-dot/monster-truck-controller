using UnityEngine;

namespace Gaia.GXC.RandomchaosLtd
{
	public class DistingoGaiaExtension : MonoBehaviour
	{
		public static string GetPublisherName()
		{
			return "Randomchaos Ltd";
		}

		public static string GetPackageName()
		{
			return "Distingo - Terrain in Detail";
		}

		public static string GetPackageImage()
		{
			return "DistingoGaiaImage";
		}

		public static string GetPackageDescription()
		{
			return "Distingo – Bringing ever increasing detail to your teerrain.\r\n\r\nAlter terrain splatting distance.\r\nRegulate texture tialing based on distance from the camera.\r\nAlter individual textures:-\r\n    Near and Far UV Multipliers\r\n    Normal map power\r\n    Smoothness\r\n    Metallic\r\n";
		}

		public static string GetPackageURL()
		{
			return "https://www.assetstore.unity3d.com/#!/content/54737";
		}
	}
}

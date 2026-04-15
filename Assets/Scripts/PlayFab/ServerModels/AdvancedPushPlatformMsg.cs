using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class AdvancedPushPlatformMsg
	{
		public string Json;

		public PushNotificationPlatform Platform;
	}
}

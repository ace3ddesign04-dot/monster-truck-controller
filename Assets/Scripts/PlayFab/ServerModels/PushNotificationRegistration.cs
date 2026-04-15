using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class PushNotificationRegistration
	{
		public string NotificationEndpointARN;

		public PushNotificationPlatform? Platform;
	}
}

using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class PushNotificationRegistration
	{
		public string NotificationEndpointARN;

		public PushNotificationPlatform? Platform;
	}
}

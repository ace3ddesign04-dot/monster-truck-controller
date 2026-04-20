using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class PushNotificationRegistrationModel
	{
		public string NotificationEndpointARN;

		public PushNotificationPlatform? Platform;
	}
}

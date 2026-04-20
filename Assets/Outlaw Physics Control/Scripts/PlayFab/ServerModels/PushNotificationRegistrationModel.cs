using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class PushNotificationRegistrationModel
	{
		public string NotificationEndpointARN;

		public PushNotificationPlatform? Platform;
	}
}

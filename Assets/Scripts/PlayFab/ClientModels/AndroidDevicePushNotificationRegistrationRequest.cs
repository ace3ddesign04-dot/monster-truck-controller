using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class AndroidDevicePushNotificationRegistrationRequest : PlayFabRequestCommon
	{
		public string ConfirmationMessage;

		public string DeviceToken;

		public bool? SendPushNotificationConfirmation;
	}
}

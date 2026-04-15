using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class RegisterForIOSPushNotificationRequest : PlayFabRequestCommon
	{
		public string ConfirmationMessage;

		public string DeviceToken;

		public bool? SendPushNotificationConfirmation;
	}
}

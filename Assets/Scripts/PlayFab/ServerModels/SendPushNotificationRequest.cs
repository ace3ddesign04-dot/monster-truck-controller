using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class SendPushNotificationRequest : PlayFabRequestCommon
	{
		public List<AdvancedPushPlatformMsg> AdvancedPlatformDelivery;

		public string Message;

		public PushNotificationPackage Package;

		public string Recipient;

		public string Subject;

		public List<PushNotificationPlatform> TargetPlatforms;
	}
}

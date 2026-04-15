using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class SetupPushNotificationRequest : PlayFabRequestCommon
	{
		public string Credential;

		public string Key;

		public string Name;

		public bool OverwriteOldARN;

		public PushSetupPlatform Platform;
	}
}

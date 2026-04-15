using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class PushNotificationPackage
	{
		public int Badge;

		public string CustomData;

		public string Icon;

		public string Message;

		public string Sound;

		public string Title;
	}
}

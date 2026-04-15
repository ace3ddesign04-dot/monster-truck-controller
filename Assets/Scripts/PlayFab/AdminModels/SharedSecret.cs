using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class SharedSecret
	{
		public bool Disabled;

		public string FriendlyName;

		public string SecretKey;
	}
}

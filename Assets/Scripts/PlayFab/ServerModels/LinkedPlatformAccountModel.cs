using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class LinkedPlatformAccountModel
	{
		public string Email;

		public LoginIdentityProvider? Platform;

		public string PlatformUserId;

		public string Username;
	}
}

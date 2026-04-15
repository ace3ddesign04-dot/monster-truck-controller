using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class AddUsernamePasswordRequest : PlayFabRequestCommon
	{
		public string Email;

		public string Password;

		public string Username;
	}
}

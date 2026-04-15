using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class LookupUserAccountInfoRequest : PlayFabRequestCommon
	{
		public string Email;

		public string PlayFabId;

		public string TitleDisplayName;

		public string Username;
	}
}

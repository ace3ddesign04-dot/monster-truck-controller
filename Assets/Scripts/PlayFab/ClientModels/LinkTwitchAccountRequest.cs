using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class LinkTwitchAccountRequest : PlayFabRequestCommon
	{
		public string AccessToken;

		public bool? ForceLink;
	}
}

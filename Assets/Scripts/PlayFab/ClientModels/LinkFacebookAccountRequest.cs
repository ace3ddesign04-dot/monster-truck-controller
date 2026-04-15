using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class LinkFacebookAccountRequest : PlayFabRequestCommon
	{
		public string AccessToken;

		public bool? ForceLink;
	}
}

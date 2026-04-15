using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class LinkGoogleAccountRequest : PlayFabRequestCommon
	{
		public bool? ForceLink;

		public string ServerAuthCode;
	}
}

using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class LinkSteamAccountRequest : PlayFabRequestCommon
	{
		public bool? ForceLink;

		public string SteamTicket;
	}
}

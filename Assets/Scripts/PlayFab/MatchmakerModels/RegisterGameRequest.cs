using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.MatchmakerModels
{
	[Serializable]
	public class RegisterGameRequest : PlayFabRequestCommon
	{
		public string Build;

		public string GameMode;

		public string LobbyId;

		public Region Region;

		public string ServerHost;

		public string ServerIPV6Address;

		public string ServerPort;

		public Dictionary<string, string> Tags;
	}
}

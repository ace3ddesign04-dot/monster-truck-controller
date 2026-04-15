using PlayFab.SharedModels;
using System;

namespace PlayFab.MatchmakerModels
{
	[Serializable]
	public class PlayerJoinedRequest : PlayFabRequestCommon
	{
		public string LobbyId;

		public string PlayFabId;
	}
}

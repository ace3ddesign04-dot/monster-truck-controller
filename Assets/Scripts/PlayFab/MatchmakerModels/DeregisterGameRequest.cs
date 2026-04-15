using PlayFab.SharedModels;
using System;

namespace PlayFab.MatchmakerModels
{
	[Serializable]
	public class DeregisterGameRequest : PlayFabRequestCommon
	{
		public string LobbyId;
	}
}

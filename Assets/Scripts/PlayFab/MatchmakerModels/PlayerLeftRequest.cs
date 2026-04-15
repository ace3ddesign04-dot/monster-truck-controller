using PlayFab.SharedModels;
using System;

namespace PlayFab.MatchmakerModels
{
	[Serializable]
	public class PlayerLeftRequest : PlayFabRequestCommon
	{
		public string LobbyId;

		public string PlayFabId;
	}
}

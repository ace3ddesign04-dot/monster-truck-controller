using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class GetMatchmakerGameInfoRequest : PlayFabRequestCommon
	{
		public string LobbyId;
	}
}

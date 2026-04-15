using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class RefreshGameServerInstanceHeartbeatRequest : PlayFabRequestCommon
	{
		public string LobbyId;
	}
}

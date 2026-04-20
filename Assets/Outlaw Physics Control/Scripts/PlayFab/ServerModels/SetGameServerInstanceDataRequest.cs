using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class SetGameServerInstanceDataRequest : PlayFabRequestCommon
	{
		public string GameServerData;

		public string LobbyId;
	}
}

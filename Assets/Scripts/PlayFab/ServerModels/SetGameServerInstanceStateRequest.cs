using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class SetGameServerInstanceStateRequest : PlayFabRequestCommon
	{
		public string LobbyId;

		public GameInstanceState State;
	}
}

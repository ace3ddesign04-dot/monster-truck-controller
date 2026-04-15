using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class DeregisterGameRequest : PlayFabRequestCommon
	{
		public string LobbyId;
	}
}

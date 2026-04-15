using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class NotifyMatchmakerPlayerLeftRequest : PlayFabRequestCommon
	{
		public string LobbyId;

		public string PlayFabId;
	}
}

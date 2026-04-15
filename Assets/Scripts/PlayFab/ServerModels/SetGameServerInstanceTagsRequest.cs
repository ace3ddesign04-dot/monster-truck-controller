using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class SetGameServerInstanceTagsRequest : PlayFabRequestCommon
	{
		public string LobbyId;

		public Dictionary<string, string> Tags;
	}
}

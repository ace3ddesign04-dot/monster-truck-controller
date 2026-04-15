using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GameServerRegionsRequest : PlayFabRequestCommon
	{
		public string BuildVersion;

		public string TitleId;
	}
}

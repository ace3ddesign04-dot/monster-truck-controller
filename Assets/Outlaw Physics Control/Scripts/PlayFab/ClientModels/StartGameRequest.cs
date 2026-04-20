using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class StartGameRequest : PlayFabRequestCommon
	{
		public string BuildVersion;

		public string CharacterId;

		public string CustomCommandLineData;

		public string GameMode;

		public Region Region;

		public string StatisticName;
	}
}

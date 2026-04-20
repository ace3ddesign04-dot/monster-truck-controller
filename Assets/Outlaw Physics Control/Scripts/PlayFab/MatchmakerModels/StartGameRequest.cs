using PlayFab.SharedModels;
using System;

namespace PlayFab.MatchmakerModels
{
	[Serializable]
	public class StartGameRequest : PlayFabRequestCommon
	{
		public string Build;

		public string CustomCommandLineData;

		public string ExternalMatchmakerEventEndpoint;

		public string GameMode;

		public Region Region;
	}
}

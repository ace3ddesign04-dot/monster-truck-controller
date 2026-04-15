using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class ModifyMatchmakerGameModesRequest : PlayFabRequestCommon
	{
		public string BuildVersion;

		public List<GameModeInfo> GameModes;
	}
}

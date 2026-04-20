using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class CurrentGamesResult : PlayFabResultCommon
	{
		public int GameCount;

		public List<GameInfo> Games;

		public int PlayerCount;
	}
}

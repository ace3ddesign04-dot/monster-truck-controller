using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetPlayFabIDsFromGameCenterIDsResult : PlayFabResultCommon
	{
		public List<GameCenterPlayFabIdPair> Data;
	}
}

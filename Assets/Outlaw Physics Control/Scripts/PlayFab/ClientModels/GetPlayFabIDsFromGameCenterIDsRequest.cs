using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetPlayFabIDsFromGameCenterIDsRequest : PlayFabRequestCommon
	{
		public List<string> GameCenterIDs;
	}
}

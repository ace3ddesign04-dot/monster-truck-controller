using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GetPlayFabIDsFromSteamIDsRequest : PlayFabRequestCommon
	{
		public List<string> SteamStringIDs;
	}
}

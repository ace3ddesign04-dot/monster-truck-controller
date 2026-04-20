using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetPlayFabIDsFromSteamIDsRequest : PlayFabRequestCommon
	{
		public List<string> SteamStringIDs;
	}
}

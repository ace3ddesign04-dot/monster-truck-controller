using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetPlayFabIDsFromSteamIDsResult : PlayFabResultCommon
	{
		public List<SteamPlayFabIdPair> Data;
	}
}

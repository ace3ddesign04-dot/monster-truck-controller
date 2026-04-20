using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetPlayFabIDsFromTwitchIDsResult : PlayFabResultCommon
	{
		public List<TwitchPlayFabIdPair> Data;
	}
}

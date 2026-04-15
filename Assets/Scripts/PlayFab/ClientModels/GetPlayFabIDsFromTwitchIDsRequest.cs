using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetPlayFabIDsFromTwitchIDsRequest : PlayFabRequestCommon
	{
		public List<string> TwitchIds;
	}
}

using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetPlayFabIDsFromFacebookIDsRequest : PlayFabRequestCommon
	{
		public List<string> FacebookIDs;
	}
}

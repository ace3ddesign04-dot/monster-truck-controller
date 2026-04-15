using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GetPlayFabIDsFromFacebookIDsRequest : PlayFabRequestCommon
	{
		public List<string> FacebookIDs;
	}
}

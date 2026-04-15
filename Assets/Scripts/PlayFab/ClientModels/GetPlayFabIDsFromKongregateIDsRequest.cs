using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetPlayFabIDsFromKongregateIDsRequest : PlayFabRequestCommon
	{
		public List<string> KongregateIDs;
	}
}

using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetPlayFabIDsFromGoogleIDsRequest : PlayFabRequestCommon
	{
		public List<string> GoogleIDs;
	}
}

using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetPlayFabIDsFromGoogleIDsResult : PlayFabResultCommon
	{
		public List<GooglePlayFabIdPair> Data;
	}
}

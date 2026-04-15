using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetPlayFabIDsFromFacebookIDsResult : PlayFabResultCommon
	{
		public List<FacebookPlayFabIdPair> Data;
	}
}

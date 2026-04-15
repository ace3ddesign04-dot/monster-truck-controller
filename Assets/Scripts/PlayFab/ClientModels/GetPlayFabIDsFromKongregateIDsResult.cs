using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetPlayFabIDsFromKongregateIDsResult : PlayFabResultCommon
	{
		public List<KongregatePlayFabIdPair> Data;
	}
}

using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetPlayFabIDsFromGenericIDsResult : PlayFabResultCommon
	{
		public List<GenericPlayFabIdPair> Data;
	}
}

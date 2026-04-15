using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetPlayFabIDsFromGenericIDsRequest : PlayFabRequestCommon
	{
		public List<GenericServiceId> GenericIDs;
	}
}

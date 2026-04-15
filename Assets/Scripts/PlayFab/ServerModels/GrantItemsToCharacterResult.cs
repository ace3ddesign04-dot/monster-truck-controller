using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GrantItemsToCharacterResult : PlayFabResultCommon
	{
		public List<GrantedItemInstance> ItemGrantResults;
	}
}

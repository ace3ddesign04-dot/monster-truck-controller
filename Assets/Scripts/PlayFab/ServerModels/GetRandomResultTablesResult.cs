using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GetRandomResultTablesResult : PlayFabResultCommon
	{
		public Dictionary<string, RandomResultTableListing> Tables;
	}
}

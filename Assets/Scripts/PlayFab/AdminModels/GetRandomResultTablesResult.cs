using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class GetRandomResultTablesResult : PlayFabResultCommon
	{
		public Dictionary<string, RandomResultTableListing> Tables;
	}
}

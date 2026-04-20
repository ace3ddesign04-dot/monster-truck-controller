using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetPlayerTagsResult : PlayFabResultCommon
	{
		public string PlayFabId;

		public List<string> Tags;
	}
}

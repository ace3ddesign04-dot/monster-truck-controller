using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class DeleteUsersRequest : PlayFabRequestCommon
	{
		public List<string> PlayFabIds;

		public string TitleId;
	}
}

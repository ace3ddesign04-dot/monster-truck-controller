using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class DeleteUsersRequest : PlayFabRequestCommon
	{
		public List<string> PlayFabIds;

		public string TitleId;
	}
}

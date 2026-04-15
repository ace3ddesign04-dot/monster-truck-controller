using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class BanUsersRequest : PlayFabRequestCommon
	{
		public List<BanRequest> Bans;
	}
}

using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class BanUsersRequest : PlayFabRequestCommon
	{
		public List<BanRequest> Bans;
	}
}

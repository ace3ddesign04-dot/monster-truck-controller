using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class BanUsersResult : PlayFabResultCommon
	{
		public List<BanInfo> BanData;
	}
}

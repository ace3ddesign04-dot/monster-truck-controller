using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class BanUsersResult : PlayFabResultCommon
	{
		public List<BanInfo> BanData;
	}
}

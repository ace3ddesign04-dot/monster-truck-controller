using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GetUserBansResult : PlayFabResultCommon
	{
		public List<BanInfo> BanData;
	}
}

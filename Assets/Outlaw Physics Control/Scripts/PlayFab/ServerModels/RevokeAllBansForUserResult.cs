using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class RevokeAllBansForUserResult : PlayFabResultCommon
	{
		public List<BanInfo> BanData;
	}
}

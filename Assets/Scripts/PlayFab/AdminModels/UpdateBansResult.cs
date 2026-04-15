using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class UpdateBansResult : PlayFabResultCommon
	{
		public List<BanInfo> BanData;
	}
}

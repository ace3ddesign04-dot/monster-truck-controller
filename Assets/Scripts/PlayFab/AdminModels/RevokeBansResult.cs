using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class RevokeBansResult : PlayFabResultCommon
	{
		public List<BanInfo> BanData;
	}
}

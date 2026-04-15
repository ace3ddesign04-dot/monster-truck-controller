using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class UpdateBansRequest : PlayFabRequestCommon
	{
		public List<UpdateBanRequest> Bans;
	}
}

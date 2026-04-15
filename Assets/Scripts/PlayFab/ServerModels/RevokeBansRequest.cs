using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class RevokeBansRequest : PlayFabRequestCommon
	{
		public List<string> BanIds;
	}
}

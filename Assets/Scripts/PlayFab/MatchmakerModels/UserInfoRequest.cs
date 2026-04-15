using PlayFab.SharedModels;
using System;

namespace PlayFab.MatchmakerModels
{
	[Serializable]
	public class UserInfoRequest : PlayFabRequestCommon
	{
		public int MinCatalogVersion;

		public string PlayFabId;
	}
}

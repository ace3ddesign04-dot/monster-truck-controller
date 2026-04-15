using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GetPlayerProfileRequest : PlayFabRequestCommon
	{
		public string PlayFabId;

		public PlayerProfileViewConstraints ProfileConstraints;
	}
}

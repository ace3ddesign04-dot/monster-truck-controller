using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetPlayerProfileRequest : PlayFabRequestCommon
	{
		public string PlayFabId;

		public PlayerProfileViewConstraints ProfileConstraints;
	}
}

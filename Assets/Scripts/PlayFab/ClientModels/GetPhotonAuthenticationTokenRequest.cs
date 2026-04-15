using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetPhotonAuthenticationTokenRequest : PlayFabRequestCommon
	{
		public string PhotonApplicationId;
	}
}

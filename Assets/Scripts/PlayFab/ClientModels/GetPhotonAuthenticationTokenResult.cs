using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetPhotonAuthenticationTokenResult : PlayFabResultCommon
	{
		public string PhotonCustomAuthenticationToken;
	}
}

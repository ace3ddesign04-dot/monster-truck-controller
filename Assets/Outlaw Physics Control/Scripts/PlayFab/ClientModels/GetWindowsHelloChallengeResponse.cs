using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetWindowsHelloChallengeResponse : PlayFabResultCommon
	{
		public string Challenge;
	}
}

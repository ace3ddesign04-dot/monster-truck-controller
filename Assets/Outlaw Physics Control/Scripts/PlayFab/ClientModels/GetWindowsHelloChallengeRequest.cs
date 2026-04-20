using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetWindowsHelloChallengeRequest : PlayFabRequestCommon
	{
		public string PublicKeyHint;

		public string TitleId;
	}
}

using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class GetMatchmakerGameModesRequest : PlayFabRequestCommon
	{
		public string BuildVersion;
	}
}

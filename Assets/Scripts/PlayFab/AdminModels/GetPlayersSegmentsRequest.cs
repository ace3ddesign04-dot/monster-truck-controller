using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class GetPlayersSegmentsRequest : PlayFabRequestCommon
	{
		public string PlayFabId;
	}
}

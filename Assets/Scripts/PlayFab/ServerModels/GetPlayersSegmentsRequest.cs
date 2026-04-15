using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GetPlayersSegmentsRequest : PlayFabRequestCommon
	{
		public string PlayFabId;
	}
}

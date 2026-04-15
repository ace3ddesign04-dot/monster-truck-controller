using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GetCharacterStatisticsRequest : PlayFabRequestCommon
	{
		public string CharacterId;

		public string PlayFabId;
	}
}

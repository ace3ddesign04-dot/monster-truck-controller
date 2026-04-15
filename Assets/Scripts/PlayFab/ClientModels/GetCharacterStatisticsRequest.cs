using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetCharacterStatisticsRequest : PlayFabRequestCommon
	{
		public string CharacterId;
	}
}

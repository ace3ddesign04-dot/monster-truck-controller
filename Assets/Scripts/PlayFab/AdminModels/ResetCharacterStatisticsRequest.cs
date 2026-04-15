using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class ResetCharacterStatisticsRequest : PlayFabRequestCommon
	{
		public string CharacterId;

		public string PlayFabId;
	}
}

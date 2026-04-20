using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class ResetUserStatisticsRequest : PlayFabRequestCommon
	{
		public string PlayFabId;
	}
}

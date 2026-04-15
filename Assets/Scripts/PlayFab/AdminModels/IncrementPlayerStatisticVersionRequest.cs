using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class IncrementPlayerStatisticVersionRequest : PlayFabRequestCommon
	{
		public string StatisticName;
	}
}

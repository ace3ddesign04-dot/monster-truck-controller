using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class ConsumeItemResult : PlayFabResultCommon
	{
		public string ItemInstanceId;

		public int RemainingUses;
	}
}

using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class ConsumeItemResult : PlayFabResultCommon
	{
		public string ItemInstanceId;

		public int RemainingUses;
	}
}

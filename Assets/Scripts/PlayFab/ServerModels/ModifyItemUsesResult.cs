using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class ModifyItemUsesResult : PlayFabResultCommon
	{
		public string ItemInstanceId;

		public int RemainingUses;
	}
}

using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class ModifyItemUsesRequest : PlayFabRequestCommon
	{
		public string ItemInstanceId;

		public string PlayFabId;

		public int UsesToAdd;
	}
}

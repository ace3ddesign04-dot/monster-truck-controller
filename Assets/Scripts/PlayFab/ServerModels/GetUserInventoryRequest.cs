using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GetUserInventoryRequest : PlayFabRequestCommon
	{
		public string PlayFabId;
	}
}

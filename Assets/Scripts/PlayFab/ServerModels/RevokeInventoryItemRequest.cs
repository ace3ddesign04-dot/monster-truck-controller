using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class RevokeInventoryItemRequest : PlayFabRequestCommon
	{
		public string CharacterId;

		public string ItemInstanceId;

		public string PlayFabId;
	}
}

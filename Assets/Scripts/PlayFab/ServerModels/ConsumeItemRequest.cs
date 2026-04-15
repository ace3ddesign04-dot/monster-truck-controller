using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class ConsumeItemRequest : PlayFabRequestCommon
	{
		public string CharacterId;

		public int ConsumeCount;

		public string ItemInstanceId;

		public string PlayFabId;
	}
}

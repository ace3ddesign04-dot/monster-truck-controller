using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class ConsumeItemRequest : PlayFabRequestCommon
	{
		public string CharacterId;

		public int ConsumeCount;

		public string ItemInstanceId;
	}
}

using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class GetUserInventoryRequest : PlayFabRequestCommon
	{
		public string PlayFabId;
	}
}

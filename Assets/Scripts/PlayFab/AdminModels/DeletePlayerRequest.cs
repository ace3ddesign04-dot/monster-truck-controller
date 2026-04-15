using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class DeletePlayerRequest : PlayFabRequestCommon
	{
		public string PlayFabId;
	}
}

using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class DeleteCharacterFromUserRequest : PlayFabRequestCommon
	{
		public string CharacterId;

		public string PlayFabId;

		public bool SaveCharacterInventory;
	}
}

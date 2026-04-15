using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class MoveItemToCharacterFromCharacterRequest : PlayFabRequestCommon
	{
		public string GivingCharacterId;

		public string ItemInstanceId;

		public string PlayFabId;

		public string ReceivingCharacterId;
	}
}

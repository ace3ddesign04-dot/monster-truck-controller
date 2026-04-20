using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GetCharacterInventoryRequest : PlayFabRequestCommon
	{
		public string CatalogVersion;

		public string CharacterId;

		public string PlayFabId;
	}
}

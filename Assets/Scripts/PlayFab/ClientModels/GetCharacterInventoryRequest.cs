using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetCharacterInventoryRequest : PlayFabRequestCommon
	{
		public string CatalogVersion;

		public string CharacterId;
	}
}

using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GrantCharacterToUserRequest : PlayFabRequestCommon
	{
		public string CatalogVersion;

		public string CharacterName;

		public string ItemId;
	}
}

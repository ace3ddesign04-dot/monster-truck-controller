using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GrantCharacterToUserRequest : PlayFabRequestCommon
	{
		public string CharacterName;

		public string CharacterType;

		public string PlayFabId;
	}
}

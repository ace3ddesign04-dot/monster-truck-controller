using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GrantCharacterToUserResult : PlayFabResultCommon
	{
		public string CharacterId;

		public string CharacterType;

		public bool Result;
	}
}

using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class CharacterResult : PlayFabResultCommon
	{
		public string CharacterId;

		public string CharacterName;

		public string CharacterType;
	}
}

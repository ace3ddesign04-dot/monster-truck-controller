using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class CharacterResult : PlayFabResultCommon
	{
		public string CharacterId;

		public string CharacterName;

		public string CharacterType;
	}
}

using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class CharacterLeaderboardEntry
	{
		public string CharacterId;

		public string CharacterName;

		public string CharacterType;

		public string DisplayName;

		public string PlayFabId;

		public int Position;

		public int StatValue;
	}
}

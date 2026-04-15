using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GrantCharacterToUserResult : PlayFabResultCommon
	{
		public string CharacterId;
	}
}

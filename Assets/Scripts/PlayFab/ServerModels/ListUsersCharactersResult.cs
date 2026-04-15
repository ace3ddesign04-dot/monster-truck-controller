using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class ListUsersCharactersResult : PlayFabResultCommon
	{
		public List<CharacterResult> Characters;
	}
}

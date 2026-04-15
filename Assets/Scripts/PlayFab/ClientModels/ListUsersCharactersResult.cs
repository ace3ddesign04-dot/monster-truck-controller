using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class ListUsersCharactersResult : PlayFabResultCommon
	{
		public List<CharacterResult> Characters;
	}
}

using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class ListUsersCharactersRequest : PlayFabRequestCommon
	{
		public string PlayFabId;
	}
}

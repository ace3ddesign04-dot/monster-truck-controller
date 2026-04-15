using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class ListUsersCharactersRequest : PlayFabRequestCommon
	{
		public string PlayFabId;
	}
}

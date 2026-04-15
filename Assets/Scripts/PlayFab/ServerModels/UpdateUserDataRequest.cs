using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class UpdateUserDataRequest : PlayFabRequestCommon
	{
		public Dictionary<string, string> Data;

		public List<string> KeysToRemove;

		public UserDataPermission? Permission;

		public string PlayFabId;
	}
}

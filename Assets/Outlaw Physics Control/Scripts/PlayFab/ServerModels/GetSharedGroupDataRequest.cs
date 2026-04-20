using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GetSharedGroupDataRequest : PlayFabRequestCommon
	{
		public bool? GetMembers;

		public List<string> Keys;

		public string SharedGroupId;
	}
}

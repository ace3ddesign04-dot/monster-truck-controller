using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GetUserDataRequest : PlayFabRequestCommon
	{
		public uint? IfChangedFromDataVersion;

		public List<string> Keys;

		public string PlayFabId;
	}
}

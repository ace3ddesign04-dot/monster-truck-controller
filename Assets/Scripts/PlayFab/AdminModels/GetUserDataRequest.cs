using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class GetUserDataRequest : PlayFabRequestCommon
	{
		public uint? IfChangedFromDataVersion;

		public List<string> Keys;

		public string PlayFabId;
	}
}

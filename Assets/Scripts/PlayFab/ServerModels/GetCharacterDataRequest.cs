using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GetCharacterDataRequest : PlayFabRequestCommon
	{
		public string CharacterId;

		public uint? IfChangedFromDataVersion;

		public List<string> Keys;

		public string PlayFabId;
	}
}

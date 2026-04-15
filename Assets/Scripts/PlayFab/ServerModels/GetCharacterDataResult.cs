using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GetCharacterDataResult : PlayFabResultCommon
	{
		public string CharacterId;

		public Dictionary<string, UserDataRecord> Data;

		public uint DataVersion;

		public string PlayFabId;
	}
}

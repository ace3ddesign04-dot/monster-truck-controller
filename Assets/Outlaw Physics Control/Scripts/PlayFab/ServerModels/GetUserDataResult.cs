using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GetUserDataResult : PlayFabResultCommon
	{
		public Dictionary<string, UserDataRecord> Data;

		public uint DataVersion;

		public string PlayFabId;
	}
}

using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class GetUserDataResult : PlayFabResultCommon
	{
		public Dictionary<string, UserDataRecord> Data;

		public uint DataVersion;

		public string PlayFabId;
	}
}

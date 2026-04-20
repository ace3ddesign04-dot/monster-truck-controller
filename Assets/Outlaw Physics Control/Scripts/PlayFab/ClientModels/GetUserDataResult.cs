using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetUserDataResult : PlayFabResultCommon
	{
		public Dictionary<string, UserDataRecord> Data;

		public uint DataVersion;
	}
}

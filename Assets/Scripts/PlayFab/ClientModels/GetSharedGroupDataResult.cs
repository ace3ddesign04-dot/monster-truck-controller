using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetSharedGroupDataResult : PlayFabResultCommon
	{
		public Dictionary<string, SharedGroupDataRecord> Data;

		public List<string> Members;
	}
}

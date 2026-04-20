using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetTitleDataResult : PlayFabResultCommon
	{
		public Dictionary<string, string> Data;
	}
}

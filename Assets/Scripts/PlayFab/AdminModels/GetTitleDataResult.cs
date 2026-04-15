using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class GetTitleDataResult : PlayFabResultCommon
	{
		public Dictionary<string, string> Data;
	}
}

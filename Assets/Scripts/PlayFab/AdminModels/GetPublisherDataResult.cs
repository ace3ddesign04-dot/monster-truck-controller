using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class GetPublisherDataResult : PlayFabResultCommon
	{
		public Dictionary<string, string> Data;
	}
}

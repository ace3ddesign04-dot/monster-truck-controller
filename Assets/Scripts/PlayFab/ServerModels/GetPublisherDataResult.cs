using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GetPublisherDataResult : PlayFabResultCommon
	{
		public Dictionary<string, string> Data;
	}
}

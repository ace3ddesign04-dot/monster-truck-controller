using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GetTitleDataRequest : PlayFabRequestCommon
	{
		public List<string> Keys;
	}
}

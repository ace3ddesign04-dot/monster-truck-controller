using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GetPublisherDataRequest : PlayFabRequestCommon
	{
		public List<string> Keys;
	}
}

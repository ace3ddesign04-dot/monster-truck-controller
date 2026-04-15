using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetPublisherDataRequest : PlayFabRequestCommon
	{
		public List<string> Keys;
	}
}

using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class GetTitleDataRequest : PlayFabRequestCommon
	{
		public List<string> Keys;
	}
}

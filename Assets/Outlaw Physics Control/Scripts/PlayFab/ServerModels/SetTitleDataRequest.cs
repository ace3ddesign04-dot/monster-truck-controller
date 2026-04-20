using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class SetTitleDataRequest : PlayFabRequestCommon
	{
		public string Key;

		public string Value;
	}
}

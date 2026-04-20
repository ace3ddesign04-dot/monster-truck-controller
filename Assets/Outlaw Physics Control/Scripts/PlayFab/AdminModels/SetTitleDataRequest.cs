using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class SetTitleDataRequest : PlayFabRequestCommon
	{
		public string Key;

		public string Value;
	}
}

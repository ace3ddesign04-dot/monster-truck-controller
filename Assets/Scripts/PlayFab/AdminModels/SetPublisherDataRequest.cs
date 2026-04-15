using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class SetPublisherDataRequest : PlayFabRequestCommon
	{
		public string Key;

		public string Value;
	}
}

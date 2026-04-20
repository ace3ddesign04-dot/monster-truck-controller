using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class SetPublisherDataRequest : PlayFabRequestCommon
	{
		public string Key;

		public string Value;
	}
}

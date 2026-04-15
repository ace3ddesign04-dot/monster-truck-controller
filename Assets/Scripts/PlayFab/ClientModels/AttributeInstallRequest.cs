using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class AttributeInstallRequest : PlayFabRequestCommon
	{
		public string Adid;

		public string Idfa;
	}
}

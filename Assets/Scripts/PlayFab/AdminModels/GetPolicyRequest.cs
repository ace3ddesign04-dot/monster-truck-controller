using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class GetPolicyRequest : PlayFabRequestCommon
	{
		public string PolicyName;
	}
}

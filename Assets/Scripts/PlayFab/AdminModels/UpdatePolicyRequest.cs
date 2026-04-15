using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class UpdatePolicyRequest : PlayFabRequestCommon
	{
		public bool OverwritePolicy;

		public string PolicyName;

		public List<PermissionStatement> Statements;
	}
}

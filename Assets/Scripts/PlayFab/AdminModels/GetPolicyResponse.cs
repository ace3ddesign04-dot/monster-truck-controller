using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class GetPolicyResponse : PlayFabResultCommon
	{
		public string PolicyName;

		public List<PermissionStatement> Statements;
	}
}

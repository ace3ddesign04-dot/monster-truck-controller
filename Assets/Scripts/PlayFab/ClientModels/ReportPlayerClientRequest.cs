using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class ReportPlayerClientRequest : PlayFabRequestCommon
	{
		public string Comment;

		public string ReporteeId;
	}
}

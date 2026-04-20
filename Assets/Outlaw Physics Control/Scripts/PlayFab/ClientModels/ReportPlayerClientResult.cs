using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class ReportPlayerClientResult : PlayFabResultCommon
	{
		public int SubmissionsRemaining;
	}
}

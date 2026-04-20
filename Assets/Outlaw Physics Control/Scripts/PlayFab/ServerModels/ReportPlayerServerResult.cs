using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class ReportPlayerServerResult : PlayFabResultCommon
	{
		public int SubmissionsRemaining;
	}
}

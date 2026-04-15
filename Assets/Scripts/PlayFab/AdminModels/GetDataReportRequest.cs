using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class GetDataReportRequest : PlayFabRequestCommon
	{
		public int Day;

		public int Month;

		public string ReportName;

		public int Year;
	}
}

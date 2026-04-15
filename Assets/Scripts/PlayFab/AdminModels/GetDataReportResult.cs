using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class GetDataReportResult : PlayFabResultCommon
	{
		public string DownloadUrl;
	}
}

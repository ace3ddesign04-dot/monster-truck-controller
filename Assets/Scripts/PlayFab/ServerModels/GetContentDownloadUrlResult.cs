using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GetContentDownloadUrlResult : PlayFabResultCommon
	{
		public string URL;
	}
}

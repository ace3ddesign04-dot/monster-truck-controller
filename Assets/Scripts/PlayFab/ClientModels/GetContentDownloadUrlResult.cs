using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetContentDownloadUrlResult : PlayFabResultCommon
	{
		public string URL;
	}
}

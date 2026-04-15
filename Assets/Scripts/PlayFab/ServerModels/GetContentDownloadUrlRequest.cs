using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class GetContentDownloadUrlRequest : PlayFabRequestCommon
	{
		public string HttpMethod;

		public string Key;

		public bool? ThruCDN;
	}
}

using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GetContentDownloadUrlRequest : PlayFabRequestCommon
	{
		public string HttpMethod;

		public string Key;

		public bool? ThruCDN;
	}
}

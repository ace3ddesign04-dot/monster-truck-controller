using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class GetContentUploadUrlRequest : PlayFabRequestCommon
	{
		public string ContentType;

		public string Key;
	}
}

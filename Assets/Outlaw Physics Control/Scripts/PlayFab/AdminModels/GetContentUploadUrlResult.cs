using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class GetContentUploadUrlResult : PlayFabResultCommon
	{
		public string URL;
	}
}

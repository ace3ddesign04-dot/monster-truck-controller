using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class GetServerBuildUploadURLRequest : PlayFabRequestCommon
	{
		public string BuildId;
	}
}

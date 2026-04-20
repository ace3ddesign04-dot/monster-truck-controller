using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class RemoveServerBuildRequest : PlayFabRequestCommon
	{
		public string BuildId;
	}
}

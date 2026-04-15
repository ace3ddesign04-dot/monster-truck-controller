using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class GetCloudScriptRevisionRequest : PlayFabRequestCommon
	{
		public int? Revision;

		public int? Version;
	}
}

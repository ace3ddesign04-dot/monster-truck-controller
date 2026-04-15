using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class GetCloudScriptVersionsResult : PlayFabResultCommon
	{
		public List<CloudScriptVersionStatus> Versions;
	}
}

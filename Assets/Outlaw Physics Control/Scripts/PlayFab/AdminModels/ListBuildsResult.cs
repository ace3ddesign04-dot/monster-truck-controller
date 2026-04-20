using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class ListBuildsResult : PlayFabResultCommon
	{
		public List<GetServerBuildInfoResult> Builds;
	}
}

using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class GameServerRegionsResult : PlayFabResultCommon
	{
		public List<RegionInfo> Regions;
	}
}

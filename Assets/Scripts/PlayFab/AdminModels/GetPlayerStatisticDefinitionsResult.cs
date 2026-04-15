using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class GetPlayerStatisticDefinitionsResult : PlayFabResultCommon
	{
		public List<PlayerStatisticDefinition> Statistics;
	}
}

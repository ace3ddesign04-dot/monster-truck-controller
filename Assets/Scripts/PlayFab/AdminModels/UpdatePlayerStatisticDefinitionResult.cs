using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class UpdatePlayerStatisticDefinitionResult : PlayFabResultCommon
	{
		public PlayerStatisticDefinition Statistic;
	}
}

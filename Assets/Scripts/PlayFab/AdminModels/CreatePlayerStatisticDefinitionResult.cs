using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class CreatePlayerStatisticDefinitionResult : PlayFabResultCommon
	{
		public PlayerStatisticDefinition Statistic;
	}
}

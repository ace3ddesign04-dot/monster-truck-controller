using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class CheckLimitedEditionItemAvailabilityResult : PlayFabResultCommon
	{
		public int Amount;
	}
}

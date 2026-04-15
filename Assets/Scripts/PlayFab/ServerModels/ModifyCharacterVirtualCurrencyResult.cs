using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class ModifyCharacterVirtualCurrencyResult : PlayFabResultCommon
	{
		public int Balance;

		public string VirtualCurrency;
	}
}

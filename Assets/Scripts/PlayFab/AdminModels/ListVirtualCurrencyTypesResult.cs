using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class ListVirtualCurrencyTypesResult : PlayFabResultCommon
	{
		public List<VirtualCurrencyData> VirtualCurrencies;
	}
}

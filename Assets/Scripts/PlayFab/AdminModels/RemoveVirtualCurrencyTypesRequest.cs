using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class RemoveVirtualCurrencyTypesRequest : PlayFabRequestCommon
	{
		public List<VirtualCurrencyData> VirtualCurrencies;
	}
}

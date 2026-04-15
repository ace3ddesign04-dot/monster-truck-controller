using PlayFab.SharedModels;
using System;
using System.Collections.Generic;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class AddVirtualCurrencyTypesRequest : PlayFabRequestCommon
	{
		public List<VirtualCurrencyData> VirtualCurrencies;
	}
}

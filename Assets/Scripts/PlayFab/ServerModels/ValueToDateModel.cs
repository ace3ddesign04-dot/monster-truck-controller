using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class ValueToDateModel
	{
		public string Currency;

		public uint TotalValue;

		public string TotalValueAsDecimal;
	}
}

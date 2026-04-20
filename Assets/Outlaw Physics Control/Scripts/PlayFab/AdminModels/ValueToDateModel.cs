using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class ValueToDateModel
	{
		public string Currency;

		public uint TotalValue;

		public string TotalValueAsDecimal;
	}
}

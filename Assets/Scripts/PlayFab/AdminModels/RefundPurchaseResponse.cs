using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class RefundPurchaseResponse : PlayFabResultCommon
	{
		public string PurchaseStatus;
	}
}

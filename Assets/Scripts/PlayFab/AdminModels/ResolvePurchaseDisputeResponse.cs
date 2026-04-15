using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class ResolvePurchaseDisputeResponse : PlayFabResultCommon
	{
		public string PurchaseStatus;
	}
}

using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class SubscriptionModel
	{
		public DateTime Expiration;

		public DateTime InitialSubscriptionTime;

		public bool IsActive;

		public SubscriptionProviderStatus? Status;

		public string SubscriptionId;

		public string SubscriptionItemId;

		public string SubscriptionProvider;
	}
}

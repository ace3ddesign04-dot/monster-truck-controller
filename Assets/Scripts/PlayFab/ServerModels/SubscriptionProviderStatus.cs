namespace PlayFab.ServerModels
{
	public enum SubscriptionProviderStatus
	{
		NoError,
		Cancelled,
		UnknownError,
		BillingError,
		ProductUnavailable,
		CustomerDidNotAcceptPriceChange,
		FreeTrial,
		PaymentPending
	}
}

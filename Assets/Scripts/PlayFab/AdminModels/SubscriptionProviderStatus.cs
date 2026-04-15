namespace PlayFab.AdminModels
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

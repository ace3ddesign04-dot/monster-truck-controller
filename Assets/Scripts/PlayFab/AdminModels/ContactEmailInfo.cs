using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class ContactEmailInfo
	{
		public string EmailAddress;

		public string Name;

		public EmailVerificationStatus? VerificationStatus;
	}
}

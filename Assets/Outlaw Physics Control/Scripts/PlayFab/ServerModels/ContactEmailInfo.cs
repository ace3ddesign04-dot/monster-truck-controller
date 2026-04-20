using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class ContactEmailInfo
	{
		public string EmailAddress;

		public string Name;

		public EmailVerificationStatus? VerificationStatus;
	}
}

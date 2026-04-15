using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class ContactEmailInfoModel
	{
		public string EmailAddress;

		public string Name;

		public EmailVerificationStatus? VerificationStatus;
	}
}

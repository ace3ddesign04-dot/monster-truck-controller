using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class ContactEmailInfoModel
	{
		public string EmailAddress;

		public string Name;

		public EmailVerificationStatus? VerificationStatus;
	}
}

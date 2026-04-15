using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class ApiCondition
	{
		public Conditionals? HasSignatureOrEncryption;
	}
}

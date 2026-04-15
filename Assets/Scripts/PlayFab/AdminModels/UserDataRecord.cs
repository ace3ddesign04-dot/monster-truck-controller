using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class UserDataRecord
	{
		public DateTime LastUpdated;

		public UserDataPermission? Permission;

		public string Value;
	}
}

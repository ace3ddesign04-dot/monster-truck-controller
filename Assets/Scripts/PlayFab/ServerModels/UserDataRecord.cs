using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class UserDataRecord
	{
		public DateTime LastUpdated;

		public UserDataPermission? Permission;

		public string Value;
	}
}

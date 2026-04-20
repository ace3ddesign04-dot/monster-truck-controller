using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class UpdateUserDataResult : PlayFabResultCommon
	{
		public uint DataVersion;
	}
}

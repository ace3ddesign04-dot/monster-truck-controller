using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class UpdateUserDataResult : PlayFabResultCommon
	{
		public uint DataVersion;
	}
}

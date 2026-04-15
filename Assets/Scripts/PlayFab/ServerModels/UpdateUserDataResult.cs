using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class UpdateUserDataResult : PlayFabResultCommon
	{
		public uint DataVersion;
	}
}

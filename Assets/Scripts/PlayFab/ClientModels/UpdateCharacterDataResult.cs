using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class UpdateCharacterDataResult : PlayFabResultCommon
	{
		public uint DataVersion;
	}
}

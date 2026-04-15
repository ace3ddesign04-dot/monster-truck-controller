using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class RemoveGenericIDRequest : PlayFabRequestCommon
	{
		public GenericServiceId GenericId;
	}
}

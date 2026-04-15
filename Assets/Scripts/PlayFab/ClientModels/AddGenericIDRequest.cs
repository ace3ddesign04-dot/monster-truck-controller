using PlayFab.SharedModels;
using System;

namespace PlayFab.ClientModels
{
	[Serializable]
	public class AddGenericIDRequest : PlayFabRequestCommon
	{
		public GenericServiceId GenericId;
	}
}

using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class GetTasksRequest : PlayFabRequestCommon
	{
		public NameIdentifier Identifier;
	}
}

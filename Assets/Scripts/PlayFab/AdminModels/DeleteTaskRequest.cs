using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class DeleteTaskRequest : PlayFabRequestCommon
	{
		public NameIdentifier Identifier;
	}
}

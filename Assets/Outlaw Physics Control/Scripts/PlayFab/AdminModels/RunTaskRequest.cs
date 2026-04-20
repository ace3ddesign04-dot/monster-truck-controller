using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class RunTaskRequest : PlayFabRequestCommon
	{
		public NameIdentifier Identifier;
	}
}

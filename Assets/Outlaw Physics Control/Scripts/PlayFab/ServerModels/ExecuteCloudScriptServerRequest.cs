using PlayFab.SharedModels;
using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class ExecuteCloudScriptServerRequest : PlayFabRequestCommon
	{
		public string FunctionName;

		public object FunctionParameter;

		public bool? GeneratePlayStreamEvent;

		public string PlayFabId;

		public CloudScriptRevisionOption? RevisionSelection;

		public int? SpecificRevision;
	}
}

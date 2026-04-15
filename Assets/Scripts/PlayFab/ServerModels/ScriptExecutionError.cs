using System;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class ScriptExecutionError
	{
		public string Error;

		public string Message;

		public string StackTrace;
	}
}

using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class ScriptExecutionError
	{
		public string Error;

		public string Message;

		public string StackTrace;
	}
}

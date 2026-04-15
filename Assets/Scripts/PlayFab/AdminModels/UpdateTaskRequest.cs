using PlayFab.SharedModels;
using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class UpdateTaskRequest : PlayFabRequestCommon
	{
		public string Description;

		public NameIdentifier Identifier;

		public bool IsActive;

		public string Name;

		public object Parameter;

		public string Schedule;

		public ScheduledTaskType Type;
	}
}

using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class PermissionStatement
	{
		public string Action;

		public ApiCondition ApiConditions;

		public string Comment;

		public EffectType Effect;

		public string Principal;

		public string Resource;
	}
}

using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class ItemGrant
	{
		public string Annotation;

		public string CharacterId;

		public Dictionary<string, string> Data;

		public string ItemId;

		public List<string> KeysToRemove;

		public string PlayFabId;
	}
}

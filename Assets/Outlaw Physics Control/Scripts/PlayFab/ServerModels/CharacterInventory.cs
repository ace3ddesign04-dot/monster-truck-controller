using System;
using System.Collections.Generic;

namespace PlayFab.ServerModels
{
	[Serializable]
	public class CharacterInventory
	{
		public string CharacterId;

		public List<ItemInstance> Inventory;
	}
}

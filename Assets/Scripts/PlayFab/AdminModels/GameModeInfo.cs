using System;

namespace PlayFab.AdminModels
{
	[Serializable]
	public class GameModeInfo
	{
		public string Gamemode;

		public uint MaxPlayerCount;

		public uint MinPlayerCount;

		public bool? StartOpen;
	}
}

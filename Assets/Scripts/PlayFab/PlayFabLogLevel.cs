using System;

namespace PlayFab
{
	[Flags]
	public enum PlayFabLogLevel
	{
		None = 0x0,
		Debug = 0x1,
		Info = 0x2,
		Warning = 0x4,
		Error = 0x8,
		All = 0xF
	}
}

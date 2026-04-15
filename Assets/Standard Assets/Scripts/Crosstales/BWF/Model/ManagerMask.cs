using System;

namespace Crosstales.BWF.Model
{
	[Flags]
	public enum ManagerMask
	{
		None = 0x0,
		All = 0x1,
		BadWord = 0x2,
		Domain = 0x4,
		Capitalization = 0x8,
		Punctuation = 0x10
	}
}

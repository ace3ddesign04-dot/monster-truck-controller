using System;
using UnityEngine;

namespace GeNa
{
	[ExecuteInEditMode]
	public class TerrainEvents : MonoBehaviour
	{
		[Flags]
		internal enum TerrainChangedFlags
		{
			NoChange = 0x0,
			Heightmap = 0x1,
			TreeInstances = 0x2,
			DelayedHeightmapUpdate = 0x4,
			FlushEverythingImmediately = 0x8,
			RemoveDirtyDetailsImmediately = 0x10,
			WillBeDestroyed = 0x100
		}

		private void OnTerrainChanged(int flags)
		{
			UnityEngine.Debug.Log((TerrainChangedFlags)flags);
		}
	}
}

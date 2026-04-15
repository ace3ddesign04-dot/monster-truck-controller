using System.Collections.Generic;
using UnityEngine;

namespace Gaia
{
	public class SpawnGrassExtension : SpawnRuleExtension
	{
		[Tooltip("The zero based index of the grass texture to be applied.")]
		public int m_grassIndex;

		[Tooltip("The minimum strength of the grass to be applied.")]
		[Range(0f, 15f)]
		public int m_minGrassStrenth;

		[Tooltip("The maximum strength of the grass to be applied.")]
		[Range(0f, 15f)]
		public int m_maxGrassStrength = 15;

		[Tooltip("The mask used to display this grass.")]
		public Texture2D m_grassMask;

		[Tooltip("Whether or not to normalise the mask. Normalisation allows the full dynamic range of the mask to be used.")]
		public bool m_normaliseMask = true;

		[Tooltip("Whether or not to invert the mask.")]
		public bool m_invertMask;

		[Tooltip("Whether or not to flip the mask.")]
		public bool m_flipMask;

		[Tooltip("The mask scale with respect to the areas bounds of this spawn.")]
		[Range(0.1f, 10f)]
		public float m_scaleMask = 1f;

		private UnityHeightMap m_textureHM;

		public override void Initialise()
		{
			m_textureHM = new UnityHeightMap(m_grassMask);
			if (m_normaliseMask && m_textureHM.HasData())
			{
				m_textureHM.Normalise();
			}
			if (m_invertMask && m_textureHM.HasData())
			{
				m_textureHM.Invert();
			}
			if (m_flipMask && m_textureHM.HasData())
			{
				m_textureHM.Flip();
			}
		}

		public override bool AffectsDetails()
		{
			return true;
		}

		public override void PostSpawn(SpawnRule spawnRule, ref SpawnInfo spawnInfo)
		{
			if (m_textureHM == null || !m_textureHM.HasData())
			{
				return;
			}
			Terrain terrain = TerrainHelper.GetTerrain(spawnInfo.m_hitLocationWU);
			if (terrain == null)
			{
				return;
			}
			List<HeightMap> detailMaps = spawnInfo.m_spawner.GetDetailMaps(spawnInfo.m_hitTerrain.GetInstanceID());
			if (detailMaps == null || m_grassIndex >= detailMaps.Count)
			{
				return;
			}
			Vector3 size = spawnInfo.m_hitTerrain.terrainData.size;
			float x = size.x;
			Vector3 size2 = spawnInfo.m_hitTerrain.terrainData.size;
			float z = size2.z;
			float num = spawnRule.GetMaxScaledRadius(ref spawnInfo) * m_scaleMask;
			float num2 = spawnInfo.m_hitLocationWU.x - num / 2f;
			float num3 = spawnInfo.m_hitLocationWU.z - num / 2f;
			float num4 = num2 + num;
			float num5 = num3 + num;
			float num6 = 0.5f;
			Vector3 zero = Vector3.zero;
			float num7 = 0f;
			float num8 = 0f;
			float num9 = m_maxGrassStrength - m_minGrassStrenth;
			for (float num10 = num2; num10 < num4; num10 += num6)
			{
				for (float num11 = num3; num11 < num5; num11 += num6)
				{
					zero = new Vector3(num10, spawnInfo.m_hitLocationWU.y, num11);
					zero = Utils.RotatePointAroundPivot(zero, spawnInfo.m_hitLocationWU, new Vector3(0f, spawnInfo.m_spawnRotationY, 0f));
					num7 = zero.x / x + 0.5f;
					num8 = zero.z / z + 0.5f;
					if (!(num7 < 0f) && !(num7 >= 1f) && !(num8 < 0f) && !(num8 > 1f))
					{
						detailMaps[m_grassIndex][num8, num7] = Mathf.Clamp(m_textureHM[(num10 - num2) / num, (num11 - num3) / num] * num9 + (float)m_minGrassStrenth, 0f, 15f);
					}
				}
			}
		}
	}
}

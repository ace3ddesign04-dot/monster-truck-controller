using System.Collections.Generic;
using UnityEngine;

namespace Gaia
{
	public class SpawnTextureExtension : SpawnRuleExtension
	{
		[Tooltip("The zero based index of the terrain texture to be applied.")]
		public int m_textureIndex;

		[Tooltip("The mask used to display this texture.")]
		public Texture2D m_textureMask;

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
			m_textureHM = new UnityHeightMap(m_textureMask);
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

		public override bool AffectsTextures()
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
			List<HeightMap> textureMaps = spawnInfo.m_spawner.GetTextureMaps(spawnInfo.m_hitTerrain.GetInstanceID());
			if (textureMaps == null || m_textureIndex >= textureMaps.Count)
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
			float num9 = 0f;
			float num10 = 1f;
			for (float num11 = num2; num11 < num4; num11 += num6)
			{
				for (float num12 = num3; num12 < num5; num12 += num6)
				{
					zero = new Vector3(num11, spawnInfo.m_hitLocationWU.y, num12);
					zero = Utils.RotatePointAroundPivot(zero, spawnInfo.m_hitLocationWU, new Vector3(0f, spawnInfo.m_spawnRotationY, 0f));
					num7 = zero.x / x + 0.5f;
					num8 = zero.z / z + 0.5f;
					if (num7 < 0f || num7 >= 1f || num8 < 0f || num8 > 1f)
					{
						continue;
					}
					num9 = textureMaps[m_textureIndex][num8, num7];
					num10 = m_textureHM[(num11 - num2) / num, (num12 - num3) / num];
					if (!(num10 > num9))
					{
						continue;
					}
					float num13 = num10 - num9;
					float num14 = 1f - num9;
					float num15 = 0f;
					if (num14 != 0f)
					{
						num15 = 1f - num13 / num14;
					}
					for (int i = 0; i < textureMaps.Count; i++)
					{
						if (i == m_textureIndex)
						{
							textureMaps[i][num8, num7] = num10;
						}
						else
						{
							HeightMap heightMap;
							float x2;
							float z2;
							(heightMap = textureMaps[i])[x2 = num8, z2 = num7] = heightMap[x2, z2] * num15;
						}
					}
				}
			}
			spawnInfo.m_spawner.SetTextureMapsDirty();
		}
	}
}

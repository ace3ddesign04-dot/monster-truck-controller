using System.IO;
using UnityEngine;

namespace Gaia
{
	public class UnityHeightMap : HeightMap
	{
		public Bounds m_boundsWU = default(Bounds);

		public UnityHeightMap()
		{
		}

		public UnityHeightMap(string path)
			: base(path)
		{
			m_boundsWU.size = new Vector3(m_widthX, 0f, m_depthZ);
			m_isDirty = false;
		}

		public UnityHeightMap(TextAsset source)
			: base(source.bytes)
		{
			m_boundsWU.size = new Vector3(m_widthX, 0f, m_depthZ);
			m_isDirty = false;
		}

		public UnityHeightMap(UnityHeightMap source)
			: base(source)
		{
			m_boundsWU = source.m_boundsWU;
			m_isDirty = false;
		}

		public UnityHeightMap(Terrain terrain)
		{
			LoadFromTerrain(terrain);
		}

		public UnityHeightMap(Bounds bounds, string sourceFile)
			: base(sourceFile)
		{
			m_boundsWU = bounds;
			m_isDirty = false;
		}

		public UnityHeightMap(Texture2D texture)
		{
			LoadFromTexture2D(texture);
			m_isDirty = false;
		}

		public Bounds GetBoundsWU()
		{
			return m_boundsWU;
		}

		public Vector3 GetPositionWU()
		{
			return m_boundsWU.center - m_boundsWU.extents;
		}

		public void SetBoundsWU(Bounds bounds)
		{
			m_boundsWU = bounds;
			m_isDirty = true;
		}

		public void SetPositionWU(Vector3 position)
		{
			m_boundsWU.center = position;
			m_isDirty = true;
		}

		public void LoadFromTerrain(Terrain terrain)
		{
			Reset();
			m_boundsWU.center = terrain.transform.position;
			m_boundsWU.size = terrain.terrainData.size;
			m_boundsWU.center += m_boundsWU.extents;
			m_widthX = terrain.terrainData.heightmapResolution;
			m_depthZ = terrain.terrainData.heightmapResolution;
			m_widthInvX = 1f / (float)m_widthX;
			m_depthInvZ = 1f / (float)m_depthZ;
			m_heights = terrain.terrainData.GetHeights(0, 0, m_widthX, m_depthZ);
			m_isPowerOf2 = (Utils.Math_IsPowerOf2(m_widthX) && Utils.Math_IsPowerOf2(m_depthZ));
			m_isDirty = false;
		}

		public void SaveToTerrain(Terrain terrain)
		{
			int heightmapWidth = terrain.terrainData.heightmapResolution;
			int heightmapHeight = terrain.terrainData.heightmapResolution;
			if (m_widthX == heightmapWidth && m_depthZ == heightmapHeight)
			{
				terrain.terrainData.SetHeights(0, 0, m_heights);
				m_isDirty = false;
				return;
			}
			float[,] array = new float[heightmapWidth, heightmapHeight];
			for (int i = 0; i < heightmapWidth; i++)
			{
				for (int j = 0; j < heightmapHeight; j++)
				{
					array[i, j] = base[(float)i / (float)heightmapWidth, (float)j / (float)heightmapHeight];
				}
			}
			terrain.terrainData.SetHeights(0, 0, array);
			m_isDirty = false;
		}

		public void LoadFromTexture2D(Texture2D texture)
		{
			Utils.MakeTextureReadable(texture);
			m_widthX = texture.width;
			m_depthZ = texture.height;
			m_widthInvX = 1f / (float)m_widthX;
			m_depthInvZ = 1f / (float)m_depthZ;
			m_heights = new float[m_widthX, m_depthZ];
			m_isPowerOf2 = (Utils.Math_IsPowerOf2(m_widthX) && Utils.Math_IsPowerOf2(m_depthZ));
			for (int i = 0; i < m_widthX; i++)
			{
				for (int j = 0; j < m_depthZ; j++)
				{
					m_heights[i, j] = texture.GetPixel(i, j).grayscale;
				}
			}
			m_isDirty = false;
		}

		public void ReadRawFromTextAsset(TextAsset asset)
		{
			using (Stream stream = new MemoryStream(asset.bytes))
			{
				using (BinaryReader binaryReader = new BinaryReader(stream))
				{
					m_widthX = (m_depthZ = Mathf.CeilToInt(Mathf.Sqrt(stream.Length / 2)));
					m_widthInvX = 1f / (float)m_widthX;
					m_depthInvZ = 1f / (float)m_depthZ;
					m_heights = new float[m_widthX, m_depthZ];
					m_isPowerOf2 = (Utils.Math_IsPowerOf2(m_widthX) && Utils.Math_IsPowerOf2(m_depthZ));
					for (int i = 0; i < m_widthX; i++)
					{
						for (int j = 0; j < m_depthZ; j++)
						{
							m_heights[i, j] = (float)(int)binaryReader.ReadUInt16() / 65535f;
						}
					}
				}
				stream.Close();
			}
			m_isDirty = false;
		}
	}
}

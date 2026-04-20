using System.Text;
using UnityEngine;

namespace Gaia
{
	public class GaiaWorldManager
	{
		private Bounds m_worldBoundsWU = default(Bounds);

		private Bounds m_worldBoundsTU = default(Bounds);

		private Bounds m_worldBoundsNU = default(Bounds);

		private Vector3 m_WUtoTU = Vector3.one;

		private Vector3 m_TUtoWU = Vector3.one;

		private Vector3 m_TUtoNU = Vector3.one;

		private Vector3 m_NUtoTU = Vector3.one;

		private Vector3 m_WUtoNU = Vector3.one;

		private Vector3 m_NUtoWU = Vector3.one;

		private Vector3 m_NUZeroOffset = Vector3.zero;

		private Vector3 m_TUZeroOffset = Vector3.zero;

		private ulong m_boundsCheckErrors;

		private Terrain[,] m_physicalTerrainArray;

		private UnityHeightMap[,] m_heightMapTerrainArray;

		private int m_tileCount;

		public int TileCount => m_tileCount;

		public Terrain[,] PhysicalTerrainArray
		{
			get
			{
				return m_physicalTerrainArray;
			}
			set
			{
				m_physicalTerrainArray = value;
			}
		}

		public UnityHeightMap[,] HeightMapTerrainArray
		{
			get
			{
				return m_heightMapTerrainArray;
			}
			set
			{
				m_heightMapTerrainArray = value;
			}
		}

		public Bounds WorldBoundsWU => m_worldBoundsWU;

		public Bounds WorldBoundsTU => m_worldBoundsTU;

		public Bounds WorldBoundsNU => m_worldBoundsNU;

		public Vector3 WUtoTUConversionFactor => m_WUtoTU;

		public Vector3 WUtoNUConversionFactor => m_WUtoNU;

		public ulong BoundsCheckErrors
		{
			get
			{
				return m_boundsCheckErrors;
			}
			set
			{
				m_boundsCheckErrors = value;
			}
		}

		public GaiaWorldManager()
		{
		}

		public GaiaWorldManager(Terrain[] terrains)
		{
			Terrain terrain = null;
			m_worldBoundsWU = default(Bounds);
			m_worldBoundsTU = default(Bounds);
			m_worldBoundsNU = default(Bounds);
			string text = IsValidWorld(terrains);
			if (!string.IsNullOrEmpty(text))
			{
				UnityEngine.Debug.LogError("GaiaWorldManager(terrains) ERROR" + text);
				return;
			}
			for (int i = 0; i < terrains.Length; i++)
			{
				terrain = terrains[i];
				Bounds bounds = new Bounds(terrain.transform.position, terrain.terrainData.size);
				bounds.center += bounds.extents;
				if (i == 0)
				{
					m_worldBoundsWU = new Bounds(bounds.center, bounds.size);
				}
				else
				{
					m_worldBoundsWU.Encapsulate(bounds);
				}
				Bounds bounds2 = default(Bounds);
				float num = terrain.terrainData.heightmapResolution;
				Vector3 size = terrain.terrainData.size;
				float x = num / size.x;
				float num2 = terrain.terrainData.heightmapResolution - 1;
				Vector3 size2 = terrain.terrainData.size;
				float x2 = size2.x;
				Vector3 size3 = terrain.terrainData.size;
				float num3 = num2 / Mathf.Max(x2, size3.z);
				Vector3 size4 = terrain.terrainData.size;
				float num4 = num3 * size4.y;
				Vector3 size5 = terrain.terrainData.size;
				float y = num4 / size5.y;
				float num5 = terrain.terrainData.heightmapResolution;
				Vector3 size6 = terrain.terrainData.size;
				m_WUtoTU = new Vector3(x, y, num5 / size6.z);
				m_TUtoWU = new Vector3(1f / m_WUtoTU.x, 1f / m_WUtoTU.y, 1f / m_WUtoTU.z);
				bounds2.center = Vector3.Scale(bounds.center, m_WUtoTU);
				bounds2.size = Vector3.Scale(bounds.size, m_WUtoTU);
				if (i == 0)
				{
					m_worldBoundsTU = new Bounds(bounds2.center, bounds2.size);
				}
				else
				{
					m_worldBoundsTU.Encapsulate(bounds2);
				}
			}
			if (terrain != null)
			{
				Vector3 size7 = m_worldBoundsTU.size;
				float x3 = 1f / size7.x;
				Vector3 size8 = m_worldBoundsTU.size;
				float y2 = 1f / size8.y;
				Vector3 size9 = m_worldBoundsTU.size;
				m_TUtoNU = new Vector3(x3, y2, 1f / size9.z);
				m_NUtoTU = m_worldBoundsTU.size;
				m_WUtoNU = Vector3.Scale(m_WUtoTU, m_TUtoNU);
				m_NUtoWU = m_worldBoundsWU.size;
			}
			m_worldBoundsNU.center = Vector3.Scale(m_worldBoundsTU.center, m_TUtoNU);
			m_worldBoundsNU.size = Vector3.Scale(m_worldBoundsTU.size, m_TUtoNU);
			m_NUZeroOffset = Vector3.zero - m_worldBoundsNU.min;
			m_TUZeroOffset = Vector3.zero - m_worldBoundsTU.min;
			Vector3 size10 = m_worldBoundsNU.size;
			float x4 = size10.x;
			Vector3 size11 = m_worldBoundsNU.size;
			m_tileCount = (int)(x4 * size11.z);
			Vector3 size12 = m_worldBoundsNU.size;
			int num6 = (int)size12.x;
			Vector3 size13 = m_worldBoundsNU.size;
			m_physicalTerrainArray = new Terrain[num6, (int)size13.z];
			Vector3 size14 = m_worldBoundsNU.size;
			int num7 = (int)size14.x;
			Vector3 size15 = m_worldBoundsNU.size;
			m_heightMapTerrainArray = new UnityHeightMap[num7, (int)size15.z];
			foreach (Terrain terrain2 in terrains)
			{
				Vector3 vector = WUtoPTI(terrain2.transform.position);
				m_physicalTerrainArray[(int)vector.x, (int)vector.z] = terrain2;
			}
		}

		public string IsValidWorld(Terrain[] terrains)
		{
			Terrain terrain = null;
			Terrain terrain2 = null;
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Terrain terrain3 in terrains)
			{
				if (terrain == null)
				{
					terrain = terrain3;
				}
				Vector3 size = terrain3.terrainData.size;
				float x = size.x;
				Vector3 size2 = terrain3.terrainData.size;
				if (x != size2.z)
				{
					StringBuilder stringBuilder2 = stringBuilder;
					string name = terrain3.name;
					Vector3 size3 = terrain3.terrainData.size;
					object arg = size3.x;
					Vector3 size4 = terrain3.terrainData.size;
					stringBuilder2.Append($"\nTerrain {name} is not a square {arg} {size4.z}");
				}
				if (terrain3.terrainData.size != terrain.terrainData.size)
				{
					stringBuilder.Append($"\nTerrain {terrain3.name} - {terrain.name} size does not match {terrain3.terrainData.size} {terrain.terrainData.size}");
				}
				if (terrain3.terrainData.heightmapResolution != terrain.terrainData.heightmapResolution)
				{
					stringBuilder.Append($"\nTerrain {terrain3.name} - {terrain.name} heightmapResolution does not match {terrain3.terrainData.heightmapResolution} {terrain.terrainData.heightmapResolution}");
				}
				if (terrain3.terrainData.alphamapResolution != terrain.terrainData.alphamapResolution)
				{
					stringBuilder.Append($"\nTerrain {terrain3.name} - {terrain.name} alphamapResolution does not match {terrain3.terrainData.alphamapResolution} {terrain.terrainData.alphamapResolution}");
				}
				if (terrain3.terrainData.baseMapResolution != terrain.terrainData.baseMapResolution)
				{
					stringBuilder.Append($"\nTerrain {terrain3.name} - {terrain.name} baseMapResolution does not match {terrain3.terrainData.baseMapResolution} {terrain.terrainData.baseMapResolution}");
				}
				if (terrain3.terrainData.detailResolution != terrain.terrainData.detailResolution)
				{
					stringBuilder.Append($"\nTerrain {terrain3.name} - {terrain.name} detailResolution does not match {terrain3.terrainData.detailResolution} {terrain.terrainData.detailResolution}");
				}
				if (terrain3.terrainData.alphamapLayers != terrain.terrainData.alphamapLayers)
				{
					stringBuilder.Append($"\nTerrain {terrain3.name} - {terrain.name} alphamapLayers does not match {terrain3.terrainData.alphamapLayers} {terrain.terrainData.alphamapLayers}");
				}
				if (terrain3.terrainData.detailPrototypes.Length != terrain.terrainData.detailPrototypes.Length)
				{
					stringBuilder.Append($"\nTerrain {terrain3.name} - {terrain.name} detailPrototypes.Length does not match {terrain3.terrainData.detailPrototypes.Length} {terrain.terrainData.detailPrototypes.Length}");
				}
				if (terrain3.terrainData.splatPrototypes.Length != terrain.terrainData.splatPrototypes.Length)
				{
					stringBuilder.Append($"\nTerrain {terrain3.name} - {terrain.name} splatPrototypes.Length does not match {terrain3.terrainData.splatPrototypes.Length} {terrain.terrainData.splatPrototypes.Length}");
				}
				if (terrain3.terrainData.treePrototypes.Length != terrain.terrainData.treePrototypes.Length)
				{
					stringBuilder.Append($"\nTerrain {terrain3.name} - {terrain.name} treePrototypes.Length does not match {terrain3.terrainData.treePrototypes.Length} {terrain.terrainData.treePrototypes.Length}");
				}
			}
			return stringBuilder.ToString();
		}

		private Terrain GetTerrainWU(Vector3 positionWU)
		{
			if (!InBoundsWU(positionWU))
			{
				m_boundsCheckErrors++;
				return null;
			}
			Vector3 vector = WUtoPTI(positionWU);
			return m_physicalTerrainArray[(int)vector.x, (int)vector.z];
		}

		private Terrain GetTerrainTU(Vector3 positionTU)
		{
			if (!InBoundsTU(positionTU))
			{
				m_boundsCheckErrors++;
				return null;
			}
			Vector3 vector = TUtoPTI(positionTU);
			return m_physicalTerrainArray[(int)vector.x, (int)vector.z];
		}

		private Terrain GetTerrainNU(Vector3 positionNU)
		{
			if (!InBoundsNU(positionNU))
			{
				m_boundsCheckErrors++;
				return null;
			}
			Vector3 vector = NUtoPTI(positionNU);
			return m_physicalTerrainArray[(int)vector.x, (int)vector.z];
		}

		private UnityHeightMap GetHeightMapWU(Vector3 positionWU)
		{
			if (!InBoundsWU(positionWU))
			{
				m_boundsCheckErrors++;
				return null;
			}
			Vector3 vector = WUtoPTI(positionWU);
			UnityHeightMap unityHeightMap = m_heightMapTerrainArray[(int)vector.x, (int)vector.z];
			if (unityHeightMap == null)
			{
				Terrain terrainWU = GetTerrainWU(positionWU);
				if (terrainWU != null)
				{
					unityHeightMap = (m_heightMapTerrainArray[(int)vector.x, (int)vector.z] = new UnityHeightMap(terrainWU));
				}
			}
			return unityHeightMap;
		}

		private UnityHeightMap GetHeightMapTU(Vector3 positionTU)
		{
			if (!InBoundsTU(positionTU))
			{
				m_boundsCheckErrors++;
				return null;
			}
			Vector3 vector = TUtoPTI(positionTU);
			UnityHeightMap unityHeightMap = m_heightMapTerrainArray[(int)vector.x, (int)vector.z];
			if (unityHeightMap == null)
			{
				Terrain terrainTU = GetTerrainTU(positionTU);
				if (terrainTU != null)
				{
					unityHeightMap = (m_heightMapTerrainArray[(int)vector.x, (int)vector.z] = new UnityHeightMap(terrainTU));
				}
			}
			return unityHeightMap;
		}

		private UnityHeightMap GetHeightMapNU(Vector3 positionNU)
		{
			if (!InBoundsNU(positionNU))
			{
				m_boundsCheckErrors++;
				return null;
			}
			Vector3 vector = NUtoPTI(positionNU);
			UnityHeightMap unityHeightMap = m_heightMapTerrainArray[(int)vector.x, (int)vector.z];
			if (unityHeightMap == null)
			{
				Terrain terrainNU = GetTerrainNU(positionNU);
				if (terrainNU != null)
				{
					unityHeightMap = (m_heightMapTerrainArray[(int)vector.x, (int)vector.z] = new UnityHeightMap(terrainNU));
				}
			}
			return unityHeightMap;
		}

		public void LoadFromWorld()
		{
			for (int i = 0; i < m_heightMapTerrainArray.GetLength(0); i++)
			{
				for (int j = 0; j < m_heightMapTerrainArray.GetLength(1); j++)
				{
					UnityHeightMap unityHeightMap = m_heightMapTerrainArray[i, j];
					if (unityHeightMap == null)
					{
						Terrain terrain = m_physicalTerrainArray[i, j];
						if (terrain != null)
						{
							m_heightMapTerrainArray[i, j] = new UnityHeightMap(terrain);
						}
					}
					else
					{
						unityHeightMap.LoadFromTerrain(m_physicalTerrainArray[i, j]);
					}
				}
			}
		}

		public void SaveToWorld(bool forceWrite = false)
		{
			for (int i = 0; i < m_heightMapTerrainArray.GetLength(0); i++)
			{
				for (int j = 0; j < m_heightMapTerrainArray.GetLength(1); j++)
				{
					UnityHeightMap unityHeightMap = m_heightMapTerrainArray[i, j];
					if (unityHeightMap == null)
					{
						continue;
					}
					if (!forceWrite)
					{
						if (unityHeightMap.IsDirty())
						{
							unityHeightMap.SaveToTerrain(m_physicalTerrainArray[i, j]);
						}
					}
					else
					{
						unityHeightMap.SaveToTerrain(m_physicalTerrainArray[i, j]);
					}
				}
			}
		}

		public void SetHeightWU(float heightWU)
		{
			Vector3 size = m_worldBoundsWU.size;
			float height = Mathf.Clamp01(heightWU / size.y);
			for (int i = 0; i < m_heightMapTerrainArray.GetLength(0); i++)
			{
				for (int j = 0; j < m_heightMapTerrainArray.GetLength(1); j++)
				{
					m_heightMapTerrainArray[i, j].SetHeight(height);
				}
			}
		}

		public void SetHeightWU(Vector3 positionWU, float height)
		{
			UnityHeightMap heightMapWU = GetHeightMapWU(positionWU);
			if (heightMapWU != null)
			{
				positionWU = WUtoPTO(positionWU);
				heightMapWU[(int)positionWU.x, (int)positionWU.z] = height;
			}
			else
			{
				m_boundsCheckErrors++;
			}
		}

		public float GetHeightWU(Vector3 positionWU)
		{
			UnityHeightMap heightMapWU = GetHeightMapWU(positionWU);
			if (heightMapWU != null)
			{
				positionWU = WUtoPTO(positionWU);
				return heightMapWU[(int)positionWU.x, (int)positionWU.z];
			}
			return float.MinValue;
		}

		public float GetHeightInterpolatedWU(Vector3 positionWU)
		{
			UnityHeightMap heightMapWU = GetHeightMapWU(positionWU);
			if (heightMapWU != null)
			{
				positionWU = WUtoPTO(positionWU);
				return heightMapWU[positionWU.x, positionWU.z];
			}
			return float.MinValue;
		}

		public void SetHeightTU(Vector3 positionTU, float height)
		{
			UnityHeightMap heightMapTU = GetHeightMapTU(positionTU);
			if (heightMapTU != null)
			{
				positionTU = TUtoPTO(positionTU);
				heightMapTU[(int)positionTU.x, (int)positionTU.z] = height;
			}
			else
			{
				m_boundsCheckErrors++;
			}
		}

		public float GetHeightTU(Vector3 positionTU)
		{
			UnityHeightMap heightMapTU = GetHeightMapTU(positionTU);
			if (heightMapTU != null)
			{
				positionTU = TUtoPTO(positionTU);
				return heightMapTU[(int)positionTU.x, (int)positionTU.z];
			}
			return float.MinValue;
		}

		public float GetHeightInterpolatedTU(Vector3 positionTU)
		{
			UnityHeightMap heightMapTU = GetHeightMapTU(positionTU);
			if (heightMapTU != null)
			{
				positionTU = TUtoPTO(positionTU);
				return heightMapTU[positionTU.x, positionTU.z];
			}
			return float.MinValue;
		}

		public void FlattenWorld()
		{
			for (int i = 0; i < m_heightMapTerrainArray.GetLength(0); i++)
			{
				for (int j = 0; j < m_heightMapTerrainArray.GetLength(1); j++)
				{
					UnityHeightMap unityHeightMap = m_heightMapTerrainArray[i, j];
					if (unityHeightMap == null)
					{
						Terrain terrain = m_physicalTerrainArray[i, j];
						if (terrain != null)
						{
							unityHeightMap = (m_heightMapTerrainArray[i, j] = new UnityHeightMap(terrain));
						}
					}
					if (unityHeightMap != null)
					{
						unityHeightMap.SetHeight(0f);
						unityHeightMap.SaveToTerrain(m_physicalTerrainArray[i, j]);
					}
				}
			}
		}

		public void SmoothWorld()
		{
			for (int i = 0; i < m_heightMapTerrainArray.GetLength(0); i++)
			{
				for (int j = 0; j < m_heightMapTerrainArray.GetLength(1); j++)
				{
					UnityHeightMap unityHeightMap = m_heightMapTerrainArray[i, j];
					if (unityHeightMap == null)
					{
						Terrain terrain = m_physicalTerrainArray[i, j];
						if (terrain != null)
						{
							unityHeightMap = (m_heightMapTerrainArray[i, j] = new UnityHeightMap(terrain));
						}
					}
					if (unityHeightMap != null)
					{
						unityHeightMap.Smooth(1);
						unityHeightMap.SaveToTerrain(m_physicalTerrainArray[i, j]);
					}
				}
			}
		}

		public void ExportWorldAsPng(string path)
		{
			Vector3 center = m_worldBoundsTU.center;
			Vector3 size = m_worldBoundsTU.size;
			int width = (int)size.z;
			Vector3 size2 = m_worldBoundsTU.size;
			HeightMap heightMap = new HeightMap(width, (int)size2.x);
			int num = 0;
			Vector3 min = m_worldBoundsTU.min;
			int num2 = (int)min.x;
			while (true)
			{
				int num3 = num2;
				Vector3 max = m_worldBoundsTU.max;
				if (num3 >= (int)max.x)
				{
					break;
				}
				center.x = num2;
				int num4 = 0;
				Vector3 min2 = m_worldBoundsTU.min;
				int num5 = (int)min2.z;
				while (true)
				{
					int num6 = num5;
					Vector3 max2 = m_worldBoundsTU.max;
					if (num6 >= (int)max2.z)
					{
						break;
					}
					center.z = num5;
					heightMap[num4, num] = GetHeightTU(center);
					num4++;
					num5++;
				}
				num++;
				num2++;
			}
			Utils.CompressToSingleChannelFileImage(heightMap.Heights(), path, TextureFormat.RGBA32, exportPNG: true, exportJPG: false);
		}

		public void ExportSplatmapAsPng(string path, int textureIdx)
		{
			Terrain activeTerrain = Terrain.activeTerrain;
			if (activeTerrain == null)
			{
				UnityEngine.Debug.LogError("No active terrain, unable to export splatmaps");
				return;
			}
			int alphamapWidth = activeTerrain.terrainData.alphamapWidth;
			int alphamapHeight = activeTerrain.terrainData.alphamapHeight;
			int alphamapLayers = activeTerrain.terrainData.alphamapLayers;
			if (textureIdx < alphamapLayers)
			{
				HeightMap heightMap = new HeightMap(activeTerrain.terrainData.GetAlphamaps(0, 0, alphamapWidth, alphamapHeight), textureIdx);
				heightMap.Flip();
				Utils.CompressToSingleChannelFileImage(heightMap.Heights(), path, TextureFormat.RGBA32, exportPNG: true, exportJPG: false);
			}
			else
			{
				float[,,] alphamaps = activeTerrain.terrainData.GetAlphamaps(0, 0, alphamapWidth, alphamapHeight);
				Utils.CompressToMultiChannelFileImage(alphamaps, path, TextureFormat.RGBA32, exportPNG: true, exportJPG: false);
			}
		}

		public void ExportGrassmapAsPng(string path)
		{
			Terrain activeTerrain = Terrain.activeTerrain;
			if (activeTerrain == null)
			{
				UnityEngine.Debug.LogError("No active terrain, unable to export grassmaps");
				return;
			}
			int detailWidth = activeTerrain.terrainData.detailWidth;
			int detailHeight = activeTerrain.terrainData.detailHeight;
			int num = activeTerrain.terrainData.detailPrototypes.Length;
			float[,,] array = new float[detailWidth, detailHeight, num];
			for (int i = 0; i < activeTerrain.terrainData.detailPrototypes.Length; i++)
			{
				int[,] detailLayer = activeTerrain.terrainData.GetDetailLayer(0, 0, activeTerrain.terrainData.detailWidth, activeTerrain.terrainData.detailHeight, i);
				for (int j = 0; j < detailWidth; j++)
				{
					for (int k = 0; k < detailHeight; k++)
					{
						array[j, k, i] = (float)detailLayer[j, k] / 16f;
					}
				}
				for (int l = 0; l < detailWidth; l++)
				{
					for (int m = 0; m < detailHeight; m++)
					{
						array[m, l, i] = array[l, m, i];
					}
				}
			}
			Utils.CompressToMultiChannelFileImage(array, path, TextureFormat.RGBA32, exportPNG: true, exportJPG: false);
		}

		public void ExportNormalmapAsPng(string path)
		{
			Terrain terrain = null;
			int num = 0;
			int num2 = 0;
			float[,,] array = null;
			for (int i = 0; i < m_physicalTerrainArray.GetLength(0); i++)
			{
				for (int j = 0; j < m_physicalTerrainArray.GetLength(1); j++)
				{
					terrain = m_physicalTerrainArray[i, j];
					if (!(terrain != null))
					{
						continue;
					}
					num = terrain.terrainData.heightmapResolution;
					num2 = terrain.terrainData.heightmapResolution;
					array = new float[num, num2, 4];
					for (int k = 0; k < num; k++)
					{
						for (int l = 0; l < num2; l++)
						{
							Vector3 interpolatedNormal = terrain.terrainData.GetInterpolatedNormal((float)k / (float)num, (float)l / (float)num2);
							array[k, l, 0] = interpolatedNormal.x * 0.5f + 0.5f;
							array[k, l, 1] = interpolatedNormal.y * 0.5f + 0.5f;
							array[k, l, 2] = interpolatedNormal.z * 0.5f + 0.5f;
						}
					}
					Utils.CompressToMultiChannelFileImage(array, path + "_" + i + "_" + j, TextureFormat.RGBA32, exportPNG: true, exportJPG: false);
				}
			}
		}

		public void ExportShorelineMask(string path, float shoreHeightWU, float shoreWidthWU)
		{
			Vector3 center = m_worldBoundsTU.center;
			Vector3 size = m_worldBoundsWU.size;
			float shoreHeightNU = shoreHeightWU / size.y;
			Vector3 vector = WUtoTU(new Vector3(shoreWidthWU, shoreWidthWU, shoreWidthWU));
			Vector3 size2 = m_worldBoundsTU.size;
			int width = (int)size2.z;
			Vector3 size3 = m_worldBoundsTU.size;
			HeightMap heightMap = new HeightMap(width, (int)size3.x);
			float num = 0f;
			Vector3 min = m_worldBoundsTU.min;
			float num2 = min.x;
			while (true)
			{
				float num3 = num2;
				Vector3 max = m_worldBoundsTU.max;
				if (!(num3 < max.x))
				{
					break;
				}
				center.x = num2;
				float num4 = 0f;
				Vector3 min2 = m_worldBoundsTU.min;
				float num5 = min2.z;
				while (true)
				{
					float num6 = num5;
					Vector3 max2 = m_worldBoundsTU.max;
					if (!(num6 < max2.z))
					{
						break;
					}
					center.z = num5;
					MakeMask(center, shoreHeightNU, vector.x, heightMap);
					num4 += 1f;
					num5 += 1f;
				}
				num += 1f;
				num2 += 1f;
			}
			heightMap.Flip();
			Utils.CompressToSingleChannelFileImage(heightMap.Heights(), path, TextureFormat.RGBA32, exportPNG: true, exportJPG: false);
		}

		private void MakeMask(Vector3 positionTU, float shoreHeightNU, float maskSizeTU, HeightMap waterMask)
		{
			float num = positionTU.x - maskSizeTU;
			float num2 = positionTU.x + maskSizeTU;
			float num3 = positionTU.z - maskSizeTU;
			float num4 = positionTU.z + maskSizeTU;
			Vector3 center = m_worldBoundsTU.center;
			for (float num5 = num; num5 < num2; num5 += 1f)
			{
				center.x = num5;
				for (float num6 = num3; num6 < num4; num6 += 1f)
				{
					center.z = num6;
					if (!InBoundsTU(center) || !(GetHeightTU(center) <= shoreHeightNU))
					{
						continue;
					}
					float num7 = Utils.Math_Distance(num5, num6, positionTU.x, positionTU.z) / maskSizeTU;
					if (num7 <= 1f)
					{
						num7 = 1f - num7;
						int x = (int)(num5 + m_TUZeroOffset.x);
						int z = (int)(num6 + m_TUZeroOffset.z);
						if (num7 > waterMask[x, z])
						{
							waterMask[x, z] = num7;
						}
					}
				}
			}
		}

		public bool InBoundsWU(Vector3 positionWU)
		{
			float x = positionWU.x;
			Vector3 min = m_worldBoundsWU.min;
			if (x >= min.x)
			{
				float z = positionWU.z;
				Vector3 min2 = m_worldBoundsWU.min;
				if (z >= min2.z)
				{
					float x2 = positionWU.x;
					Vector3 max = m_worldBoundsWU.max;
					if (x2 < max.x)
					{
						float z2 = positionWU.z;
						Vector3 max2 = m_worldBoundsWU.max;
						if (z2 < max2.z)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public bool InBoundsTU(Vector3 positionTU)
		{
			float x = positionTU.x;
			Vector3 min = m_worldBoundsTU.min;
			if (x >= min.x)
			{
				float z = positionTU.z;
				Vector3 min2 = m_worldBoundsTU.min;
				if (z >= min2.z)
				{
					float x2 = positionTU.x;
					Vector3 max = m_worldBoundsTU.max;
					if (x2 < max.x)
					{
						float z2 = positionTU.z;
						Vector3 max2 = m_worldBoundsTU.max;
						if (z2 < max2.z)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public bool InBoundsNU(Vector3 positionNU)
		{
			float x = positionNU.x;
			Vector3 min = m_worldBoundsNU.min;
			if (x >= min.x)
			{
				float z = positionNU.z;
				Vector3 min2 = m_worldBoundsNU.min;
				if (z >= min2.z)
				{
					float x2 = positionNU.x;
					Vector3 max = m_worldBoundsNU.max;
					if (x2 < max.x)
					{
						float z2 = positionNU.z;
						Vector3 max2 = m_worldBoundsNU.max;
						if (z2 < max2.z)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public Vector3 WUtoTU(Vector3 positionWU)
		{
			return Vector3.Scale(positionWU, m_WUtoTU);
		}

		public Vector3 WUtoNU(Vector3 positionWU)
		{
			return Vector3.Scale(positionWU, m_WUtoNU);
		}

		public Vector3 WUtoPTI(Vector3 positionWU)
		{
			return NUtoPTI(WUtoNU(positionWU));
		}

		public Vector3 WUtoPTO(Vector3 positionWU)
		{
			return TUtoPTO(WUtoTU(positionWU));
		}

		public Vector3 TUtoWU(Vector3 positionTU)
		{
			return Vector3.Scale(positionTU, m_TUtoWU);
		}

		public Vector3 TUtoNU(Vector3 positionTU)
		{
			return Vector3.Scale(positionTU, m_TUtoNU);
		}

		public Vector3 TUtoPTI(Vector3 positionTU)
		{
			return NUtoPTI(TUtoNU(positionTU));
		}

		public Vector3 TUtoPTO(Vector3 positionTU)
		{
			float num = positionTU.x + m_TUZeroOffset.x;
			Vector3 size = m_worldBoundsTU.size;
			float x = num % size.x;
			float num2 = positionTU.y + m_TUZeroOffset.y;
			Vector3 size2 = m_worldBoundsTU.size;
			float y = num2 % size2.y;
			float num3 = positionTU.z + m_TUZeroOffset.z;
			Vector3 size3 = m_worldBoundsTU.size;
			return new Vector3(x, y, num3 % size3.z);
		}

		public Vector3 NUtoWU(Vector3 positionNU)
		{
			return Vector3.Scale(positionNU, m_NUtoWU);
		}

		public Vector3 NUtoTU(Vector3 positionNU)
		{
			return Vector3.Scale(positionNU, m_NUtoTU);
		}

		public Vector3 NUtoPTI(Vector3 positionNU)
		{
			return new Vector3(Mathf.Floor(positionNU.x + m_NUZeroOffset.x), Mathf.Floor(positionNU.y + m_NUZeroOffset.y), Mathf.Floor(positionNU.z + m_NUZeroOffset.z));
		}

		public Vector3 NUtoPTO(Vector3 positionNU)
		{
			float num = (positionNU.x + m_NUZeroOffset.x) % 1f;
			Vector3 size = m_worldBoundsTU.size;
			float x = num * size.x;
			float num2 = (positionNU.y + m_NUZeroOffset.y) % 1f;
			Vector3 size2 = m_worldBoundsTU.size;
			float y = num2 * size2.y;
			float num3 = (positionNU.z + m_NUZeroOffset.z) % 1f;
			Vector3 size3 = m_worldBoundsTU.size;
			return new Vector3(x, y, num3 * size3.z);
		}

		public Vector3 Ceil(Vector3 source)
		{
			return new Vector3(Mathf.Ceil(source.x), Mathf.Ceil(source.y), Mathf.Ceil(source.z));
		}

		public Vector3 Floor(Vector3 source)
		{
			return new Vector3(Mathf.Floor(source.x), Mathf.Floor(source.y), Mathf.Floor(source.z));
		}

		public void Test()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("GaiaWorldManagerTest\n");
			stringBuilder.Append($"World Bounds WU : Min {m_worldBoundsWU.min}, Centre {m_worldBoundsWU.center}, Max {m_worldBoundsWU.max}, Size {m_worldBoundsWU.size}\n");
			stringBuilder.Append($"World Bounds TU : Min {m_worldBoundsTU.min}, Centre {m_worldBoundsTU.center}, Max {m_worldBoundsTU.max}, Size {m_worldBoundsTU.size}\n");
			stringBuilder.Append($"World Bounds NU : Min {m_worldBoundsNU.min}, Centre {m_worldBoundsNU.center}, Max {m_worldBoundsNU.max}, Size {m_worldBoundsNU.size}\n");
			stringBuilder.Append("\nBounds Tests:");
			Vector3 min = m_worldBoundsWU.min;
			float x = min.x - 1f;
			Vector3 min2 = m_worldBoundsWU.min;
			float y = min2.y;
			Vector3 min3 = m_worldBoundsWU.min;
			Vector3 vector = new Vector3(x, y, min3.z);
			stringBuilder.Append($"\n<MIN - InBoundsWU({vector}) = {InBoundsWU(vector)}\n");
			Vector3 min4 = m_worldBoundsWU.min;
			float x2 = min4.x;
			Vector3 min5 = m_worldBoundsWU.min;
			float y2 = min5.y;
			Vector3 min6 = m_worldBoundsWU.min;
			vector = new Vector3(x2, y2, min6.z);
			stringBuilder.Append($"  MIN - InBoundsWU({vector}) = {InBoundsWU(vector)}\n");
			Vector3 max = m_worldBoundsWU.max;
			float x3 = max.x;
			Vector3 max2 = m_worldBoundsWU.max;
			float y3 = max2.y;
			Vector3 max3 = m_worldBoundsWU.max;
			vector = new Vector3(x3, y3, max3.z);
			stringBuilder.Append($"  MAX - InBoundsWU({vector}) = {InBoundsWU(vector)}\n");
			Vector3 max4 = m_worldBoundsWU.max;
			float x4 = max4.x + 1f;
			Vector3 max5 = m_worldBoundsWU.max;
			float y4 = max5.y;
			Vector3 max6 = m_worldBoundsWU.max;
			vector = new Vector3(x4, y4, max6.z);
			stringBuilder.Append($">MAX - InBoundsWU({vector}) = {InBoundsWU(vector)}\n");
			Vector3 min7 = m_worldBoundsTU.min;
			float x5 = min7.x - 1f;
			Vector3 min8 = m_worldBoundsTU.min;
			float y5 = min8.y;
			Vector3 min9 = m_worldBoundsTU.min;
			vector = new Vector3(x5, y5, min9.z);
			stringBuilder.Append($"\n<MIN - InBoundsTU({vector}) = {InBoundsTU(vector)}\n");
			Vector3 min10 = m_worldBoundsTU.min;
			float x6 = min10.x;
			Vector3 min11 = m_worldBoundsTU.min;
			float y6 = min11.y;
			Vector3 min12 = m_worldBoundsTU.min;
			vector = new Vector3(x6, y6, min12.z);
			stringBuilder.Append($"  MIN - InBoundsTU({vector}) = {InBoundsTU(vector)}\n");
			Vector3 max7 = m_worldBoundsTU.max;
			float x7 = max7.x;
			Vector3 max8 = m_worldBoundsTU.max;
			float y7 = max8.y;
			Vector3 max9 = m_worldBoundsTU.max;
			vector = new Vector3(x7, y7, max9.z);
			stringBuilder.Append($"  MAX - InBoundsTU({vector}) = {InBoundsTU(vector)}\n");
			Vector3 max10 = m_worldBoundsTU.max;
			float x8 = max10.x + 1f;
			Vector3 max11 = m_worldBoundsTU.max;
			float y8 = max11.y;
			Vector3 max12 = m_worldBoundsTU.max;
			vector = new Vector3(x8, y8, max12.y);
			stringBuilder.Append($">MAX - InBoundsTU({vector}) = {InBoundsTU(vector)}\n");
			Vector3 min13 = m_worldBoundsNU.min;
			float x9 = min13.x - 0.1f;
			Vector3 min14 = m_worldBoundsNU.min;
			float y9 = min14.y;
			Vector3 min15 = m_worldBoundsNU.min;
			vector = new Vector3(x9, y9, min15.z);
			stringBuilder.Append($"\n<MIN - InBoundsNU({vector}) = {InBoundsNU(vector)}\n");
			Vector3 min16 = m_worldBoundsNU.min;
			float x10 = min16.x;
			Vector3 min17 = m_worldBoundsNU.min;
			float y10 = min17.y;
			Vector3 min18 = m_worldBoundsNU.min;
			vector = new Vector3(x10, y10, min18.z);
			stringBuilder.Append($"  MIN - InBoundsNU({vector}) = {InBoundsNU(vector)}\n");
			Vector3 max13 = m_worldBoundsNU.max;
			float x11 = max13.x;
			Vector3 max14 = m_worldBoundsNU.max;
			float y11 = max14.y;
			Vector3 max15 = m_worldBoundsNU.max;
			vector = new Vector3(x11, y11, max15.z);
			stringBuilder.Append($"  MAX - InBoundsNU({vector}) = {InBoundsNU(vector)}\n");
			Vector3 max16 = m_worldBoundsNU.max;
			float x12 = max16.x + 0.1f;
			Vector3 max17 = m_worldBoundsNU.max;
			float y12 = max17.y;
			Vector3 max18 = m_worldBoundsNU.max;
			vector = new Vector3(x12, y12, max18.z);
			stringBuilder.Append($">MAX - InBoundsNU({vector}) = {InBoundsNU(vector)}\n");
			stringBuilder.Append("\nPosition Conversion Tests (<MIN, CENTRE, >MAX):");
			Vector3 min19 = m_worldBoundsWU.min;
			float x13 = min19.x - 1f;
			Vector3 center = m_worldBoundsWU.center;
			float y13 = center.y;
			Vector3 max19 = m_worldBoundsWU.max;
			vector = new Vector3(x13, y13, max19.z + 1f);
			stringBuilder.Append($"\nInBoundsWU({vector}) = {InBoundsWU(vector)}\n");
			StringBuilder stringBuilder2 = stringBuilder;
			object arg = vector;
			Vector3 vector2 = WUtoTU(vector);
			object arg2 = vector2.x;
			Vector3 vector3 = WUtoTU(vector);
			stringBuilder2.Append($"WUtoTU({arg}) = {arg2:0.000}, {vector3.z:0.000}\n");
			StringBuilder stringBuilder3 = stringBuilder;
			object arg3 = vector;
			Vector3 vector4 = WUtoNU(vector);
			object arg4 = vector4.x;
			Vector3 vector5 = WUtoNU(vector);
			stringBuilder3.Append($"WUtoNU({arg3}) = {arg4:0.000}, {vector5.z:0.000}\n");
			StringBuilder stringBuilder4 = stringBuilder;
			object arg5 = vector;
			Vector3 vector6 = WUtoPTI(vector);
			object arg6 = vector6.x;
			Vector3 vector7 = WUtoPTI(vector);
			stringBuilder4.Append($"WUtoPTI({arg5}) = {arg6}, {vector7.z}\n");
			StringBuilder stringBuilder5 = stringBuilder;
			object arg7 = vector;
			Vector3 vector8 = WUtoPTO(vector);
			object arg8 = vector8.x;
			Vector3 vector9 = WUtoPTO(vector);
			stringBuilder5.Append($"WUtoPTO({arg7}) = {arg8}, {vector9.z}\n");
			stringBuilder.Append("\nPosition Conversion Tests (MIN, CENTRE, MAX):");
			Vector3 min20 = m_worldBoundsWU.min;
			float x14 = min20.x;
			Vector3 center2 = m_worldBoundsWU.center;
			float y14 = center2.y;
			Vector3 max20 = m_worldBoundsWU.max;
			vector = new Vector3(x14, y14, max20.z);
			stringBuilder.Append($"\nInBoundsWU({vector}) = {InBoundsWU(vector)}\n");
			StringBuilder stringBuilder6 = stringBuilder;
			object arg9 = vector;
			Vector3 vector10 = WUtoTU(vector);
			object arg10 = vector10.x;
			Vector3 vector11 = WUtoTU(vector);
			stringBuilder6.Append($"WUtoTU({arg9}) = {arg10:0.000}, {vector11.z:0.000}\n");
			StringBuilder stringBuilder7 = stringBuilder;
			object arg11 = vector;
			Vector3 vector12 = WUtoNU(vector);
			object arg12 = vector12.x;
			Vector3 vector13 = WUtoNU(vector);
			stringBuilder7.Append($"WUtoNU({arg11}) = {arg12:0.000}, {vector13.z:0.000}\n");
			StringBuilder stringBuilder8 = stringBuilder;
			object arg13 = vector;
			Vector3 vector14 = WUtoPTI(vector);
			object arg14 = vector14.x;
			Vector3 vector15 = WUtoPTI(vector);
			stringBuilder8.Append($"WUtoPTI({arg13}) = {arg14}, {vector15.z}\n");
			StringBuilder stringBuilder9 = stringBuilder;
			object arg15 = vector;
			Vector3 vector16 = WUtoPTO(vector);
			object arg16 = vector16.x;
			Vector3 vector17 = WUtoPTO(vector);
			stringBuilder9.Append($"WUtoPTO({arg15}) = {arg16}, {vector17.z}\n");
			vector = WUtoTU(vector);
			stringBuilder.Append($"\nTUtoWU({vector}) = {TUtoWU(vector)}\n");
			stringBuilder.Append($"TUtoNU({vector}) = {TUtoNU(vector)}\n");
			vector = TUtoNU(vector);
			stringBuilder.Append($"\nNUtoWU({vector}) = {NUtoWU(vector)}\n");
			stringBuilder.Append($"NUtoTU({vector}) = {NUtoTU(vector)}\n");
			stringBuilder.Append("\nTerrain Tests:");
			FlattenWorld();
			m_boundsCheckErrors = 0uL;
			TestBlobWU(m_worldBoundsWU.min, 100, 0.25f);
			TestBlobTU(m_worldBoundsTU.center, 100, 0.5f);
			TestBlobWU(m_worldBoundsWU.max, 100, 1f);
			SaveToWorld();
			stringBuilder.Append($"Bounds check errors : {m_boundsCheckErrors}");
			UnityEngine.Debug.Log(stringBuilder.ToString());
		}

		public void TestBlobWU(Vector3 positionWU, int widthWU, float height)
		{
			Vector3 vector = WUtoTU(new Vector3(widthWU, widthWU, widthWU));
			Vector3 vector2 = WUtoTU(positionWU);
			Vector3 positionTU = default(Vector3);
			for (int i = (int)(vector2.x - vector.x); i < (int)(vector2.x + vector.x); i++)
			{
				for (int j = (int)(vector2.z - vector.z); j < (int)(vector2.z + vector.z); j++)
				{
					float x = i;
					Vector3 center = m_worldBoundsTU.center;
					positionTU = new Vector3(x, center.y, j);
					SetHeightTU(positionTU, height);
				}
			}
		}

		public void TestBlobTU(Vector3 positionTU, int widthWU, float height)
		{
			Vector3 vector = WUtoTU(new Vector3(widthWU, widthWU, widthWU));
			Vector3 positionTU2 = default(Vector3);
			for (int i = (int)(positionTU.x - vector.x); i < (int)(positionTU.x + vector.x); i++)
			{
				for (int j = (int)(positionTU.z - vector.z); j < (int)(positionTU.z + vector.z); j++)
				{
					float x = i;
					Vector3 center = m_worldBoundsTU.center;
					positionTU2 = new Vector3(x, center.y, j);
					SetHeightTU(positionTU2, height);
				}
			}
		}
	}
}

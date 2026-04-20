using UnityEngine;

namespace Gaia
{
	public class WaterFlowMap
	{
		public float m_dropletVolume = 0.3f;

		public float m_dropletAbsorbtionRate = 0.05f;

		public int m_waterflowSmoothIterations = 1;

		private UnityHeightMap m_heightMap;

		private HeightMap m_waterFlowMap;

		public void CreateWaterFlowMap(Terrain terrain)
		{
			m_heightMap = new UnityHeightMap(terrain);
			int num = m_heightMap.Width();
			int num2 = m_heightMap.Depth();
			m_waterFlowMap = new HeightMap(num, num2);
			for (int i = 1; i < num - 1; i++)
			{
				for (int j = 1; j < num2 - 1; j++)
				{
					TraceWaterFlow(i, j, num, num2);
				}
			}
			m_waterFlowMap.Flip();
			m_waterFlowMap.Smooth(m_waterflowSmoothIterations);
		}

		private void TraceWaterFlow(int startX, int startZ, int width, int height)
		{
			float num = m_dropletVolume;
			int num2 = startX;
			int num3 = startZ;
			while (num > 0f)
			{
				HeightMap waterFlowMap;
				int x;
				int z;
				(waterFlowMap = m_waterFlowMap)[x = num2, z = num3] = waterFlowMap[x, z] + m_dropletAbsorbtionRate;
				num -= m_dropletAbsorbtionRate;
				float num4;
				float num5 = num4 = m_heightMap[num2, num3];
				int num6 = num2;
				int num7 = num3;
				for (int i = -1; i < 2; i++)
				{
					for (int j = -1; j < 2; j++)
					{
						int num8 = num2 + i;
						int num9 = num3 + j;
						if (num8 >= 0 && num8 < width && num9 >= 0 && num9 < height && m_heightMap[num8, num9] < num5)
						{
							num6 = num8;
							num7 = num9;
							num5 = m_heightMap[num8, num9];
						}
					}
				}
				if (num4 == num5)
				{
					UnityHeightMap heightMap;
					int x2;
					int z2;
					(heightMap = m_heightMap)[x2 = num2, z2 = num3] = heightMap[x2, z2] + m_dropletAbsorbtionRate;
					continue;
				}
				num2 = num6;
				num3 = num7;
			}
		}

		public void ExportWaterMapToPath(string path)
		{
			Utils.CompressToSingleChannelFileImage(m_waterFlowMap.Heights(), path, TextureFormat.RGBA32, exportPNG: true, exportJPG: false);
		}
	}
}

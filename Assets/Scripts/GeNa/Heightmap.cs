using System;

namespace GeNa
{
	public class Heightmap
	{
		protected int m_widthX;

		protected int m_depthZ;

		protected float[,] m_heights;

		protected bool m_isPowerOf2;

		protected float m_widthInvX;

		protected float m_depthInvZ;

		protected float m_statMinVal;

		protected float m_statMaxVal;

		protected double m_statSumVals;

		protected bool m_isDirty;

		protected byte[] m_metaData = new byte[0];

		public float this[int x, int z]
		{
			get
			{
				return m_heights[x, z];
			}
			set
			{
				m_heights[x, z] = value;
				m_isDirty = true;
			}
		}

		public float this[float x, float z]
		{
			get
			{
				return GetInterpolatedHeight(x, z);
			}
			set
			{
				x *= (float)m_widthX;
				z *= (float)m_depthZ;
				m_heights[(int)x, (int)z] = value;
				m_isDirty = true;
			}
		}

		public Heightmap()
		{
			Reset();
		}

		public Heightmap(int width, int depth)
		{
			m_widthX = width;
			m_depthZ = depth;
			m_widthInvX = 1f / (float)m_widthX;
			m_depthInvZ = 1f / (float)m_depthZ;
			m_heights = new float[m_widthX, m_depthZ];
			m_isPowerOf2 = (Math_IsPowerOf2(m_widthX) && Math_IsPowerOf2(m_depthZ));
			m_statMinVal = (m_statMaxVal = 0f);
			m_statSumVals = 0.0;
			m_metaData = new byte[0];
			m_isDirty = false;
		}

		public Heightmap(float[,] source)
		{
			m_widthX = source.GetLength(0);
			m_depthZ = source.GetLength(1);
			m_widthInvX = 1f / (float)m_widthX;
			m_depthInvZ = 1f / (float)m_depthZ;
			m_heights = new float[m_widthX, m_depthZ];
			m_isPowerOf2 = (Math_IsPowerOf2(m_widthX) && Math_IsPowerOf2(m_depthZ));
			m_statMinVal = (m_statMaxVal = 0f);
			m_statSumVals = 0.0;
			m_metaData = new byte[0];
			for (int i = 0; i < m_widthX; i++)
			{
				for (int j = 0; j < m_depthZ; j++)
				{
					m_heights[i, j] = source[i, j];
				}
			}
			m_isDirty = false;
		}

		public Heightmap(float[,,] source, int slice)
		{
			m_widthX = source.GetLength(0);
			m_depthZ = source.GetLength(1);
			m_widthInvX = 1f / (float)m_widthX;
			m_depthInvZ = 1f / (float)m_depthZ;
			m_heights = new float[m_widthX, m_depthZ];
			m_isPowerOf2 = (Math_IsPowerOf2(m_widthX) && Math_IsPowerOf2(m_depthZ));
			m_statMinVal = (m_statMaxVal = 0f);
			m_statSumVals = 0.0;
			m_metaData = new byte[0];
			for (int i = 0; i < m_widthX; i++)
			{
				for (int j = 0; j < m_depthZ; j++)
				{
					m_heights[i, j] = source[i, j, slice];
				}
			}
			m_isDirty = false;
		}

		public Heightmap(int[,] source)
		{
			m_widthX = source.GetLength(0);
			m_depthZ = source.GetLength(1);
			m_widthInvX = 1f / (float)m_widthX;
			m_depthInvZ = 1f / (float)m_depthZ;
			m_heights = new float[m_widthX, m_depthZ];
			m_isPowerOf2 = (Math_IsPowerOf2(m_widthX) && Math_IsPowerOf2(m_depthZ));
			m_statMinVal = (m_statMaxVal = 0f);
			m_statSumVals = 0.0;
			m_metaData = new byte[0];
			for (int i = 0; i < m_widthX; i++)
			{
				for (int j = 0; j < m_depthZ; j++)
				{
					m_heights[i, j] = source[i, j];
				}
			}
			m_isDirty = false;
		}

		public Heightmap(Heightmap source)
		{
			Reset();
			m_widthX = source.m_widthX;
			m_depthZ = source.m_depthZ;
			m_widthInvX = 1f / (float)m_widthX;
			m_depthInvZ = 1f / (float)m_depthZ;
			m_heights = new float[m_widthX, m_depthZ];
			m_isPowerOf2 = source.m_isPowerOf2;
			m_metaData = new byte[source.m_metaData.Length];
			for (int i = 0; i < source.m_metaData.Length; i++)
			{
				m_metaData[i] = source.m_metaData[i];
			}
			for (int j = 0; j < m_widthX; j++)
			{
				for (int k = 0; k < m_depthZ; k++)
				{
					m_heights[j, k] = source.m_heights[j, k];
				}
			}
			m_isDirty = false;
		}

		public int Width()
		{
			return m_widthX;
		}

		public int Depth()
		{
			return m_depthZ;
		}

		public float MinVal()
		{
			return m_statMinVal;
		}

		public float MaxVal()
		{
			return m_statMaxVal;
		}

		public double SumVal()
		{
			return m_statSumVals;
		}

		public byte[] GetMetaData()
		{
			return m_metaData;
		}

		public bool IsDirty()
		{
			return m_isDirty;
		}

		public void SetDirty(bool dirty = true)
		{
			m_isDirty = dirty;
		}

		public void ClearDirty()
		{
			m_isDirty = false;
		}

		public void SetMetaData(byte[] metadata)
		{
			m_metaData = new byte[metadata.Length];
			Buffer.BlockCopy(metadata, 0, m_metaData, 0, metadata.Length);
			m_isDirty = true;
		}

		public float[,] Heights()
		{
			return m_heights;
		}

		public float GetSafeHeight(int x, int z)
		{
			if (x < 0)
			{
				x = 0;
			}
			if (z < 0)
			{
				z = 0;
			}
			if (x >= m_widthX)
			{
				x = m_widthX - 1;
			}
			if (z >= m_depthZ)
			{
				z = m_depthZ - 1;
			}
			return m_heights[x, z];
		}

		public void SetSafeHeight(int x, int z, float height)
		{
			if (x < 0)
			{
				x = 0;
			}
			if (z < 0)
			{
				z = 0;
			}
			if (x >= m_widthX)
			{
				x = m_widthX - 1;
			}
			if (z >= m_depthZ)
			{
				z = m_depthZ - 1;
			}
			m_heights[x, z] = height;
			m_isDirty = true;
		}

		protected float GetInterpolatedHeight(float x, float z)
		{
			x *= (float)m_widthX - 1f;
			z *= (float)m_depthZ - 1f;
			int num = (int)x;
			int num2 = (int)z;
			int num3 = num + 1;
			int num4 = num2 + 1;
			if (num3 >= m_widthX)
			{
				num3 = num;
			}
			if (num4 >= m_depthZ)
			{
				num4 = num2;
			}
			float num5 = x - (float)num;
			float num6 = z - (float)num2;
			float num7 = 1f - num5;
			float num8 = 1f - num6;
			return num7 * num8 * m_heights[num, num2] + num7 * num6 * m_heights[num, num4] + num5 * num8 * m_heights[num3, num2] + num5 * num6 * m_heights[num3, num4];
		}

		public void SetHeight(float height)
		{
			float num = Math_Clamp(0f, 1f, height);
			for (int i = 0; i < m_widthX; i++)
			{
				for (int j = 0; j < m_depthZ; j++)
				{
					m_heights[i, j] = num;
				}
			}
			m_isDirty = true;
		}

		public void GetHeightRange(ref float minHeight, ref float maxHeight)
		{
			maxHeight = float.MinValue;
			minHeight = float.MaxValue;
			for (int i = 0; i < m_widthX; i++)
			{
				for (int j = 0; j < m_depthZ; j++)
				{
					float num = m_heights[i, j];
					if (num > maxHeight)
					{
						maxHeight = num;
					}
					if (num < minHeight)
					{
						minHeight = num;
					}
				}
			}
		}

		public float GetSlope(int x, int z)
		{
			float num = m_heights[x, z];
			float num2 = m_heights[x + 1, z] - num;
			float num3 = m_heights[x, z + 1] - num;
			return (float)Math.Sqrt(num2 * num2 + num3 * num3);
		}

		public float GetSlope(float x, float z)
		{
			float num = GetInterpolatedHeight(x + m_widthInvX * 0.9f, z) - GetInterpolatedHeight(x - m_widthInvX * 0.9f, z);
			float num2 = GetInterpolatedHeight(x, z + m_depthInvZ * 0.9f) - GetInterpolatedHeight(x, z - m_depthInvZ * 0.9f);
			return Math_Clamp(0f, 90f, (float)(Math.Sqrt(num * num + num2 * num2) * 10000.0));
		}

		public float GetSlope_a(float x, float z)
		{
			float interpolatedHeight = GetInterpolatedHeight(x, z);
			float num = Math.Abs(GetInterpolatedHeight(x - m_widthInvX, z) - interpolatedHeight);
			float num2 = Math.Abs(GetInterpolatedHeight(x + m_widthInvX, z) - interpolatedHeight);
			float num3 = Math.Abs(GetInterpolatedHeight(x, z - m_depthInvZ) - interpolatedHeight);
			float num4 = Math.Abs(GetInterpolatedHeight(x, z + m_depthInvZ) - interpolatedHeight);
			return (num + num2 + num3 + num4) / 4f * 400f;
		}

		public float GetBaseLevel()
		{
			float num = 0f;
			for (int i = 0; i < m_widthX; i++)
			{
				if (m_heights[i, 0] > num)
				{
					num = m_heights[i, 0];
				}
				if (m_heights[i, m_depthZ - 1] > num)
				{
					num = m_heights[i, m_depthZ - 1];
				}
			}
			for (int j = 0; j < m_depthZ; j++)
			{
				if (m_heights[0, j] > num)
				{
					num = m_heights[0, j];
				}
				if (m_heights[m_widthX - 1, j] > num)
				{
					num = m_heights[m_widthX - 1, j];
				}
			}
			return num;
		}

		public bool HasData()
		{
			if (m_widthX <= 0 || m_depthZ <= 0)
			{
				return false;
			}
			if (m_heights == null)
			{
				return false;
			}
			if (m_heights.GetLength(0) != m_widthX || m_heights.GetLength(1) != m_depthZ)
			{
				return false;
			}
			return true;
		}

		public void Reset()
		{
			m_widthX = (m_depthZ = 0);
			m_widthInvX = (m_depthInvZ = 0f);
			m_heights = null;
			m_statMinVal = (m_statMaxVal = 0f);
			m_statSumVals = 0.0;
			m_metaData = new byte[0];
			m_isDirty = false;
		}

		public void UpdateStats()
		{
			m_statMinVal = 1f;
			m_statMaxVal = 0f;
			m_statSumVals = 0.0;
			float num = 0f;
			for (int i = 0; i < m_widthX; i++)
			{
				for (int j = 0; j < m_depthZ; j++)
				{
					num = m_heights[i, j];
					if (num < m_statMinVal)
					{
						m_statMinVal = num;
					}
					if (num > m_statMaxVal)
					{
						m_statMaxVal = num;
					}
					m_statSumVals += num;
				}
			}
		}

		public void Smooth(int iterations)
		{
			for (int i = 0; i < iterations; i++)
			{
				for (int j = 0; j < m_widthX; j++)
				{
					for (int k = 0; k < m_depthZ; k++)
					{
						m_heights[j, k] = Math_Clamp(0f, 1f, (GetSafeHeight(j - 1, k) + GetSafeHeight(j + 1, k) + GetSafeHeight(j, k - 1) + GetSafeHeight(j, k + 1)) / 4f);
					}
				}
			}
			m_isDirty = true;
		}

		public Heightmap GetSlopeMap()
		{
			Heightmap heightmap = new Heightmap(this);
			for (int i = 0; i < m_widthX; i++)
			{
				for (int j = 0; j < m_depthZ; j++)
				{
					heightmap[i, j] = GetSlope(i, j);
				}
			}
			return heightmap;
		}

		public void Invert()
		{
			for (int i = 0; i < m_widthX; i++)
			{
				for (int j = 0; j < m_depthZ; j++)
				{
					m_heights[i, j] = 1f - m_heights[i, j];
				}
			}
			m_isDirty = true;
		}

		public void Flip()
		{
			float[,] array = new float[m_depthZ, m_widthX];
			for (int i = 0; i < m_widthX; i++)
			{
				for (int j = 0; j < m_depthZ; j++)
				{
					array[j, i] = m_heights[i, j];
				}
			}
			m_heights = array;
			m_widthX = array.GetLength(0);
			m_depthZ = array.GetLength(1);
			m_widthInvX = 1f / (float)m_widthX;
			m_depthInvZ = 1f / (float)m_depthZ;
			m_isPowerOf2 = (Math_IsPowerOf2(m_widthX) && Math_IsPowerOf2(m_depthZ));
			m_statMinVal = (m_statMaxVal = 0f);
			m_statSumVals = 0.0;
			m_isDirty = true;
		}

		public void Normalise()
		{
			float num = float.MinValue;
			float num2 = float.MaxValue;
			for (int i = 0; i < m_widthX; i++)
			{
				for (int j = 0; j < m_depthZ; j++)
				{
					float num3 = m_heights[i, j];
					if (num3 > num)
					{
						num = num3;
					}
					if (num3 < num2)
					{
						num2 = num3;
					}
				}
			}
			float num4 = num - num2;
			if (!(num4 > 0f))
			{
				return;
			}
			for (int k = 0; k < m_widthX; k++)
			{
				for (int l = 0; l < m_depthZ; l++)
				{
					m_heights[k, l] = (m_heights[k, l] - num2) / num4;
				}
			}
			m_isDirty = true;
		}

		public static bool Math_IsPowerOf2(int value)
		{
			return (value & (value - 1)) == 0;
		}

		public static float Math_Clamp(float min, float max, float value)
		{
			if (value < min)
			{
				return min;
			}
			if (value > max)
			{
				return max;
			}
			return value;
		}
	}
}

using UnityEngine;

namespace Gaia
{
	public class FractalGenerator
	{
		public enum Fractals
		{
			Perlin,
			Billow,
			RidgeMulti
		}

		private delegate float GetCalcValue(float x, float z);

		private float m_seed;

		private int m_octaves = 8;

		private float m_persistence = 0.65f;

		private float m_frequency = 1f;

		private float m_lacunarity = 1.5f;

		private float m_XOffset;

		private float m_ZOffset;

		private float m_YOffset;

		private Fractals m_fractalType;

		private float[] m_spectralWeights = new float[20];

		private GetCalcValue m_noiseCalculator;

		public float Seed
		{
			get
			{
				return m_seed;
			}
			set
			{
				m_seed = value;
			}
		}

		public int Octaves
		{
			get
			{
				return m_octaves;
			}
			set
			{
				m_octaves = value;
			}
		}

		public float Persistence
		{
			get
			{
				return m_persistence;
			}
			set
			{
				m_persistence = value;
			}
		}

		public float Frequency
		{
			get
			{
				return m_frequency;
			}
			set
			{
				m_frequency = value;
			}
		}

		public float Lacunarity
		{
			get
			{
				return m_lacunarity;
			}
			set
			{
				m_lacunarity = value;
			}
		}

		public float XOffset
		{
			get
			{
				return m_XOffset;
			}
			set
			{
				m_XOffset = value;
			}
		}

		public float ZOffset
		{
			get
			{
				return m_ZOffset;
			}
			set
			{
				m_ZOffset = value;
			}
		}

		public float YOffset
		{
			get
			{
				return m_YOffset;
			}
			set
			{
				m_YOffset = value;
			}
		}

		public Fractals FractalType
		{
			get
			{
				return m_fractalType;
			}
			set
			{
				m_fractalType = value;
				switch (m_fractalType)
				{
				case Fractals.Perlin:
					m_noiseCalculator = GetValue_Perlin;
					break;
				case Fractals.Billow:
					m_noiseCalculator = GetValue_Billow;
					break;
				case Fractals.RidgeMulti:
					CalcSpectralWeights();
					m_noiseCalculator = GetValue_RidgedMulti;
					break;
				}
			}
		}

		public FractalGenerator()
		{
			FractalType = Fractals.Perlin;
		}

		public FractalGenerator(float frequency, float lacunarity, int octaves, float persistance, float seed, Fractals type)
		{
			m_frequency = frequency;
			m_lacunarity = lacunarity;
			m_octaves = octaves;
			m_persistence = persistance;
			m_seed = seed;
			switch (type)
			{
			case Fractals.Perlin:
				m_noiseCalculator = GetValue_Perlin;
				break;
			case Fractals.Billow:
				m_noiseCalculator = GetValue_Billow;
				break;
			case Fractals.RidgeMulti:
				CalcSpectralWeights();
				m_noiseCalculator = GetValue_RidgedMulti;
				break;
			default:
				m_noiseCalculator = GetValue_Perlin;
				break;
			}
		}

		public void SetDefaults()
		{
			switch (m_fractalType)
			{
			case Fractals.Perlin:
				m_frequency = 1f;
				m_lacunarity = 2f;
				m_octaves = 6;
				m_persistence = 0.5f;
				m_seed = 0f;
				m_noiseCalculator = GetValue_Perlin;
				break;
			case Fractals.Billow:
				m_frequency = 1f;
				m_lacunarity = 2f;
				m_octaves = 6;
				m_persistence = 0.5f;
				m_seed = 0f;
				m_noiseCalculator = GetValue_Billow;
				break;
			case Fractals.RidgeMulti:
				m_frequency = 1f;
				m_lacunarity = 2f;
				m_octaves = 6;
				m_seed = 0f;
				CalcSpectralWeights();
				m_noiseCalculator = GetValue_RidgedMulti;
				break;
			}
		}

		public float GetValue(float x, float z)
		{
			return m_noiseCalculator(x, z);
		}

		public double GetValue(double x, double z)
		{
			return GetValue((float)x, (float)z);
		}

		public float GetNormalisedValue(float x, float z)
		{
			return Mathf.Clamp01((GetValue(x, z) + 1f) / 2f);
		}

		public double GetNormalisedValue(double x, double z)
		{
			return GetNormalisedValue((float)x, (float)z);
		}

		public float GetValue_Perlin(float x, float z)
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = 1f;
			x += m_seed;
			z += m_seed;
			x += m_XOffset;
			z += m_ZOffset;
			x *= m_frequency;
			z *= m_frequency;
			for (int i = 0; i < m_octaves; i++)
			{
				float x2 = x;
				float y = z;
				num2 = SimplexNoiseGenerator.Generate(x2, y);
				num += num2 * num3;
				x *= m_lacunarity;
				z *= m_lacunarity;
				num3 *= m_persistence;
			}
			return num + m_YOffset * 2.4f;
		}

		public float GetValue_Billow(float x, float z)
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = 1f;
			x += m_seed;
			z += m_seed;
			x += m_XOffset;
			z += m_ZOffset;
			x *= m_frequency;
			z *= m_frequency;
			for (int i = 0; i < m_octaves; i++)
			{
				float x2 = x;
				float y = z;
				num2 = SimplexNoiseGenerator.Generate(x2, y);
				num2 = 2f * Mathf.Abs(num2) - 1f;
				num += num2 * num3;
				x *= m_lacunarity;
				z *= m_lacunarity;
				num3 *= m_persistence;
			}
			return num + m_YOffset * 2.4f;
		}

		public float GetValue_RidgedMulti(float x, float z)
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = 1f;
			float num4 = 1f;
			float persistence = m_persistence;
			x += m_seed;
			z += m_seed;
			x += m_XOffset;
			z += m_ZOffset;
			x *= m_frequency;
			z *= m_frequency;
			for (int i = 0; i < m_octaves; i++)
			{
				float x2 = x;
				float y = z;
				num = SimplexNoiseGenerator.Generate(x2, y);
				num = Mathf.Abs(num);
				num = num4 - num;
				num *= num;
				num *= num3;
				num3 = num * persistence;
				if ((double)num3 > 1.0)
				{
					num3 = 1f;
				}
				if (num3 < 0f)
				{
					num3 = 0f;
				}
				num2 += num * m_spectralWeights[i];
				x *= m_lacunarity;
				z *= m_lacunarity;
			}
			num2 = num2 * 1.25f - 1f;
			return num2 + m_YOffset;
		}

		private void CalcSpectralWeights()
		{
			float num = 1f;
			float num2 = 1f;
			int length = m_spectralWeights.GetLength(0);
			for (int i = 0; i < length; i++)
			{
				m_spectralWeights[i] = Mathf.Pow(num2, 0f - num);
				num2 *= m_lacunarity;
			}
		}
	}
}

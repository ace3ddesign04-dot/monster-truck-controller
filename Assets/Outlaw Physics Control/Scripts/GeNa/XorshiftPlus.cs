using UnityEngine;

namespace GeNa
{
	public class XorshiftPlus
	{
		private const ulong m_A_Init = 181353uL;

		private const ulong m_B_Init = 7uL;

		public int m_seed;

		public ulong m_stateA;

		public ulong m_stateB;

		public XorshiftPlus(int seed = 1)
		{
			m_seed = seed;
			if (m_seed == 0)
			{
				m_seed = 1;
			}
			Reset();
		}

		public void Reset()
		{
			m_stateA = (ulong)(181353L * (long)(uint)m_seed);
			m_stateB = (ulong)(7L * (long)(uint)m_seed);
		}

		public void Reset(int seed)
		{
			m_seed = seed;
			if (m_seed == 0)
			{
				m_seed = 1;
			}
			Reset();
		}

		public void Reset(ulong stateA, ulong stateB)
		{
			UnityEngine.Debug.Log("Resetting RNG State " + stateA + " " + stateB);
			m_stateA = stateA;
			m_stateB = stateB;
		}

		public void GetState(out int seed, out ulong stateA, out ulong stateB)
		{
			seed = m_seed;
			stateA = m_stateA;
			stateB = m_stateB;
		}

		public float Next()
		{
			ulong stateA = m_stateA;
			ulong num = m_stateA = m_stateB;
			stateA ^= stateA << 23;
			stateA ^= stateA >> 17;
			return (float)(double)((m_stateB = (stateA ^ (num ^ (num >> 26)))) + num) / 1.84467441E+19f;
		}

		public int NextInt()
		{
			return (int)(Next() * 2.14748365E+09f);
		}

		public float Next(float min, float max)
		{
			return min + Next() * (max - min);
		}

		public int Next(int min, int max)
		{
			if (min == max)
			{
				return min;
			}
			return (int)Next(min, (float)max + 0.999f);
		}

		public Vector3 NextVector()
		{
			return new Vector3(Next(), Next(), Next());
		}

		public Vector3 NextVector(float min, float max)
		{
			return new Vector3(Next(min, max), Next(min, max), Next(min, max));
		}
	}
}

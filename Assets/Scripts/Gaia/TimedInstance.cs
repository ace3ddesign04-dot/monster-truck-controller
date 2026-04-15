using System.Diagnostics;

namespace Gaia
{
	public class TimedInstance : Stopwatch
	{
		public long nanosecPerTick = 1000000000 / Stopwatch.Frequency;

		public string m_name;

		public int m_iterations;

		public TimedInstance(string name, bool start = true)
		{
			m_name = name;
			if (start)
			{
				Start();
			}
		}

		public new void Start()
		{
			m_iterations++;
			base.Start();
		}

		public new void Reset()
		{
			m_iterations = 0;
			base.Reset();
		}

		public void IncIterations()
		{
			m_iterations++;
		}

		public float GetAvgMs()
		{
			return (float)base.ElapsedMilliseconds / (float)m_iterations;
		}

		public float GetAvgS()
		{
			return (float)base.ElapsedMilliseconds / (float)m_iterations / 1000f;
		}

		public override string ToString()
		{
			return string.Format("{0}: Avg: {1:0.000}s Last: {1:0.000}s", m_name, GetAvgMs() / 1000f, (float)base.ElapsedMilliseconds / 1000f);
		}
	}
}

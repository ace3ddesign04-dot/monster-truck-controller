using UnityEngine;

namespace Gaia
{
	public class PreviewTextureAttribute : PropertyAttribute
	{
		public Rect m_lastPosition = new Rect(0f, 0f, 0f, 0f);

		public long m_expire = 6000000000L;

		public WWW m_www;

		public Texture2D m_cached;

		public float m_width = 1f;

		public float m_offset;

		public PreviewTextureAttribute()
		{
		}

		public PreviewTextureAttribute(int expire)
		{
			m_expire = expire * 1000 * 10000;
		}

		public PreviewTextureAttribute(float offset, float width)
		{
			m_offset = offset;
			m_width = width;
		}
	}
}

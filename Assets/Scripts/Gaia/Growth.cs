using System.Collections;
using UnityEngine;

namespace Gaia
{
	public class Growth : MonoBehaviour
	{
		[Range(0.1f, 2f)]
		[Tooltip("The start size in the game.")]
		public float m_startScale = 0.15f;

		[Range(0.1f, 2f)]
		[Tooltip("The end size in the game.")]
		public float m_endScale = 1f;

		[Range(0f, 2f)]
		[Tooltip("Scale variance. Final scale is equal to end scale plus a a random value between 0 and this.")]
		public float m_scaleVariance = 0.25f;

		[Range(0.5f, 60f)]
		[Tooltip("The time it takes to grow in seconds.")]
		public float m_growthTime = 5f;

		private float m_actualEndScale;

		private void Start()
		{
			Initialise();
		}

		public virtual void Initialise()
		{
			m_actualEndScale = m_endScale + UnityEngine.Random.Range(0f, m_scaleVariance);
			StartCoroutine(Grow());
		}

		private IEnumerator Grow()
		{
			float startTime = Time.realtimeSinceStartup;
			float currentTime = startTime;
			float deltaScale = m_actualEndScale - m_startScale;
			float finishTime = startTime + m_growthTime;
			while (currentTime < finishTime)
			{
				float scale2 = 1f - (finishTime - currentTime) / m_growthTime;
				scale2 = m_startScale + scale2 * deltaScale;
				base.gameObject.transform.localScale = Vector3.one * scale2;
				yield return null;
				currentTime = Time.realtimeSinceStartup;
			}
		}

		public virtual void Die()
		{
			UnityEngine.Object.Destroy(base.gameObject, 5f);
		}
	}
}

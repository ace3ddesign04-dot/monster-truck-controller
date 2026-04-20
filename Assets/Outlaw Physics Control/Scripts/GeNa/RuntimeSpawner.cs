using System.Collections;
using UnityEngine;

namespace GeNa
{
	public class RuntimeSpawner : MonoBehaviour
	{
		[Tooltip("The amount of time in seconds that the spawner will run a spawn iteration.")]
		public float m_spawnInterval = 10f;

		[Tooltip("The spawner that will run the spawn iteration.")]
		public Spawner m_spawner;

		[Tooltip("Update the spawner settings on every spawn iteration, otherwise just use the original cxriteria / settings ant apply them at the current location.")]
		public bool m_updateSpawnerSettings = true;

		[Tooltip("Show debug messages when it runs.")]
		public bool m_showDebug;

		private void Start()
		{
			StartCoroutine(RunSpawnerIteration(m_spawnInterval));
		}

		private IEnumerator RunSpawnerIteration(float waitTime)
		{
			while (true)
			{
				yield return new WaitForSeconds(waitTime);
				if (m_spawner != null)
				{
					if (m_showDebug)
					{
						UnityEngine.Debug.Log("Running spawner iteration");
					}
					if (m_updateSpawnerSettings)
					{
						Ray ray = new Ray(base.transform.position, Vector3.down);
						if (Physics.Raycast(ray, out RaycastHit hitInfo, 10000f))
						{
							m_spawner.SetSpawnOriginAndUpdateRanges(hitInfo.transform, hitInfo.point, hitInfo.normal);
							m_spawner.Spawn(hitInfo.point, subSpawn: false);
						}
						else
						{
							m_spawner.SetSpawnOriginAndUpdateRanges(null, base.transform.position, Vector3.up);
							m_spawner.Spawn(base.transform.position, subSpawn: false);
						}
					}
					else
					{
						m_spawner.Spawn(base.transform.position, subSpawn: false);
					}
				}
				else if (m_showDebug)
				{
					UnityEngine.Debug.Log("Need a spawner in order to do the spawn!");
				}
			}
		}
	}
}

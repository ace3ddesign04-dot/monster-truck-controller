using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gaia
{
	[ExecuteInEditMode]
	public class SpawnerGroup : MonoBehaviour
	{
		[Serializable]
		public class SpawnerInstance
		{
			public string m_name;

			public Spawner m_spawner;

			public int m_interationsPerSpawn = 1;
		}

		public List<SpawnerInstance> m_spawners = new List<SpawnerInstance>();

		[HideInInspector]
		public List<SpawnerGroup> m_spawnerGroups = new List<SpawnerGroup>();

		public IEnumerator m_updateCoroutine;

		private bool m_cancelSpawn;

		[HideInInspector]
		public int m_progress;

		private void OnEnable()
		{
		}

		private void OnDisable()
		{
		}

		public void StartEditorUpdates()
		{
		}

		public void StopEditorUpdates()
		{
		}

		private void EditorUpdate()
		{
			if (m_updateCoroutine == null)
			{
				StopEditorUpdates();
			}
			else
			{
				m_updateCoroutine.MoveNext();
			}
		}

		public void RunSpawnerIteration()
		{
			m_cancelSpawn = false;
			m_progress = 0;
			StartCoroutine(RunSpawnerIterationCoRoutine());
		}

		public IEnumerator RunSpawnerIterationCoRoutine()
		{
			for (int idx = 0; idx < m_spawners.Count; idx++)
			{
				SpawnerInstance si = m_spawners[idx];
				if (si == null || !(si.m_spawner != null))
				{
					continue;
				}
				for (int iter = 0; iter < si.m_interationsPerSpawn; iter++)
				{
					if (!m_cancelSpawn)
					{
						si.m_spawner.RunSpawnerIteration();
						yield return new WaitForSeconds(0.2f);
						while (!si.m_spawner.m_spawnComplete)
						{
							m_progress++;
							yield return new WaitForSeconds(0.5f);
						}
						m_progress++;
					}
				}
			}
			m_progress = 0;
			m_updateCoroutine = null;
		}

		public void CancelSpawn()
		{
			m_cancelSpawn = true;
			for (int i = 0; i < m_spawners.Count; i++)
			{
				m_spawners[i].m_spawner.CancelSpawn();
			}
			for (int j = 0; j < m_spawnerGroups.Count; j++)
			{
				m_spawnerGroups[j].CancelSpawn();
			}
		}

		public bool FixNames()
		{
			bool result = false;
			for (int i = 0; i < m_spawners.Count; i++)
			{
				SpawnerInstance spawnerInstance = m_spawners[i];
				if (spawnerInstance != null && spawnerInstance.m_spawner != null && spawnerInstance.m_name != spawnerInstance.m_spawner.name)
				{
					spawnerInstance.m_name = spawnerInstance.m_spawner.name;
					result = true;
				}
			}
			return result;
		}

		public void ResetSpawner()
		{
			for (int i = 0; i < m_spawners.Count; i++)
			{
				SpawnerInstance spawnerInstance = m_spawners[i];
				if (spawnerInstance != null && spawnerInstance.m_spawner != null)
				{
					spawnerInstance.m_spawner.ResetSpawner();
				}
			}
			for (int j = 0; j < m_spawnerGroups.Count; j++)
			{
				SpawnerGroup spawnerGroup = m_spawnerGroups[j];
				if (spawnerGroup != null)
				{
					spawnerGroup.ResetSpawner();
				}
			}
		}
	}
}

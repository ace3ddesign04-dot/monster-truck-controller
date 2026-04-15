using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gaia
{
	[Serializable]
	public class ResourceProtoTree
	{
		[Tooltip("Resource name.")]
		public string m_name;

		[Tooltip("Desktop prefab.")]
		public GameObject m_desktopPrefab;

		[HideInInspector]
		public string m_desktopPrefabFileName;

		[Tooltip("Mobile prefab - future proofing here - not currently used.")]
		public GameObject m_mobilePrefab;

		[HideInInspector]
		public string m_mobilePrefabFileName;

		[Tooltip("How much the tree bends in the wind - only used by unity tree creator trees, ignored by SpeedTree trees.")]
		public float m_bendFactor;

		[Tooltip("The colour of healthy trees - only used by unity tree creator trees, ignored by SpeedTree trees.")]
		public Color m_healthyColour = Color.white;

		[Tooltip("The colour of dry trees - only used by unity tree creator trees, ignored by SpeedTree trees.")]
		public Color m_dryColour = Color.white;

		[Tooltip("DNA - Used by the spawner to control how and where the tree will be spawned.")]
		public ResourceProtoDNA m_dna;

		[Tooltip("SPAWN CRITERIA - Spawn criteria are run against the terrain to assess its fitness in a range of 0..1 for use by this resource. If you add multiple criteria then the fittest one will be selected.")]
		public SpawnCritera[] m_spawnCriteria = new SpawnCritera[0];

		[Tooltip("SPAWN EXTENSIONS - Spawn extensions allow fitness, spawning and post spawning extensions to be made to the spawning system.")]
		public SpawnRuleExtension[] m_spawnExtensions = new SpawnRuleExtension[0];

		public void Initialise(Spawner spawner)
		{
			SpawnCritera[] spawnCriteria = m_spawnCriteria;
			foreach (SpawnCritera spawnCritera in spawnCriteria)
			{
				spawnCritera.Initialise(spawner);
			}
		}

		public bool HasActiveCriteria()
		{
			for (int i = 0; i < m_spawnCriteria.Length; i++)
			{
				if (m_spawnCriteria[i].m_isActive)
				{
					return true;
				}
			}
			return false;
		}

		public bool SetAssetAssociations()
		{
			return false;
		}

		public bool AssociateAssets()
		{
			return false;
		}

		public bool ChecksTextures()
		{
			for (int i = 0; i < m_spawnCriteria.Length; i++)
			{
				if (m_spawnCriteria[i].m_isActive && m_spawnCriteria[i].m_checkTexture)
				{
					return true;
				}
			}
			return false;
		}

		public bool ChecksProximity()
		{
			for (int i = 0; i < m_spawnCriteria.Length; i++)
			{
				if (m_spawnCriteria[i].m_isActive && m_spawnCriteria[i].m_checkProximity)
				{
					return true;
				}
			}
			return false;
		}

		public void AddTags(ref List<string> tagList)
		{
			for (int i = 0; i < m_spawnCriteria.Length; i++)
			{
				if (m_spawnCriteria[i].m_isActive && m_spawnCriteria[i].m_checkProximity && !tagList.Contains(m_spawnCriteria[i].m_proximityTag))
				{
					tagList.Add(m_spawnCriteria[i].m_proximityTag);
				}
			}
		}
	}
}

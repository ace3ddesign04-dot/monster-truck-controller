using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gaia
{
	[Serializable]
	public class ResourceProtoDetail
	{
		[Tooltip("Resource name.")]
		public string m_name;

		[Tooltip("Render mode.")]
		public DetailRenderMode m_renderMode;

		[Tooltip("Detail prototype - used by vertex lit render mode.")]
		public GameObject m_detailProtoype;

		[HideInInspector]
		public string m_detailPrototypeFileName;

		[Tooltip("The texture that represents the grass and used by grass and billboard grass render mode.")]
		public Texture2D m_detailTexture;

		[HideInInspector]
		public string m_detailTextureFileName;

		[Tooltip("Minimum width. Lower limit of the width of the clumps of grass that are generated.")]
		public float m_minWidth;

		[Tooltip("Maximum width. Upper limit of the width of the clumps of grass that are generated.")]
		public float m_maxWidth;

		[Tooltip("Minimum height. Lower limit of the height of the clumps of grass that are generated.")]
		public float m_minHeight;

		[Tooltip("Maximum height. Upper limit of the height of the clumps of grass that are generated.")]
		public float m_maxHeight;

		[Tooltip("Controls the approximate size of the alternating patches, with higher values indicating more variation within a given area.")]
		[Range(0f, 1f)]
		public float m_noiseSpread = 0.3f;

		[Tooltip("Controls the degree to which the grass will bend based on terrain settings.")]
		[Range(0f, 5f)]
		public float m_bendFactor;

		[Tooltip("Healthy grass clump colour.")]
		public Color m_healthyColour = Color.white;

		[Tooltip("Dry grass clump colour.")]
		public Color m_dryColour = Color.white;

		[Tooltip("DNA - Used by the spawner to control how and where the grass will be spawned.")]
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

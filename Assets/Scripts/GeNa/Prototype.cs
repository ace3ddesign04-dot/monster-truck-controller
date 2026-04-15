using System;
using System.Collections.Generic;
using UnityEngine;

namespace GeNa
{
	[Serializable]
	public class Prototype
	{
		public string m_name;

		public bool m_active = true;

		public Vector3 m_size = Vector3.one;

		public Vector3 m_extents = Vector3.one;

		public float m_boundsBorder;

		public bool m_constrainWithinMaskedBounds;

		public bool m_invertMaskedAlpha;

		public bool m_scaleOnMaskedAlpha;

		public float m_scaleOnMaskedAlphaMin = 0.5f;

		public float m_scaleOnMaskedAlphaMax = 1f;

		public bool m_successOnMaskedAlpha;

		public float m_forwardRotation;

		public List<Resource> m_resources = new List<Resource>();

		public Constants.ResourceType m_resourceType;

		public bool m_hasColliders;

		public bool m_hasMeshes;

		public bool m_hasRigidBody;

		public long m_instancesSpawned;

		public Color m_imageFilterColour = Color.white;

		public float m_imageFilterFuzzyMatch = 0.8f;

		public bool m_displayedInEditor;

		public Prototype()
		{
		}

		public Prototype(Prototype src)
		{
			m_name = src.m_name;
			m_active = src.m_active;
			m_size = src.m_size;
			m_extents = src.m_extents;
			m_boundsBorder = src.m_boundsBorder;
			m_constrainWithinMaskedBounds = src.m_constrainWithinMaskedBounds;
			m_invertMaskedAlpha = src.m_invertMaskedAlpha;
			m_scaleOnMaskedAlpha = src.m_scaleOnMaskedAlpha;
			m_scaleOnMaskedAlphaMin = src.m_scaleOnMaskedAlphaMin;
			m_scaleOnMaskedAlphaMax = src.m_scaleOnMaskedAlphaMax;
			m_successOnMaskedAlpha = src.m_successOnMaskedAlpha;
			m_forwardRotation = src.m_forwardRotation;
			m_resources = new List<Resource>();
			foreach (Resource resource in src.m_resources)
			{
				Resource item = new Resource(resource);
				m_resources.Add(item);
			}
			m_resourceType = src.m_resourceType;
			m_hasColliders = src.m_hasColliders;
			m_hasMeshes = src.m_hasMeshes;
			m_hasRigidBody = src.m_hasRigidBody;
			m_displayedInEditor = src.m_displayedInEditor;
			m_instancesSpawned = src.m_instancesSpawned;
			m_imageFilterColour = src.m_imageFilterColour;
			m_imageFilterFuzzyMatch = src.m_imageFilterFuzzyMatch;
		}

		public float GetSuccessChance()
		{
			if (!m_active)
			{
				return 0f;
			}
			float num = 0f;
			foreach (Resource resource in m_resources)
			{
				if (resource.m_successRate > num)
				{
					num = resource.m_successRate;
				}
			}
			return num;
		}
	}
}

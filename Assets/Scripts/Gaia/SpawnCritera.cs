using System;
using UnityEngine;

namespace Gaia
{
	[Serializable]
	public class SpawnCritera
	{
		[Tooltip("Criteria name")]
		public string m_name;

		[Tooltip("CHECK TYPE - a single point on the terrain or area based. The size of the area checks is based on the Bounds Radius in the DNA. Area based checks are good for larger structures but substantially slower so use with care.")]
		public GaiaConstants.SpawnerLocationCheckType m_checkType;

		[Tooltip("When selected, the criteria will only be valid if the terrain was clear of any other objects at this location. A location is determined to be ‘virgin’ when raycast collision test hits clear terrain. To detect other objects at this location they must have colliders. You can use an invisible collider and this test to stop any resources that require virgin terrain from spawning at that location.")]
		public bool m_virginTerrain = true;

		[Tooltip("Whether or not this location will be checked for height.")]
		public bool m_checkHeight = true;

		[Tooltip("The minimum valid height relative to sea level. Only tested when Check Height is checked.")]
		public float m_minHeight;

		[Tooltip("The maximum valid height relative to sea level. Only tested when Check Height is checked.")]
		public float m_maxHeight = 1000f;

		[Tooltip("The fitness curve - evaluated between the minimum and the maximum height when Check Height is checked. 0 is low fitness and unlikely to spawn, 1 is high fitness and likely to spawn.")]
		public AnimationCurve m_heightFitness = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 0f));

		[Tooltip("Whether or not this location will be checked for slope.")]
		public bool m_checkSlope = true;

		[Tooltip("The minimum valid slope. Only tested when Check Slope is checked.")]
		public float m_minSlope;

		[Tooltip("The maximum valid slope. Only tested when Check Slope is checked.")]
		public float m_maxSlope = 90f;

		[Tooltip("The fitness curve - evaluated between the minimum and the maximum slope when Check Slope is checked. 0 is low fitness and unlikely to spawn, 1 is high fitness and likely to spawn.")]
		public AnimationCurve m_slopeFitness = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 0f));

		[Tooltip("Whether or not to check proximity to other tags near this location.")]
		public bool m_checkProximity;

		[Tooltip("The tag that will be checked in proximity to this location e.g. House")]
		public string m_proximityTag = string.Empty;

		[Tooltip("The minimum valid proximity. Only tested when Check Proximity is checked.")]
		public float m_minProximity;

		[Tooltip("The maximum valid proximity. Only tested when Check Proximity is checked.")]
		public float m_maxProximity = 100f;

		[Tooltip("The fitness curve - evaluated between the minimum and the maximum proximity when Check Proximity is checked. 0 is low fitness and unlikely to spawn, 1 is high fitness and likely to spawn.")]
		public AnimationCurve m_proximityFitness = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(0.3f, 0.2f), new Keyframe(1f, 0f));

		[Tooltip("Check textures at this location.")]
		public bool m_checkTexture;

		[Tooltip("Texture slots from your terrain (first valid slot is 0). Will select for presence that texture. Use exclamation mark in front of slot to select for absence of that texture. For example 3 selects for presence of texture 3, !3 checks for absence of texture 3. Fitness is based on the strength of the texture at that location in range 0..1. Only tested when Check Texture is checked.")]
		public string m_matchingTextures = "!3";

		[Tooltip("Whether or not this spawn criteria is active. It will be ignored if not active.")]
		public bool m_isActive = true;

		private bool m_isInitialised;

		public void Initialise(Spawner spawner)
		{
			string text = string.Empty;
			char c = 4.ToString()[0];
			if (Terrain.activeTerrain != null)
			{
				c = Terrain.activeTerrain.terrainData.alphamapLayers.ToString()[0];
			}
			for (int i = 0; i < m_matchingTextures.Length; i++)
			{
				if ((m_matchingTextures[i] >= '0' && m_matchingTextures[i] <= c) || m_matchingTextures[i] == '!')
				{
					text += m_matchingTextures[i];
				}
			}
			m_matchingTextures = text;
			m_isInitialised = true;
		}

		public float GetSlopeFitness(float slope)
		{
			float result = 1f;
			if (m_checkSlope)
			{
				if (slope < m_minSlope || slope > m_maxSlope)
				{
					return 0f;
				}
				result = m_maxSlope - m_minSlope;
				if (result > 0f)
				{
					result = (slope - m_minSlope) / result;
					result = m_slopeFitness.Evaluate(result);
				}
				else
				{
					result = 0f;
				}
			}
			return result;
		}

		public float GetHeightFitness(float height, float sealLevel)
		{
			float result = 1f;
			if (m_checkHeight)
			{
				height -= sealLevel;
				if (height < m_minHeight || height > m_maxHeight)
				{
					return 0f;
				}
				result = m_maxHeight - m_minHeight;
				if (result > 0f)
				{
					result = (height - m_minHeight) / result;
					result = m_heightFitness.Evaluate(result);
				}
				else
				{
					result = 0f;
				}
			}
			return result;
		}

		public float GetProximityFitness(float proximity)
		{
			float result = 1f;
			if (m_checkProximity)
			{
				if (proximity < m_minProximity || proximity > m_maxProximity)
				{
					return 0f;
				}
				result = m_maxProximity - m_minProximity;
				if (result > 0f)
				{
					result = (proximity - m_minProximity) / result;
					result = m_proximityFitness.Evaluate(result);
				}
				else
				{
					result = 0f;
				}
			}
			return result;
		}

		public float GetTextureFitness(float[] textures)
		{
			bool flag = false;
			float num = 1f;
			if (m_checkTexture)
			{
				num = float.MaxValue;
				for (int i = 0; i < m_matchingTextures.Length; i++)
				{
					if (m_matchingTextures[i] == '!')
					{
						flag = true;
					}
					else if (flag)
					{
						float num2 = 1f - textures[(int)char.GetNumericValue(m_matchingTextures[i])];
						flag = false;
						if (num == float.MaxValue || num2 < num)
						{
							num = num2;
						}
					}
					else
					{
						float num2 = textures[(int)char.GetNumericValue(m_matchingTextures[i])];
						if (num == float.MaxValue || num2 > num)
						{
							num = num2;
						}
					}
				}
			}
			return num;
		}

		public float GetFitness(ref SpawnInfo spawnInfo)
		{
			if (!m_isInitialised)
			{
				Initialise(spawnInfo.m_spawner);
			}
			if (!m_isActive)
			{
				return 0f;
			}
			if (m_virginTerrain && !spawnInfo.m_wasVirginTerrain)
			{
				return 0f;
			}
			float num = 1f;
			if (m_checkHeight)
			{
				num = Mathf.Min(num, GetHeightFitness(spawnInfo.m_terrainHeightWU, spawnInfo.m_spawner.m_resources.m_seaLevel));
			}
			if (m_checkSlope && num > 0f)
			{
				num = ((m_checkType != 0) ? Mathf.Min(num, GetSlopeFitness(spawnInfo.m_areaAvgSlopeWU)) : Mathf.Min(num, GetSlopeFitness(spawnInfo.m_terrainSlopeWU)));
			}
			if (m_checkTexture && num > 0f)
			{
				num = Mathf.Min(num, GetTextureFitness(spawnInfo.m_textureStrengths));
			}
			if (m_checkProximity && num > 0f)
			{
				Rect area = new Rect(spawnInfo.m_hitLocationWU.x - m_maxProximity, spawnInfo.m_hitLocationWU.z - m_maxProximity, m_maxProximity * 2f, m_maxProximity * 2f);
				GameObject closestObject = spawnInfo.m_spawner.GetClosestObject(m_proximityTag, area);
				num = ((!(closestObject != null)) ? 0f : Mathf.Min(num, GetProximityFitness(Vector3.Distance(closestObject.transform.position, spawnInfo.m_hitLocationWU))));
			}
			return num;
		}
	}
}

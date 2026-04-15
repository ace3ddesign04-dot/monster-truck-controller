using UnityEngine;

namespace Gaia
{
	public class SpawnInfo
	{
		public Spawner m_spawner;

		public bool m_outOfBounds = true;

		public bool m_wasVirginTerrain;

		public float m_spawnRotationY;

		public float m_hitDistanceWU;

		public Vector3 m_hitLocationWU;

		public Vector3 m_hitLocationNU;

		public Vector3 m_hitNormal;

		public Transform m_hitObject;

		public Terrain m_hitTerrain;

		public float m_terrainHeightWU;

		public float m_terrainSlopeWU;

		public Vector3 m_terrainNormalWU;

		public float m_fitness;

		public float[] m_textureStrengths;

		public Vector3[] m_areaHitsWU;

		public float m_areaHitSlopeWU;

		public float m_areaMinSlopeWU;

		public float m_areaAvgSlopeWU;

		public float m_areaMaxSlopeWU;
	}
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GeNa
{
	public class ProbeManager
	{
		private Quadtree<LightProbeGroup> m_probeLocations = new Quadtree<LightProbeGroup>(new Rect(0f, 0f, 10f, 10f));

		public void LoadProbesFromScene()
		{
			m_probeLocations = null;
			float f = float.NaN;
			float num = float.NaN;
			float num2 = float.NaN;
			float num3 = float.NaN;
			float num4 = float.NaN;
			Terrain x = null;
			Terrain[] activeTerrains = Terrain.activeTerrains;
			foreach (Terrain terrain in activeTerrains)
			{
				if (float.IsNaN(f))
				{
					x = terrain;
					Vector3 position = terrain.transform.position;
					f = position.y;
					Vector3 position2 = terrain.transform.position;
					num = position2.x;
					Vector3 position3 = terrain.transform.position;
					num3 = position3.z;
					float num5 = num;
					Vector3 size = terrain.terrainData.size;
					num2 = num5 + size.x;
					float num6 = num3;
					Vector3 size2 = terrain.terrainData.size;
					num4 = num6 + size2.z;
					continue;
				}
				Vector3 position4 = terrain.transform.position;
				if (position4.x < num)
				{
					Vector3 position5 = terrain.transform.position;
					num = position5.x;
				}
				Vector3 position6 = terrain.transform.position;
				if (position6.z < num3)
				{
					Vector3 position7 = terrain.transform.position;
					num3 = position7.z;
				}
				Vector3 position8 = terrain.transform.position;
				float x2 = position8.x;
				Vector3 size3 = terrain.terrainData.size;
				if (x2 + size3.x > num2)
				{
					Vector3 position9 = terrain.transform.position;
					float x3 = position9.x;
					Vector3 size4 = terrain.terrainData.size;
					num2 = x3 + size4.x;
				}
				Vector3 position10 = terrain.transform.position;
				float z = position10.z;
				Vector3 size5 = terrain.terrainData.size;
				if (z + size5.z > num4)
				{
					Vector3 position11 = terrain.transform.position;
					float z2 = position11.z;
					Vector3 size6 = terrain.terrainData.size;
					num4 = z2 + size6.z;
				}
			}
			if (x != null)
			{
				Rect boundaries = new Rect(num, num3, num2 - num, num4 - num3);
				m_probeLocations = new Quadtree<LightProbeGroup>(boundaries);
			}
			else
			{
				Rect boundaries2 = new Rect(-10000f, -10000f, 20000f, 20000f);
				m_probeLocations = new Quadtree<LightProbeGroup>(boundaries2);
			}
			LightProbeGroup[] array = Object.FindObjectsOfType<LightProbeGroup>();
			foreach (LightProbeGroup lightProbeGroup in array)
			{
				for (int k = 0; k < lightProbeGroup.probePositions.Length; k++)
				{
					Quadtree<LightProbeGroup> probeLocations = m_probeLocations;
					Vector3 position12 = lightProbeGroup.transform.position;
					float x4 = position12.x + lightProbeGroup.probePositions[k].x;
					Vector3 position13 = lightProbeGroup.transform.position;
					probeLocations.Insert(x4, position13.z + lightProbeGroup.probePositions[k].z, lightProbeGroup);
				}
			}
		}

		public void AddProbe(Vector3 position, LightProbeGroup probeGroup)
		{
			if (m_probeLocations != null)
			{
				m_probeLocations.Insert(position.x, position.z, probeGroup);
			}
		}

		public List<LightProbeGroup> GetProbeGroups(Vector3 position, float range)
		{
			if (m_probeLocations == null)
			{
				return new List<LightProbeGroup>();
			}
			Rect range2 = new Rect(position.x - range, position.z - range, range * 2f, range * 2f);
			return m_probeLocations.Find(range2).ToList();
		}

		public int Count(Vector3 position, float range)
		{
			if (m_probeLocations == null)
			{
				return 0;
			}
			Rect range2 = new Rect(position.x - range, position.z - range, range * 2f, range * 2f);
			return m_probeLocations.Find(range2).Count();
		}

		public int Count()
		{
			if (m_probeLocations == null)
			{
				return 0;
			}
			return m_probeLocations.Count;
		}
	}
}

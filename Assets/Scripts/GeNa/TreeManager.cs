using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GeNa
{
	public class TreeManager
	{
		private List<TreePrototype> m_terrainTrees = new List<TreePrototype>();

		private Quadtree<int> m_terrainTreeLocations = new Quadtree<int>(new Rect(0f, 0f, 10f, 10f));

		public void LoadTreesFromTerrain()
		{
			m_terrainTrees = null;
			m_terrainTreeLocations = null;
			float f = float.NaN;
			float num = float.NaN;
			float num2 = float.NaN;
			float num3 = float.NaN;
			float num4 = float.NaN;
			Terrain terrain = null;
			Terrain[] activeTerrains = Terrain.activeTerrains;
			foreach (Terrain terrain2 in activeTerrains)
			{
				if (float.IsNaN(f))
				{
					terrain = terrain2;
					Vector3 position = terrain2.transform.position;
					f = position.y;
					Vector3 position2 = terrain2.transform.position;
					num = position2.x;
					Vector3 position3 = terrain2.transform.position;
					num3 = position3.z;
					float num5 = num;
					Vector3 size = terrain2.terrainData.size;
					num2 = num5 + size.x;
					float num6 = num3;
					Vector3 size2 = terrain2.terrainData.size;
					num4 = num6 + size2.z;
					continue;
				}
				Vector3 position4 = terrain2.transform.position;
				if (position4.x < num)
				{
					Vector3 position5 = terrain2.transform.position;
					num = position5.x;
				}
				Vector3 position6 = terrain2.transform.position;
				if (position6.z < num3)
				{
					Vector3 position7 = terrain2.transform.position;
					num3 = position7.z;
				}
				Vector3 position8 = terrain2.transform.position;
				float x = position8.x;
				Vector3 size3 = terrain2.terrainData.size;
				if (x + size3.x > num2)
				{
					Vector3 position9 = terrain2.transform.position;
					float x2 = position9.x;
					Vector3 size4 = terrain2.terrainData.size;
					num2 = x2 + size4.x;
				}
				Vector3 position10 = terrain2.transform.position;
				float z = position10.z;
				Vector3 size5 = terrain2.terrainData.size;
				if (z + size5.z > num4)
				{
					Vector3 position11 = terrain2.transform.position;
					float z2 = position11.z;
					Vector3 size6 = terrain2.terrainData.size;
					num4 = z2 + size6.z;
				}
			}
			if (!(terrain != null))
			{
				return;
			}
			Rect boundaries = new Rect(num, num3, num2 - num, num4 - num3);
			m_terrainTreeLocations = new Quadtree<int>(boundaries);
			m_terrainTrees = new List<TreePrototype>(terrain.terrainData.treePrototypes);
			Terrain[] activeTerrains2 = Terrain.activeTerrains;
			foreach (Terrain terrain3 in activeTerrains2)
			{
				Vector3 position12 = terrain3.transform.position;
				float x3 = position12.x;
				Vector3 position13 = terrain3.transform.position;
				float z3 = position13.z;
				Vector3 size7 = terrain3.terrainData.size;
				float x4 = size7.x;
				Vector3 size8 = terrain3.terrainData.size;
				float z4 = size8.z;
				TreeInstance[] treeInstances = terrain3.terrainData.treeInstances;
				for (int k = 0; k < treeInstances.Length; k++)
				{
					TreeInstance treeInstance = treeInstances[k];
					m_terrainTreeLocations.Insert(x3 + treeInstance.position.x * x4, z3 + treeInstance.position.z * z4, treeInstances[k].prototypeIndex);
				}
			}
		}

		public void AddTree(Vector3 position, int prototypeIdx)
		{
			if (m_terrainTreeLocations != null)
			{
				m_terrainTreeLocations.Insert(position.x, position.z, prototypeIdx);
			}
		}

		public int Count(Vector3 position, float range)
		{
			if (m_terrainTreeLocations == null)
			{
				return 0;
			}
			Rect range2 = new Rect(position.x - range, position.z - range, range * 2f, range * 2f);
			return m_terrainTreeLocations.Find(range2).Count();
		}

		public int Count()
		{
			if (m_terrainTreeLocations == null)
			{
				return 0;
			}
			return m_terrainTreeLocations.Count;
		}
	}
}

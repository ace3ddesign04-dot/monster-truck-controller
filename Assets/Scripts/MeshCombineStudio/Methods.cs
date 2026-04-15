using System.Collections.Generic;
using UnityEngine;

namespace MeshCombineStudio
{
	public static class Methods
	{
		public static void SetTag(GameObject go, string tag)
		{
			Transform[] componentsInChildren = go.GetComponentsInChildren<Transform>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].tag = tag;
			}
		}

		public static void SetTagWhenCollider(GameObject go, string tag)
		{
			Transform[] componentsInChildren = go.GetComponentsInChildren<Transform>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].GetComponent<Collider>() != null)
				{
					componentsInChildren[i].tag = tag;
				}
			}
		}

		public static void SetTagAndLayer(GameObject go, string tag, int layer)
		{
			Transform[] componentsInChildren = go.GetComponentsInChildren<Transform>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].tag = tag;
				componentsInChildren[i].gameObject.layer = layer;
			}
		}

		public static void SetLayer(GameObject go, int layer)
		{
			go.layer = layer;
			Transform[] componentsInChildren = go.GetComponentsInChildren<Transform>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].gameObject.layer = layer;
			}
		}

		public static bool Contains(string compare, string name)
		{
			List<string> list = new List<string>();
			int num;
			do
			{
				num = name.IndexOf("*");
				if (num != -1)
				{
					if (num != 0)
					{
						list.Add(name.Substring(0, num));
					}
					if (num == name.Length - 1)
					{
						break;
					}
					name = name.Substring(num + 1);
				}
			}
			while (num != -1);
			list.Add(name);
			for (int i = 0; i < list.Count; i++)
			{
				if (!compare.Contains(list[i]))
				{
					return false;
				}
			}
			return true;
		}

		public static T[] Search<T>(GameObject parentGO = null)
		{
			GameObject[] array = new GameObject[1]
			{
				parentGO
			};
			if (typeof(T) == typeof(GameObject))
			{
				List<GameObject> list = new List<GameObject>();
				for (int i = 0; i < array.Length; i++)
				{
					Transform[] componentsInChildren = array[i].GetComponentsInChildren<Transform>(includeInactive: true);
					for (int j = 0; j < componentsInChildren.Length; j++)
					{
						list.Add(componentsInChildren[j].gameObject);
					}
				}
				return list.ToArray() as T[];
			}
			if (parentGO == null)
			{
				List<T> list2 = new List<T>();
				for (int k = 0; k < array.Length; k++)
				{
					list2.AddRange(array[k].GetComponentsInChildren<T>(includeInactive: true));
				}
				return list2.ToArray();
			}
			return parentGO.GetComponentsInChildren<T>(includeInactive: true);
		}

		public static T Find<T>(GameObject parentGO, string name) where T : Object
		{
			T[] array = Search<T>(parentGO);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].name == name)
				{
					return array[i];
				}
			}
			return (T)null;
		}

		public static void SetCollidersActive(Collider[] colliders, bool active, string[] nameList)
		{
			for (int i = 0; i < colliders.Length; i++)
			{
				for (int j = 0; j < nameList.Length; j++)
				{
					if (colliders[i].name.Contains(nameList[j]))
					{
						colliders[i].enabled = active;
					}
				}
			}
		}
	}
}

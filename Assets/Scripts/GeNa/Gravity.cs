using System;
using System.Collections.Generic;
using UnityEngine;

namespace GeNa
{
	public class Gravity : ScriptableObject
	{
		[Serializable]
		public class GravityInstance
		{
			public Resource m_resource;

			public GameObject m_instance;

			public Vector3 m_startPosition;

			public Vector3 m_endPosition;

			public Vector3 m_startRotation;

			public Vector3 m_endRotation;
		}

		public bool m_haveGravity;

		public List<GravityInstance> m_instances = new List<GravityInstance>();

		public void UpdateInstances()
		{
			foreach (GravityInstance instance in m_instances)
			{
				if (instance.m_instance != null)
				{
					instance.m_endPosition = instance.m_instance.transform.position;
					instance.m_endRotation = instance.m_instance.transform.rotation.eulerAngles;
				}
				m_haveGravity = true;
			}
		}

		public void AddInstances(List<GravityInstance> instanceList)
		{
			m_haveGravity = false;
			m_instances.AddRange(instanceList);
		}

		public void UpdateOriginalsToStart()
		{
			foreach (GravityInstance instance in m_instances)
			{
				if (instance.m_instance != null)
				{
					instance.m_instance.transform.position = instance.m_startPosition;
					instance.m_instance.transform.rotation = Quaternion.Euler(instance.m_startRotation.x, instance.m_startRotation.y, instance.m_startRotation.z);
				}
			}
		}

		public void UpdateOriginalsToEnd()
		{
			foreach (GravityInstance instance in m_instances)
			{
				if (instance.m_instance != null)
				{
					instance.m_instance.transform.position = instance.m_endPosition;
					instance.m_instance.transform.rotation = Quaternion.Euler(instance.m_endRotation.x, instance.m_endRotation.y, instance.m_endRotation.z);
				}
			}
		}

		public void FinaliseGravity(Spawner spawner)
		{
			spawner.LoadLightProbes();
			foreach (GravityInstance instance in m_instances)
			{
				if (instance.m_instance != null)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(instance.m_resource.m_prefab);
					gameObject.name = "_Sp_" + instance.m_resource.m_name;
					if (instance.m_resource.m_conformToSlope)
					{
						gameObject.name = "_Sp_" + instance.m_resource.m_name + " C";
					}
					gameObject.transform.position = instance.m_endPosition;
					gameObject.transform.localScale = instance.m_instance.transform.localScale;
					gameObject.transform.rotation = Quaternion.Euler(instance.m_endRotation.x, instance.m_endRotation.y, instance.m_endRotation.z);
					gameObject.transform.parent = instance.m_instance.transform.parent;
					spawner.AutoOptimiseGameObject(instance.m_resource, gameObject);
					spawner.AutoProbeGameObject(instance.m_resource, gameObject);
					UnityEngine.Object.DestroyImmediate(instance.m_instance);
				}
			}
			m_haveGravity = false;
			m_instances.Clear();
		}
	}
}

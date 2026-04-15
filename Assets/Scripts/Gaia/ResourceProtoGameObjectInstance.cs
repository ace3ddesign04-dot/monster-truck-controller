using System;
using UnityEngine;

namespace Gaia
{
	[Serializable]
	public class ResourceProtoGameObjectInstance
	{
		[Tooltip("Instance name.")]
		public string m_name;

		[Tooltip("Desktop prefab.")]
		public GameObject m_desktopPrefab;

		[HideInInspector]
		public string m_desktopPrefabFileName;

		[Tooltip("Mobile prefab - future proofing here - not currently used.")]
		public GameObject m_mobilePrefab;

		[HideInInspector]
		public string m_mobilePrefabFileName;

		[Tooltip("Minimum number of instances to spawn.")]
		public int m_minInstances = 1;

		[Tooltip("Maximum number of instances to spawn.")]
		public int m_maxInstances = 1;

		[Tooltip("The failure chance of each spawn attempt.")]
		[Range(0f, 1f)]
		public float m_failureRate;

		[Tooltip("Minimum X offset from spawn point in meters to intantiate at. Can use this to move objects relative to the spawn point chosen.")]
		public float m_minSpawnOffsetX;

		[Tooltip("Maximum X offset from spawn point in meters to intantiate at. Can use this to move objects relative to the spawn point chosen.")]
		public float m_maxSpawnOffsetX;

		[Tooltip("Minimum Y offset from terrain in meters to intantiate at. Can use this to move embed or raise objects from the terrain.")]
		public float m_minSpawnOffsetY = -0.3f;

		[Tooltip("Maximum Y offset from terrain in meters to intantiate at. Can use this to move embed or raise objects from the terrain.")]
		public float m_maxSpawnOffsetY = -0.1f;

		[Tooltip("Minimum Z offset from spawn point in meters to intantiate at. Can use this to move objects relative to the spawn point chosen.")]
		public float m_minSpawnOffsetZ;

		[Tooltip("Maximum Z offset from spawn point in meters to intantiate at. Can use this to move objects relative to the spawn point chosen.")]
		public float m_maxSpawnOffsetZ;

		[Tooltip("Rotate the object to the terrain normal. Allows natural slope following. Great for things like trees to give them a little more variation in your scene.")]
		public bool m_rotateToSlope;

		[Tooltip("Minimum X rotation from spawned rotation to intantiate at. Can use this to rotate objects relative to spawn point rotation.")]
		[Range(-180f, 180f)]
		public float m_minRotationOffsetX;

		[Tooltip("Maximum X rotation from spawned rotation to intantiate at. Can use this to rotate objects relative to spawn point rotation.")]
		[Range(-180f, 180f)]
		public float m_maxRotationOffsetX;

		[Tooltip("Minimum Y rotation from spawned rotation to intantiate at. Can use this to rotate objects relative to spawn point rotation.")]
		[Range(-180f, 180f)]
		public float m_minRotationOffsetY = -180f;

		[Tooltip("Maximum Y rotation from spawned rotation to intantiate at. Can use this to rotate objects relative to spawn point rotation.")]
		[Range(-180f, 180f)]
		public float m_maxRotationOffsetY = 180f;

		[Tooltip("Minimum Z rotation from spawned rotation to intantiate at. Can use this to rotate objects relative to spawn point rotation.")]
		[Range(-180f, 180f)]
		public float m_minRotationOffsetZ;

		[Tooltip("Maximum Z rotation from spawned rotation to intantiate at. Can use this to rotate objects relative to spawn point rotation.")]
		[Range(-180f, 180f)]
		public float m_maxRotationOffsetZ;

		[Tooltip("Get object scale from parent scale.")]
		public bool m_useParentScale = true;

		[Tooltip("Minimum scale.")]
		[Range(0f, 20f)]
		public float m_minScale = 1f;

		[Tooltip("Maximum scale.")]
		[Range(0f, 20f)]
		public float m_maxScale = 1f;

		[Tooltip("Influence scale between min and max scale based on distance from spawn point centre.")]
		public AnimationCurve m_scaleByDistance = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 1f));

		[Tooltip("Local bounds radius of this instance.")]
		public float m_localBounds = 5f;

		[Tooltip("Will only spawn on virgin terrain.")]
		public bool m_virginTerrain = true;

		[Tooltip("Custom parameter to be interpreted by an extension if there is one.")]
		public string m_extParam = string.Empty;
	}
}

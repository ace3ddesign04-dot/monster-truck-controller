using UnityEngine;

[CreateAssetMenu(fileName = "LevelEditorResources", menuName = "LevelEditorResources", order = 1)]
public class LevelEditorResources : ScriptableObject
{
	[Header("Routes")]
	public GameObject routeStartPrefab;

	public GameObject routeCheckpointPrefab;

	public GameObject routeFinishPrefab;

	public GameObject routeIndicatorPrefab;

	[Header("Trees")]
	public int maxTreesCount;

	[Header("Materials")]
	public Material routeMaterial;

	public Material waterMaterial;

	public Material frozenWaterMaterial;

	[Header("Object prefabs")]
	public ExtraObjectArray[] extraObjectsDictionary;

	public Prop[] propsDictionary;

	public GameObject mudIndicatorPrefab;

	[Header("Cliffs")]
	public int baseCliffsCount;

	public float minHillAngle;

	public GameObject[] cliffPrefabs;

	[Header("Textures")]
	public int underwaterTextureID;

	public int rockTextureID;

	public int mudTextureID;

	public float minRockAngle;

	public float maxRockAngle;

	public Texture2D[] stampTextures;

	public Texture2D[] pathPatterns;

	public Texture2D[] terrainGenerationStamps;

	public Texture2D[] mudStampTextures;
}

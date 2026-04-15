using UnityEngine;

public class MudTerrain : MonoBehaviour
{
	[HideInInspector]
	public Terrain terrain;

	[HideInInspector]
	public TerrainData terrainData;

	[HideInInspector]
	public Texture2D[] textures;

	[HideInInspector]
	public int hRes;

	[HideInInspector]
	public int aRes;

	[HideInInspector]
	public Rect terRect;

	[HideInInspector]
	public float[,] DefaultHeights;

	[HideInInspector]
	public float[,,] DefaultAlphaMap;

	[HideInInspector]
	public bool LODS_Baked;

	[HideInInspector]
	public float DeformableMaterialMaxDepth;

	public float drag;

	private void Start()
	{
		GetComponent<TerrainCollider>().enabled = false;
		DefaultHeights = terrainData.GetHeights(0, 0, hRes, hRes);
	}
}

using UnityEngine;

[ExecuteInEditMode]
public class ObjectSpawner : MonoBehaviour
{
	public GameObject[] objects;

	public float density = 0.5f;

	public Vector2 scaleRange = new Vector2(0.5f, 2f);

	public Vector3 rotationRange = new Vector3(5f, 360f, 5f);

	public Vector2 heightRange = new Vector2(0f, 1f);

	public float scaleMulti = 1f;

	public float resolutionPerMeter = 2f;

	public bool spawnInRuntime;

	public bool spawnOnStart;

	public bool spawn;

	public bool deleteChildren;

	private Transform t;

	private void Awake()
	{
		t = base.transform;
	}

	private void Start()
	{
		if (spawnInRuntime && spawnOnStart)
		{
			Spawn();
		}
	}

	private void Update()
	{
		if (spawn)
		{
			spawn = false;
			Spawn();
		}
		if (deleteChildren)
		{
			deleteChildren = false;
			DeleteChildren();
		}
	}

	public void DeleteChildren()
	{
		Transform[] componentsInChildren = GetComponentsInChildren<Transform>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (t != componentsInChildren[i] && componentsInChildren[i] != null)
			{
				UnityEngine.Object.DestroyImmediate(componentsInChildren[i].gameObject);
			}
		}
	}

	public void Spawn()
	{
		Bounds bounds = default(Bounds);
		bounds.center = base.transform.position;
		bounds.size = base.transform.lossyScale;
		Vector3 min = bounds.min;
		float x = min.x;
		Vector3 max = bounds.max;
		float x2 = max.x;
		Vector3 min2 = bounds.min;
		float y = min2.y;
		Vector3 max2 = bounds.max;
		float y2 = max2.y;
		Vector3 min3 = bounds.min;
		float z = min3.z;
		Vector3 max3 = bounds.max;
		float z2 = max3.z;
		int max4 = objects.Length;
		float num = resolutionPerMeter * 0.5f;
		Vector3 lossyScale = base.transform.lossyScale;
		float num2 = lossyScale.y * 0.5f;
		int num3 = 0;
		for (float num4 = z; num4 < z2; num4 += resolutionPerMeter)
		{
			for (float num5 = x; num5 < x2; num5 += resolutionPerMeter)
			{
				for (float num6 = y; num6 < y2; num6 += resolutionPerMeter)
				{
					int num7 = UnityEngine.Random.Range(0, max4);
					float value = UnityEngine.Random.value;
					if (value < density)
					{
						Vector3 position = new Vector3(num5 + UnityEngine.Random.Range(0f - num, num), (num6 + UnityEngine.Random.Range(0f - num, num)) * UnityEngine.Random.Range(heightRange.x, heightRange.y), num4 + UnityEngine.Random.Range(0f - num, num));
						if (!(position.x < x) && !(position.x > x2) && !(position.y < y) && !(position.y > y2) && !(position.z < z) && !(position.z > z2))
						{
							position.y += num2;
							GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(rotation: Quaternion.Euler(new Vector3(UnityEngine.Random.Range(0f, rotationRange.x), UnityEngine.Random.Range(0f, rotationRange.y), UnityEngine.Random.Range(0f, rotationRange.z))), original: objects[num7], position: position);
							float num8 = UnityEngine.Random.Range(scaleRange.x, scaleRange.y) * scaleMulti;
							gameObject.transform.localScale = new Vector3(num8, num8, num8);
							gameObject.transform.parent = t;
							num3++;
						}
					}
				}
			}
		}
		UnityEngine.Debug.Log("Spawned " + num3);
	}

	private void OnDrawGizmosSelected()
	{
		Vector3 position = base.transform.position;
		Vector3 lossyScale = base.transform.lossyScale;
		Gizmos.DrawWireCube(position + new Vector3(0f, lossyScale.y * 0.5f, 0f), base.transform.lossyScale);
	}
}

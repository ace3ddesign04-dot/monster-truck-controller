using UnityEngine;

public class VertexColorTest : MonoBehaviour
{
	public float threshold;

	public Transform[] spheres;

	private Color[] colors;

	private Mesh mesh;

	private Vector3[] vertices;

	private int[] triangles;

	public float step = 0.1f;

	public Color colorToSet;

	private void Start()
	{
		mesh = GetComponent<MeshFilter>().mesh;
		vertices = mesh.vertices;
		triangles = mesh.triangles;
		colors = new Color[vertices.Length];
		for (int i = 0; i < colors.Length; i++)
		{
			colors[i] = colorToSet;
		}
		mesh.colors = colors;
	}

	private void Update()
	{
		Transform[] array = spheres;
		foreach (Transform transform in array)
		{
			if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo))
			{
				int num = triangles[hitInfo.triangleIndex * 3];
				int num2 = triangles[hitInfo.triangleIndex * 3 + 1];
				int num3 = triangles[hitInfo.triangleIndex * 3 + 2];
				Vector3 vector = vertices[num];
				Vector3 vector2 = vertices[num2];
				Vector3 vector3 = vertices[num3];
				Color color = colors[num];
				color.r += step;
				color.r = Mathf.Clamp01(color.r);
				colors[num] = color;
				color = colors[num2];
				color.r += step;
				color.r = Mathf.Clamp01(color.r);
				colors[num2] = color;
				color = colors[num3];
				color.r += step;
				color.r = Mathf.Clamp01(color.r);
				colors[num3] = color;
			}
		}
		mesh.colors = colors;
	}

	[ContextMenu("Apply")]
	private void apply()
	{
		for (int i = 0; i < colors.Length; i++)
		{
			Vector3 vector = base.transform.TransformPoint(vertices[i]);
			Color color = colors[i];
			colors[i] = color;
		}
		mesh.colors = colors;
	}
}

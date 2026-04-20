using UnityEngine;
using UnityEngine.Rendering;

public class TireMarksRenderer : MonoBehaviour
{
	public int maxMarks = 1024;

	public Material material;

	private int m_markCount;

	private int m_markArraySize;

	private MarkPoint[] m_markPoints;

	private bool m_segmentsUpdated;

	private int m_segmentCount;

	private int m_segmentArraySize;

	private Mesh m_mesh;

	private Vector3[] m_vertices;

	private Vector3[] m_normals;

	private Vector4[] m_tangents;

	private Color[] m_colors;

	private Vector2[] m_uvs;

	private int[] m_triangles;

	private Vector2[] m_values;

	private void Start()
	{
		base.transform.position = Vector3.zero;
		base.transform.rotation = Quaternion.identity;
		MeshFilter meshFilter = base.gameObject.GetComponent<MeshFilter>();
		if (meshFilter == null)
		{
			meshFilter = base.gameObject.AddComponent<MeshFilter>();
		}
		MeshRenderer component = base.gameObject.GetComponent<MeshRenderer>();
		if (component == null)
		{
			component = base.gameObject.AddComponent<MeshRenderer>();
			component.shadowCastingMode = ShadowCastingMode.Off;
			component.material = material;
		}
		m_markPoints = new MarkPoint[maxMarks * 2];
		int i = 0;
		for (int num = m_markPoints.Length; i < num; i++)
		{
			m_markPoints[i] = new MarkPoint();
		}
		m_markCount = 0;
		m_markArraySize = m_markPoints.Length;
		m_vertices = new Vector3[maxMarks * 4];
		m_normals = new Vector3[maxMarks * 4];
		m_tangents = new Vector4[maxMarks * 4];
		m_colors = new Color[maxMarks * 4];
		m_uvs = new Vector2[maxMarks * 4];
		m_triangles = new int[maxMarks * 6];
		m_values = new Vector2[maxMarks];
		m_segmentCount = 0;
		m_segmentArraySize = maxMarks;
		m_segmentsUpdated = false;
		for (int j = 0; j < m_segmentArraySize; j++)
		{
			m_uvs[j * 4] = new Vector2(0f, 0.05f);
			m_uvs[j * 4 + 1] = new Vector2(1f, 0.05f);
			m_uvs[j * 4 + 2] = new Vector2(0f, 0.95f);
			m_uvs[j * 4 + 3] = new Vector2(1f, 0.95f);
			m_triangles[j * 6] = j * 4;
			m_triangles[j * 6 + 2] = j * 4 + 1;
			m_triangles[j * 6 + 1] = j * 4 + 2;
			m_triangles[j * 6 + 3] = j * 4 + 2;
			m_triangles[j * 6 + 5] = j * 4 + 1;
			m_triangles[j * 6 + 4] = j * 4 + 3;
		}
		m_mesh = new Mesh();
		m_mesh.MarkDynamic();
		m_mesh.vertices = m_vertices;
		m_mesh.normals = m_normals;
		m_mesh.tangents = m_tangents;
		m_mesh.colors = m_colors;
		m_mesh.triangles = m_triangles;
		m_mesh.uv = m_uvs;
		m_mesh.RecalculateBounds();
		meshFilter.mesh = m_mesh;
	}

	private void OnValidate()
	{
		if (m_uvs != null)
		{
			for (int i = 0; i < m_uvs.Length / 4; i++)
			{
				m_uvs[i * 4] = new Vector2(0f, 0.05f);
				m_uvs[i * 4 + 1] = new Vector2(1f, 0.05f);
				m_uvs[i * 4 + 2] = new Vector2(0f, 0.95f);
				m_uvs[i * 4 + 3] = new Vector2(1f, 0.95f);
			}
		}
		m_segmentsUpdated = true;
	}

	public int AddMark(Vector3 pos, Vector3 normal, float width, int lastIndex)
	{
		if (!base.isActiveAndEnabled || m_markArraySize == 0)
		{
			return -1;
		}
		Vector3 vector = pos + normal * 0.02f;
		if (lastIndex >= 0 && Vector3.Distance(vector, m_markPoints[lastIndex % m_markArraySize].pos) < 0.1f)
		{
			return lastIndex;
		}
		if (lastIndex >= 0 && Vector3.Distance(vector, m_markPoints[lastIndex % m_markArraySize].pos) > 10f)
		{
			return -1;
		}
		MarkPoint markPoint = m_markPoints[m_markCount % m_markArraySize];
		markPoint.pos = vector;
		markPoint.normal = normal;
		markPoint.lastIndex = lastIndex;
		if (lastIndex >= 0 && lastIndex > m_markCount - m_markArraySize)
		{
			MarkPoint markPoint2 = m_markPoints[lastIndex % m_markArraySize];
			Vector3 lhs = markPoint.pos - markPoint2.pos;
			Vector3 normalized = Vector3.Cross(lhs, normal).normalized;
			Vector3 b = 0.5f * width * normalized;
			markPoint.posl = markPoint.pos + b;
			markPoint.posr = markPoint.pos - b;
			markPoint.tangent = new Vector4(normalized.x, normalized.y, normalized.z, 1f);
			if (markPoint2.lastIndex < 0)
			{
				markPoint2.tangent = markPoint.tangent;
				markPoint2.posl = markPoint.pos + b;
				markPoint2.posr = markPoint.pos - b;
			}
			AddSegment(markPoint2, markPoint);
		}
		m_markCount++;
		return m_markCount - 1;
	}

	private void AddSegment(MarkPoint first, MarkPoint second)
	{
		int num = m_segmentCount % m_segmentArraySize * 4;
		m_vertices[num] = first.posl;
		m_vertices[num + 1] = first.posr;
		m_vertices[num + 2] = second.posl;
		m_vertices[num + 3] = second.posr;
		m_normals[num] = first.normal;
		m_normals[num + 1] = first.normal;
		m_normals[num + 2] = second.normal;
		m_normals[num + 3] = second.normal;
		m_tangents[num] = first.tangent;
		m_tangents[num + 1] = first.tangent;
		m_tangents[num + 2] = second.tangent;
		m_tangents[num + 3] = second.tangent;
		m_colors[num].a = 1f;
		m_colors[num + 1].a = 1f;
		m_colors[num + 2].a = 1f;
		m_colors[num + 3].a = 1f;
		m_values[num / 4] = Vector2.one;
		if (m_segmentCount == 0)
		{
			Vector3 vector = m_vertices[0];
			int i = 4;
			for (int num2 = m_vertices.Length; i < num2; i++)
			{
				m_vertices[i] = vector;
			}
		}
		m_segmentCount++;
		m_segmentsUpdated = true;
	}

	private void LateUpdate()
	{
		if (!m_segmentsUpdated)
		{
			return;
		}
		m_segmentsUpdated = false;
		int num = (int)((float)m_segmentArraySize * 0.5f);
		if (num > 0)
		{
			int num2 = m_segmentCount - m_segmentArraySize;
			int num3 = 0;
			if (num2 < 0)
			{
				num3 = -num2;
				num2 = 0;
			}
			float num4 = 1f / (float)num;
			for (int i = num3; i < num; i++)
			{
				int num5 = num2 % m_segmentArraySize;
				int num6 = num5 * 4;
				float num7 = (float)i * num4;
				float a = m_values[num5].x * num7;
				float a2 = m_values[num5].y * num7 + num4;
				m_colors[num6].a = a;
				m_colors[num6 + 1].a = a;
				m_colors[num6 + 2].a = a2;
				m_colors[num6 + 3].a = a2;
				num2++;
			}
		}
		m_mesh.MarkDynamic();
		m_mesh.vertices = m_vertices;
		m_mesh.normals = m_normals;
		m_mesh.tangents = m_tangents;
		m_mesh.colors = m_colors;
		m_mesh.RecalculateBounds();
	}
}

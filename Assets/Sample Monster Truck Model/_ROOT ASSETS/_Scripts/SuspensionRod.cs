using UnityEngine;

[ExecuteAlways]
public class SuspensionRod : MonoBehaviour
{
    public enum Mode { LineRenderer, CylinderMesh }
    public Mode mode = Mode.LineRenderer;

    public Transform startPoint;
    public Transform endPoint;

    [Header("Line Renderer Settings")]
    public LineRenderer lr;
    public Material material;
    public float width = 0.05f;
    public Vector2 textureScale = new Vector2(1, 1);

    [Header("Cylinder Mesh Settings")]
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    public float cylinderRadius = 0.03f;
    public int cylinderSides = 12;

    private Mesh cylinderMesh;

    private void OnEnable()
    {
        ApplyAppearance();
    }

    private void OnValidate()
    {
        ApplyAppearance();
    }

    private void Update()
    {
        UpdateRod();
    }

    private void ApplyAppearance()
    {
        if (mode == Mode.LineRenderer)
        {
            if (!lr)
                lr = GetComponent<LineRenderer>();

            if (lr)
            {
                lr.enabled = true;
                if (meshRenderer) meshRenderer.enabled = false;

                lr.widthMultiplier = width;

                if (material)
                    lr.sharedMaterial = material; // ✅ prevents material instancing

                // ✅ FIX: Use sharedMaterial instead of material
                var mat = lr.sharedMaterial;
                if (mat)
                    mat.SetTextureScale("_MainTex", textureScale);
            }
        }
        else if (mode == Mode.CylinderMesh)
        {
            if (lr) lr.enabled = false;

            if (meshFilter)
            {
                if (!cylinderMesh)
                    cylinderMesh = new Mesh { name = "SuspensionRod_Cylinder" };

                meshFilter.sharedMesh = cylinderMesh;
                GenerateCylinderMesh();
            }

            if (meshRenderer && material)
                meshRenderer.sharedMaterial = material; // ✅ sharedMaterial prevents leak
        }
    }

    private void UpdateRod()
    {
        if (!startPoint || !endPoint) return;

        if (mode == Mode.LineRenderer && lr)
        {
            lr.positionCount = 2;
            lr.SetPosition(0, startPoint.position);
            lr.SetPosition(1, endPoint.position);
        }
        else if (mode == Mode.CylinderMesh && meshFilter)
        {
            Vector3 dir = endPoint.position - startPoint.position;
            Vector3 mid = (startPoint.position + endPoint.position) * 0.5f;
            float length = dir.magnitude;

            meshFilter.transform.position = mid;
            meshFilter.transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);
            meshFilter.transform.localScale = new Vector3(cylinderRadius, length * 0.5f, cylinderRadius);
        }
    }

    private void GenerateCylinderMesh()
    {
        // (unchanged mesh generation code)
        int segments = cylinderSides;
        Vector3[] vertices = new Vector3[(segments + 1) * 2];
        int[] triangles = new int[segments * 6];

        for (int i = 0; i <= segments; i++)
        {
            float angle = 2 * Mathf.PI * i / segments;
            float x = Mathf.Cos(angle) * cylinderRadius;
            float z = Mathf.Sin(angle) * cylinderRadius;

            vertices[i] = new Vector3(x, -0.5f, z);
            vertices[i + segments + 1] = new Vector3(x, 0.5f, z);

            if (i < segments)
            {
                int baseIdx = i * 6;
                triangles[baseIdx] = i;
                triangles[baseIdx + 1] = i + segments + 1;
                triangles[baseIdx + 2] = i + 1;

                triangles[baseIdx + 3] = i + 1;
                triangles[baseIdx + 4] = i + segments + 1;
                triangles[baseIdx + 5] = i + segments + 2;
            }
        }

        cylinderMesh.Clear();
        cylinderMesh.vertices = vertices;
        cylinderMesh.triangles = triangles;
        cylinderMesh.RecalculateNormals();
    }

    private void OnDestroy()
    {
        if (cylinderMesh)
            DestroyImmediate(cylinderMesh);
    }
}

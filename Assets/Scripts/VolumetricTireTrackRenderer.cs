using CustomVP;
using UnityEngine;

public class VolumetricTireTrackRenderer : MonoBehaviour
{
	public bool RenderTrack;

	public Material material;

	public WheelComponent wheelCollider;

	public int maxMarks = 100;

	public float TireWidth = 0.4f;

	public float BumpHeight = 0.2f;

	public float BumpWidth = 0.4f;

	public float VerticalOffset = 0.05f;

	public float step = 0.5f;

	public float spawnPointOffset = 0.2f;

	public float Randomness = 0.1f;

	private Mesh resultMesh;

	private Vector3[] vertices;

	private int[] triangles;

	private int currentMark;

	private Vector3 lastPos;

	private Vector3[] randomVectors;

	private bool breakStamp;

	private bool firstLapPassed;

	private int[] CurrentVerticesIDs;

	private int[] PrevVerticesIDs;

	private int[] LastVerticlesIDs;

	private int[] LastVerticlesTargetsIDs;

	private Vector3[] LastVerticlesPositions;

	private float lastDirection;

	private void Start()
	{
		resultMesh = new Mesh();
		vertices = new Vector3[maxMarks * 6 + 6];
		triangles = new int[maxMarks * 30 + 30];
		Vector2[] array = new Vector2[maxMarks * 6 + 6];
		Vector3[] array2 = new Vector3[maxMarks * 6 + 6];
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i] = Vector3.up;
		}
		for (int j = 0; j < maxMarks; j++)
		{
			int[] array3 = new int[6];
			for (int k = 0; k < 6; k++)
			{
				array3[k] = j * 6 + k;
			}
			float y = (float)j * step;
			array[array3[0]] = new Vector2(0f, y);
			array[array3[1]] = new Vector2(0.1f, y);
			array[array3[2]] = new Vector2(0.2f, y);
			array[array3[3]] = new Vector2(0.8f, y);
			array[array3[4]] = new Vector2(0.9f, y);
			array[array3[5]] = new Vector2(1f, y);
		}
		resultMesh.vertices = vertices;
		resultMesh.normals = array2;
		resultMesh.uv = array;
		resultMesh.MarkDynamic();
		base.gameObject.AddComponent<MeshFilter>().mesh = resultMesh;
		base.gameObject.AddComponent<MeshRenderer>().material = material;
		lastPos = base.transform.position;
		ChangeRandomVectors();
	}

	private void ChangeRandomVectors()
	{
		randomVectors = new Vector3[4];
		for (int i = 0; i < 4; i++)
		{
			randomVectors[i] = UnityEngine.Random.insideUnitSphere * Randomness;
		}
	}

	private void Spawn3DStamp()
	{
		if (!wheelCollider.IsGrounded)
		{
			return;
		}
		Vector3 realHitPoint = wheelCollider.wheelCollider.realHitPoint;
		Vector3 b = wheelCollider.transform.forward * spawnPointOffset * ((wheelCollider.rpm >= 0f) ? 1 : (-1));
		Vector3 vector = Vector3.Cross(wheelCollider.wheelCollider.contactNormal, wheelCollider.transform.forward);
		Vector3 contactNormal = wheelCollider.wheelCollider.contactNormal;
		Vector3 to = realHitPoint - lastPos;
		if (Vector3.Distance(realHitPoint, lastPos) > step * 3f || Vector3.Angle(vector, to) < 45f || Vector3.Angle(-vector, to) < 45f)
		{
			lastPos = realHitPoint + b;
			BreakStamp();
			return;
		}
		if (currentMark % 4 == 0)
		{
			ChangeRandomVectors();
		}
		int[] array = new int[6];
		for (int i = 0; i < 6; i++)
		{
			array[i] = currentMark * 6 + i;
		}
		int num = currentMark + 1;
		if (num > maxMarks)
		{
			num = 0;
		}
		int num2 = currentMark + 2;
		if (num2 == maxMarks + 1)
		{
			num2 = 0;
		}
		if (num2 == maxMarks + 2)
		{
			num2 = 1;
		}
		LastVerticlesPositions = new Vector3[6];
		LastVerticlesIDs = new int[6];
		for (int j = 0; j < 6; j++)
		{
			LastVerticlesIDs[j] = num * 6 + j;
			LastVerticlesPositions[j] = vertices[LastVerticlesIDs[j]];
		}
		LastVerticlesTargetsIDs = new int[6];
		for (int k = 0; k < 6; k++)
		{
			LastVerticlesTargetsIDs[k] = num2 * 6 + k;
		}
		float d = 0f;
		float d2 = 0f;
		if (Vector3.Distance(base.transform.position, lastPos) > step * 3f)
		{
			d = 0f - BumpHeight;
			d2 = 0f - VerticalOffset;
		}
		vertices[array[0]] = realHitPoint + b - vector * (TireWidth / 2f + BumpWidth) + Vector3.ProjectOnPlane(randomVectors[0], contactNormal);
		vertices[array[1]] = realHitPoint + b - vector * (TireWidth / 2f + BumpWidth * 0.5f) + Vector3.ProjectOnPlane(randomVectors[1], contactNormal) + contactNormal * d;
		vertices[array[2]] = realHitPoint + b - vector * (TireWidth / 2f) + Vector3.up * d2;
		vertices[array[3]] = realHitPoint + b + vector * (TireWidth / 2f) + Vector3.up * d2;
		vertices[array[4]] = realHitPoint + b + vector * (TireWidth / 2f + BumpWidth * 0.5f) + Vector3.ProjectOnPlane(randomVectors[2], contactNormal) + contactNormal * d;
		vertices[array[5]] = realHitPoint + b + vector * (TireWidth / 2f + BumpWidth) + Vector3.ProjectOnPlane(randomVectors[3], contactNormal);
		if (PrevVerticesIDs != null && PrevVerticesIDs.Length == 6 && !breakStamp)
		{
			vertices[PrevVerticesIDs[0]] = lastPos + b - vector * (TireWidth / 2f + BumpWidth) + Vector3.ProjectOnPlane(randomVectors[0], contactNormal);
			vertices[PrevVerticesIDs[1]] = lastPos + b - vector * (TireWidth / 2f + BumpWidth * 0.5f) + Vector3.ProjectOnPlane(randomVectors[1], contactNormal) + contactNormal * BumpHeight + randomVectors[1];
			vertices[PrevVerticesIDs[2]] = lastPos + b - vector * (TireWidth / 2f) + Vector3.up * VerticalOffset;
			vertices[PrevVerticesIDs[3]] = lastPos + b + vector * (TireWidth / 2f) + Vector3.up * VerticalOffset;
			vertices[PrevVerticesIDs[4]] = lastPos + b + vector * (TireWidth / 2f + BumpWidth * 0.5f) + Vector3.ProjectOnPlane(randomVectors[2], contactNormal) + contactNormal * BumpHeight + randomVectors[2];
			vertices[PrevVerticesIDs[5]] = lastPos + b + vector * (TireWidth / 2f + BumpWidth) + Vector3.ProjectOnPlane(randomVectors[3], contactNormal);
		}
		if (!breakStamp)
		{
			PrevVerticesIDs = CurrentVerticesIDs;
		}
		CurrentVerticesIDs = array;
		if (currentMark > 0 || (currentMark == 0 && firstLapPassed))
		{
			int num3 = (currentMark <= 0) ? maxMarks : (currentMark - 1);
			int[] array2 = new int[6];
			for (int l = 0; l < 6; l++)
			{
				array2[l] = num3 * 6 + l;
			}
			for (int m = 0; m < 5; m++)
			{
				if (!breakStamp)
				{
					if (wheelCollider.rpm >= 0f)
					{
						triangles[num3 * 30 + m * 6] = array2[m];
						triangles[num3 * 30 + m * 6 + 1] = array[m];
						triangles[num3 * 30 + m * 6 + 2] = array[m + 1];
						triangles[num3 * 30 + m * 6 + 3] = array2[m];
						triangles[num3 * 30 + m * 6 + 4] = array[m + 1];
						triangles[num3 * 30 + m * 6 + 5] = array2[m + 1];
					}
					else
					{
						triangles[num3 * 30 + m * 6] = array2[m];
						triangles[num3 * 30 + m * 6 + 1] = array[m + 1];
						triangles[num3 * 30 + m * 6 + 2] = array[m];
						triangles[num3 * 30 + m * 6 + 3] = array2[m];
						triangles[num3 * 30 + m * 6 + 4] = array2[m + 1];
						triangles[num3 * 30 + m * 6 + 5] = array[m + 1];
					}
				}
				triangles[currentMark * 30 + m * 6] = 0;
				triangles[currentMark * 30 + m * 6 + 1] = 0;
				triangles[currentMark * 30 + m * 6 + 2] = 0;
				triangles[currentMark * 30 + m * 6 + 3] = 0;
				triangles[currentMark * 30 + m * 6 + 4] = 0;
				triangles[currentMark * 30 + m * 6 + 5] = 0;
			}
		}
		breakStamp = false;
		resultMesh.vertices = vertices;
		resultMesh.triangles = triangles;
		resultMesh.RecalculateBounds();
		currentMark++;
		lastPos = realHitPoint;
		if (currentMark > maxMarks)
		{
			currentMark = 0;
			firstLapPassed = true;
		}
	}

	private void UpdatePosition()
	{
		if (CurrentVerticesIDs != null && PrevVerticesIDs != null)
		{
			Vector3 realHitPoint = wheelCollider.wheelCollider.realHitPoint;
			Vector3 b = wheelCollider.transform.forward * spawnPointOffset * ((!(wheelCollider.rpm < -1f)) ? 1 : (-1));
			Vector3 a = Vector3.Cross(wheelCollider.wheelCollider.contactNormal, wheelCollider.transform.forward);
			Vector3 contactNormal = wheelCollider.wheelCollider.contactNormal;
			float num = Mathf.InverseLerp(0f, step, Vector3.Distance(realHitPoint + b, lastPos + b));
			float d = BumpHeight * Mathf.InverseLerp(0f, step, Vector3.Distance(realHitPoint + b, lastPos + b));
			vertices[PrevVerticesIDs[0]] = lastPos + b - a * (TireWidth / 2f + BumpWidth) * num + Vector3.ProjectOnPlane(randomVectors[0], contactNormal);
			vertices[PrevVerticesIDs[1]] = lastPos + b - a * (TireWidth / 2f + BumpWidth * 0.5f) * num + Vector3.ProjectOnPlane(randomVectors[1], contactNormal) + contactNormal * d + randomVectors[1];
			vertices[PrevVerticesIDs[2]] = lastPos + b - a * (TireWidth / 2f) * num + Vector3.up * VerticalOffset;
			vertices[PrevVerticesIDs[3]] = lastPos + b + a * (TireWidth / 2f) * num + Vector3.up * VerticalOffset;
			vertices[PrevVerticesIDs[4]] = lastPos + b + a * (TireWidth / 2f + BumpWidth * 0.5f) * num + Vector3.ProjectOnPlane(randomVectors[2], contactNormal) + contactNormal * d + randomVectors[2];
			vertices[PrevVerticesIDs[5]] = lastPos + b + a * (TireWidth / 2f + BumpWidth) * num + Vector3.ProjectOnPlane(randomVectors[3], contactNormal);
			vertices[CurrentVerticesIDs[0]] = realHitPoint + b;
			vertices[CurrentVerticesIDs[1]] = realHitPoint + b;
			vertices[CurrentVerticesIDs[2]] = realHitPoint + b + Vector3.up * VerticalOffset;
			vertices[CurrentVerticesIDs[3]] = realHitPoint + b + Vector3.up * VerticalOffset;
			vertices[CurrentVerticesIDs[4]] = realHitPoint + b;
			vertices[CurrentVerticesIDs[5]] = realHitPoint + b;
			for (int i = 0; i < LastVerticlesIDs.Length; i++)
			{
				vertices[LastVerticlesIDs[i]] = Vector3.Lerp(LastVerticlesPositions[i], vertices[LastVerticlesTargetsIDs[i]], num);
			}
			resultMesh.vertices = vertices;
		}
	}

	private void BreakStamp()
	{
		CurrentVerticesIDs = null;
		PrevVerticesIDs = null;
		breakStamp = true;
	}

	private void Update()
	{
		if (!(wheelCollider == null))
		{
			float num = (!(wheelCollider.rpm < -1f)) ? 1 : (-1);
			if (num != lastDirection)
			{
				BreakStamp();
			}
			lastDirection = num;
			if (Vector3.Distance(wheelCollider.wheelCollider.realHitPoint, lastPos) > step && RenderTrack)
			{
				Spawn3DStamp();
			}
			if (wheelCollider.IsGrounded && RenderTrack)
			{
				UpdatePosition();
			}
			else
			{
				BreakStamp();
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Terrain/Ramp Brush")]
public class RampBrush : MonoBehaviour
{
	private bool VERBOSE;

	public bool brushOn;

	public bool turnBrushOnVar;

	public bool isBrushHidden;

	public Vector3 brushPosition;

	public Vector3 beginRamp;

	public Vector3 endRamp;

	public float brushSize = 50f;

	public float brushOpacity = 1f;

	public float brushSoftness = 0.5f;

	public float brushSampleDensity = 4f;

	public bool shiftProcessed = true;

	public Vector3 backupVector;

	public int numSubDivPerSeg = 10;

	public float spacingJitter;

	public float sizeJitter;

	public bool multiPoint;

	public List<Vector3> controlPoints = new List<Vector3>();

	private List<float> _distBetweenPoints = new List<float>();

	private float _totalLength;

	private float _totalLengthPixels;

	public void OnDrawGizmos()
	{
		if (!turnBrushOnVar)
		{
			return;
		}
		Terrain x = (Terrain)GetComponent(typeof(Terrain));
		if (x == null)
		{
			return;
		}
		Gizmos.color = Color.cyan;
		float num = brushSize / 4f;
		Gizmos.DrawLine(brushPosition + new Vector3(0f - num, 0f, 0f), brushPosition + new Vector3(num, 0f, 0f));
		Gizmos.DrawLine(brushPosition + new Vector3(0f, 0f - num, 0f), brushPosition + new Vector3(0f, num, 0f));
		Gizmos.DrawLine(brushPosition + new Vector3(0f, 0f, 0f - num), brushPosition + new Vector3(0f, 0f, num));
		Gizmos.DrawWireCube(brushPosition, new Vector3(brushSize, 0f, brushSize));
		Gizmos.DrawWireSphere(brushPosition, brushSize / 2f);
		if (!multiPoint)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(beginRamp, brushSize / 2f);
			return;
		}
		Gizmos.color = Color.magenta;
		for (int i = 0; i < controlPoints.Count; i++)
		{
			Gizmos.DrawWireSphere(controlPoints[i], brushSize / 2f);
		}
		if (controlPoints.Count > 2)
		{
			double num2 = 1.0 / (((double)controlPoints.Count - 1.0) * 8.0) - 1E-14;
			calculateDistBetweenPoints(controlPoints);
			Ray ray = parameterizedLine(0f, controlPoints);
			double num3 = num2;
			int num4 = 0;
			while (num3 <= 1.0 && num4 < 1000)
			{
				Ray ray2 = parameterizedLine((float)num3, controlPoints);
				Gizmos.DrawLine(ray.origin, ray2.origin);
				ray = ray2;
				num3 += num2;
				num4++;
			}
		}
	}

	public int[] terrainCordsToBitmap(TerrainData terData, Vector3 v)
	{
		float num = terData.heightmapResolution;
		float num2 = terData.heightmapResolution;
		Vector3 size = terData.size;
		int num3 = (int)Mathf.Floor(num / size.x * v.x);
		int num4 = (int)Mathf.Floor(num2 / size.z * v.z);
		return new int[2]
		{
			num4,
			num3
		};
	}

	public float[] bitmapCordsToTerrain(TerrainData terData, int x, int y)
	{
		int heightmapWidth = terData.heightmapResolution;
		int heightmapHeight = terData.heightmapResolution;
		Vector3 size = terData.size;
		float num = (float)x * (size.z / (float)heightmapHeight);
		float num2 = (float)y * (size.x / (float)heightmapWidth);
		return new float[2]
		{
			num2,
			num
		};
	}

	public void toggleBrushOn()
	{
		if (turnBrushOnVar)
		{
			turnBrushOnVar = false;
		}
		else
		{
			turnBrushOnVar = true;
		}
	}

	public void rampBrush()
	{
		Terrain terrain = (Terrain)GetComponent(typeof(Terrain));
		if (terrain == null)
		{
			UnityEngine.Debug.LogError("No terrain component on this GameObject");
			return;
		}
		int num = 0;
		int num2 = 0;
		try
		{
			TerrainData terrainData = terrain.terrainData;
			int heightmapWidth = terrainData.heightmapResolution;
			int heightmapHeight = terrainData.heightmapResolution;
			Vector3 size = terrainData.size;
			if (VERBOSE)
			{
				UnityEngine.Debug.Log("terrainData heightmapHeight/heightmapWidth:" + heightmapWidth + " " + heightmapWidth);
			}
			if (VERBOSE)
			{
				UnityEngine.Debug.Log("terrainData heightMapResolution:" + terrainData.heightmapResolution);
			}
			if (VERBOSE)
			{
				UnityEngine.Debug.Log("terrainData size:" + terrainData.size);
			}
			Vector3 localScale = base.transform.localScale;
			base.transform.localScale = new Vector3(1f, 1f, 1f);
			Vector3 vector = beginRamp - base.transform.position;
			Vector3 vector2 = endRamp - base.transform.position;
			base.transform.localScale = localScale;
			int num3 = (int)Mathf.Floor((float)heightmapWidth / size.z * brushSize);
			int num4 = (int)Mathf.Floor((float)heightmapHeight / size.x * brushSize);
			int[] array = terrainCordsToBitmap(terrainData, vector);
			int[] array2 = terrainCordsToBitmap(terrainData, vector2);
			if (array[0] < 0 || array2[0] < 0 || array[1] < 0 || array2[1] < 0 || array[0] >= heightmapWidth || array2[0] >= heightmapWidth || array[1] >= heightmapHeight || array2[1] >= heightmapHeight)
			{
				UnityEngine.Debug.LogError("The start point or the end point was out of bounds. Make sure the gizmo is over the terrain before setting the start and end points.Note: that sometimes Unity does not update the collider after changing settings in the 'Set Resolution' dialog. Entering play mode should reset the collider.");
			}
			else
			{
				double num5 = Math.Sqrt((array2[0] - array[0]) * (array2[0] - array[0]) + (array2[1] - array[1]) * (array2[1] - array[1]));
				float[,] heights = terrainData.GetHeights(0, 0, heightmapWidth, heightmapHeight);
				vector2.y = heights[array2[0], array2[1]];
				vector.y = heights[array[0], array[1]];
				Vector3 vector3 = vector2 - vector;
				Vector3 vector4 = Vector3.Cross(rhs: new Vector3(0f - vector3.z, 0f, vector3.x), lhs: vector3);
				vector4.Normalize();
				Vector3 vector5 = new Vector3(vector3.x, 0f, vector3.z);
				float num6 = (!(brushSize < 15f)) ? ((float)(1.0 / num5 * (double)brushSampleDensity)) : (brushSize / 6f / vector3.magnitude);
				if (VERBOSE)
				{
					float[] array3 = bitmapCordsToTerrain(terrainData, array[0], array[1]);
					UnityEngine.Debug.Log("Local Begin Pos:" + vector);
					UnityEngine.Debug.Log("pixel begin coord:" + array[0] + " " + array[0]);
					UnityEngine.Debug.Log("Local begin Pos Rev Transformed:" + array3[0] + " " + array3[1]);
					array3 = bitmapCordsToTerrain(terrainData, array2[0], array2[1]);
					UnityEngine.Debug.Log("Local End Pos:" + vector2);
					UnityEngine.Debug.Log("pixel End coord:" + array2[0] + " " + array2[1]);
					UnityEngine.Debug.Log("Local End Pos Rev Transformed:" + array3[0] + " " + array3[1]);
					UnityEngine.Debug.Log("Sample Width/height: " + num3 + " " + num4);
					UnityEngine.Debug.Log("Brush Width: " + num6);
				}
				for (float num7 = 0f; num7 <= 1f; num7 += num6)
				{
					Vector3 v = vector + num7 * vector3;
					int[] array4 = terrainCordsToBitmap(terrainData, v);
					num = array4[0] - num3 / 2;
					num2 = array4[1] - num4 / 2;
					float[,] array5 = new float[num3, num4];
					for (int i = 0; i < num3; i++)
					{
						for (int j = 0; j < num4; j++)
						{
							if (num + i >= 0 && num2 + j >= 0 && num + i < heightmapWidth && num2 + j < heightmapHeight)
							{
								array5[i, j] = heights[num + i, num2 + j];
							}
							else
							{
								array5[i, j] = 0f;
							}
						}
					}
					num3 = array5.GetLength(0);
					num4 = array5.GetLength(1);
					float[,] array6 = (float[,])array5.Clone();
					for (int k = 0; k < num3; k++)
					{
						for (int l = 0; l < num4; l++)
						{
							float[] array7 = bitmapCordsToTerrain(terrainData, num + k, num2 + l);
							bool flag = false;
							if (vector5.x * (array7[0] - vector.x) + vector5.z * (array7[1] - vector.z) < 0f)
							{
								flag = true;
							}
							else if ((0f - vector5.x) * (array7[0] - vector2.x) - vector5.z * (array7[1] - vector2.z) < 0f)
							{
								flag = true;
							}
							if (!flag)
							{
								array6[k, l] = vector.y - (vector4.x * (array7[0] - vector.x) + vector4.z * (array7[1] - vector.z)) / vector4.y;
							}
						}
					}
					float num8 = (float)num3 / 2f;
					for (int m = 0; m < num3; m++)
					{
						for (int n = 0; n < num4; n++)
						{
							float num9 = array6[m, n];
							float num10 = array5[m, n];
							float num11 = Vector2.Distance(new Vector2(m, n), new Vector2(num8, num8));
							float num12 = 1f - (num11 - (num8 - num8 * brushSoftness)) / (num8 * brushSoftness);
							if (num12 < 0f)
							{
								num12 = 0f;
							}
							else if (num12 > 1f)
							{
								num12 = 1f;
							}
							num12 *= brushOpacity;
							float num13 = array5[m, n] = num9 * num12 + num10 * (1f - num12);
						}
					}
					for (int num14 = 0; num14 < num3; num14++)
					{
						for (int num15 = 0; num15 < num4; num15++)
						{
							if (num + num14 >= 0 && num2 + num15 >= 0 && num + num14 < heightmapWidth && num2 + num15 < heightmapHeight)
							{
								heights[num + num14, num2 + num15] = array5[num14, num15];
							}
						}
					}
				}
				terrainData.SetHeights(0, 0, heights);
			}
		}
		catch (Exception arg)
		{
			UnityEngine.Debug.LogError("A brush error occurred: " + arg);
		}
	}

	public void StrokePath()
	{
		_StrokePath();
	}

	public void _StrokePath()
	{
		Terrain terrain = (Terrain)GetComponent(typeof(Terrain));
		if (terrain == null)
		{
			UnityEngine.Debug.LogError("No terrain component on this GameObject");
			return;
		}
		int num = 0;
		int num2 = 0;
		try
		{
			TerrainData terrainData = terrain.terrainData;
			int heightmapWidth = terrainData.heightmapResolution;
			int heightmapHeight = terrainData.heightmapResolution;
			Vector3 size = terrainData.size;
			if (VERBOSE)
			{
				UnityEngine.Debug.Log("terrainData heightmapHeight/heightmapWidth:" + heightmapWidth + " " + heightmapWidth);
			}
			if (VERBOSE)
			{
				UnityEngine.Debug.Log("terrainData heightMapResolution:" + terrainData.heightmapResolution);
			}
			if (VERBOSE)
			{
				UnityEngine.Debug.Log("terrainData size:" + terrainData.size);
			}
			Vector3 localScale = base.transform.localScale;
			base.transform.localScale = new Vector3(1f, 1f, 1f);
			List<Vector3> list = new List<Vector3>();
			for (int i = 0; i < controlPoints.Count; i++)
			{
				list.Add(controlPoints[i] - base.transform.position);
			}
			base.transform.localScale = localScale;
			for (int j = 0; j < list.Count; j++)
			{
				int[] array = terrainCordsToBitmap(terrainData, list[j]);
				if (array[0] < 0 || array[1] < 0 || array[0] >= heightmapWidth || array[1] >= heightmapHeight)
				{
					UnityEngine.Debug.LogError("The start point or the end point was out of bounds. Make sure the gizmo is over the terrain before setting the start and end points.Note: that sometimes Unity does not update the collider after changing settings in the 'Set Resolution' dialog. Entering play mode should reset the collider.");
					return;
				}
			}
			int num3 = (int)Mathf.Floor((float)heightmapWidth / size.z * brushSize);
			int num4 = (int)Mathf.Floor((float)heightmapHeight / size.x * brushSize);
			float[,] heights = terrainData.GetHeights(0, 0, heightmapWidth, heightmapHeight);
			for (int k = 0; k < list.Count; k++)
			{
				int[] array2 = terrainCordsToBitmap(terrainData, list[k]);
				Vector3 value = list[k];
				value.y = heights[array2[0], array2[1]];
				list[k] = value;
			}
			calculateDistBetweenPoints(list);
			calculateDistBetweenPointsInPixels(list, terrainData);
			float num5 = brushSampleDensity / _totalLengthPixels;
			float num6 = brushSize / _totalLengthPixels;
			UnityEngine.Debug.Log("Sample w " + num3 + " h " + num4);
			UnityEngine.Debug.Log("parameterized brush width " + num6);
			if (num6 > 0.5f)
			{
				num6 = 0.5f;
			}
			if (VERBOSE)
			{
				for (int l = 0; l < list.Count; l++)
				{
					int[] array3 = terrainCordsToBitmap(terrainData, list[l]);
					float[] array4 = bitmapCordsToTerrain(terrainData, array3[0], array3[1]);
					UnityEngine.Debug.Log(l + " Local control Pos:" + list[l]);
					UnityEngine.Debug.Log(l + " pixel begin coord:" + array3[0] + " " + array3[0]);
					UnityEngine.Debug.Log(l + " Local begin Pos Rev Transformed:" + array4[0] + " " + array4[1]);
				}
				UnityEngine.Debug.Log("parameterized brush width " + num6);
			}
			StringBuilder message = new StringBuilder();
			Plane plane2 = default(Plane);
			Plane plane3 = default(Plane);
			for (float num7 = 0f; num7 <= 1f; num7 += num5)
			{
				Ray ray = parameterizedLine(num7, list);
				Vector3 direction = ray.direction;
				float x = 0f - direction.z;
				Vector3 direction2 = ray.direction;
				Vector3 vector = Vector3.Cross(new Vector3(x, 0f, direction2.x), ray.direction);
				vector.Normalize();
				if (spacingJitter > 0f)
				{
					float f = (float)Math.PI * 2f * UnityEngine.Random.value;
					float num8 = UnityEngine.Random.value + UnityEngine.Random.value;
					float num9 = (!(num8 > 1f)) ? num8 : (2f - num8);
					num9 *= spacingJitter * brushSize;
					Vector3 vector2 = new Vector3(num9 * Mathf.Cos(f), 0f, num9 * Mathf.Sin(f));
					if (VERBOSE)
					{
						UnityEngine.Debug.Log("jittering by " + vector2 + " dir " + ray.direction + " n " + vector);
					}
					Plane plane = new Plane(vector, ray.origin);
					Ray ray2 = new Ray(ray.origin + vector2, Vector3.up);
					if (plane.Raycast(ray2, out float enter))
					{
						Vector3 vector4 = ray.origin = ray2.origin + ray2.direction * enter;
					}
				}
				plane2 = new Plane((list[0] - list[1]).normalized, list[0]);
				plane3 = new Plane((list[list.Count - 1] - list[list.Count - 2]).normalized, list[list.Count - 1]);
				int[] array5 = terrainCordsToBitmap(terrainData, ray.origin);
				num = array5[0] - num3 / 2;
				num2 = array5[1] - num4 / 2;
				float[,] array6 = new float[num3, num4];
				for (int m = 0; m < num3; m++)
				{
					for (int n = 0; n < num4; n++)
					{
						if (num + m >= 0 && num2 + n >= 0 && num + m < heightmapWidth && num2 + n < heightmapHeight)
						{
							array6[m, n] = heights[num + m, num2 + n];
						}
						else
						{
							array6[m, n] = 0f;
						}
					}
				}
				float[,] array7 = (float[,])array6.Clone();
				for (int num10 = 0; num10 < num3; num10++)
				{
					for (int num11 = 0; num11 < num4; num11++)
					{
						float[] array8 = bitmapCordsToTerrain(terrainData, num + num10, num2 + num11);
						Vector3 vector5 = new Vector3(array8[0], 0f, array8[1]);
						bool flag = false;
						if (plane2.GetSide(vector5) && num7 < num6 / 2f)
						{
							flag = true;
						}
						else if (plane3.GetSide(vector5) && num7 > 1f - num6 / 2f)
						{
							flag = true;
						}
						if (!flag)
						{
							Plane plane4 = new Plane(vector, ray.origin);
							Ray ray3 = new Ray(vector5, Vector3.up);
							if (plane4.Raycast(ray3, out float enter2))
							{
								float[,] array9 = array7;
								int num12 = num10;
								int num13 = num11;
								Vector3 origin = ray3.origin;
								float y = origin.y;
								Vector3 direction3 = ray3.direction;
								array9[num12, num13] = y + direction3.y * enter2;
							}
						}
					}
				}
				float num14 = Mathf.Min((float)num4 / 2f, (float)num3 / 2f);
				for (int num15 = 0; num15 < num3; num15++)
				{
					for (int num16 = 0; num16 < num4; num16++)
					{
						float num17 = array7[num15, num16];
						float num18 = array6[num15, num16];
						float num19 = Vector2.Distance(new Vector2(num15, num16), new Vector2(num14, num14));
						float num20 = (1f - num19 / num14) / brushSoftness;
						if (num20 < 0f)
						{
							num20 = 0f;
						}
						else if (num20 > 1f)
						{
							num20 = 1f;
						}
						num20 *= brushOpacity;
						float num21 = array6[num15, num16] = num17 * num20 + num18 * (1f - num20);
					}
				}
				for (int num22 = 0; num22 < num3; num22++)
				{
					for (int num23 = 0; num23 < num4; num23++)
					{
						if (num + num22 >= 0 && num2 + num23 >= 0 && num + num22 < heightmapWidth && num2 + num23 < heightmapHeight)
						{
							heights[num + num22, num2 + num23] = array6[num22, num23];
						}
					}
				}
			}
			UnityEngine.Debug.Log(message);
			terrainData.SetHeights(0, 0, heights);
		}
		catch (Exception arg)
		{
			UnityEngine.Debug.LogError("A brush error occurred: " + arg);
		}
	}

	private void calculateDistBetweenPoints(List<Vector3> cps)
	{
		_distBetweenPoints.Clear();
		_totalLength = 0f;
		for (int i = 1; i < cps.Count; i++)
		{
			_distBetweenPoints.Add((cps[i] - cps[i - 1]).magnitude);
			_totalLength += _distBetweenPoints[_distBetweenPoints.Count - 1];
		}
	}

	private void calculateDistBetweenPointsInPixels(List<Vector3> cps, TerrainData terData)
	{
		_totalLengthPixels = 0f;
		int[] array = terrainCordsToBitmap(terData, cps[0]);
		for (int i = 1; i < cps.Count; i++)
		{
			int[] array2 = terrainCordsToBitmap(terData, cps[i]);
			_totalLengthPixels += Mathf.Sqrt((array2[0] - array[0]) * (array2[0] - array[0]) + (array2[1] - array[1]) * (array2[1] - array[1]));
			array = array2;
		}
	}

	private Ray parameterizedLine(float t, List<Vector3> cps, StringBuilder sb = null)
	{
		if (cps.Count < 2)
		{
			UnityEngine.Debug.LogError("Less than two control points.");
			return default(Ray);
		}
		if (t < 0f)
		{
			t = 0f;
		}
		if (t >= 1f)
		{
			t = 1f;
		}
		Vector3[] array = new Vector3[cps.Count + 2];
		for (int i = 0; i < cps.Count; i++)
		{
			array[i + 1] = cps[i];
		}
		array[0] = 2f * cps[0] - cps[1];
		array[array.Length - 1] = 2f * cps[cps.Count - 1] - cps[cps.Count - 2];
		float num = t * _totalLength;
		int num2 = 0;
		float num3 = 0f;
		bool flag = false;
		float num4 = 0f;
		while (!flag)
		{
			if (num4 + _distBetweenPoints[num2] < num)
			{
				num4 += _distBetweenPoints[num2];
				if (num2 < controlPoints.Count - 2)
				{
					num2++;
					continue;
				}
				flag = true;
				num3 = 1f;
			}
			else
			{
				flag = true;
				num3 = (num - num4) / _distBetweenPoints[num2];
			}
		}
		if (num2 >= controlPoints.Count - 1)
		{
			num2--;
		}
		if (num3 > 1f)
		{
			num3 = 1f;
		}
		num2++;
		if (num2 + 2 > array.Length - 1)
		{
			UnityEngine.Debug.LogError("Off end=" + t);
		}
		sb?.AppendFormat("t={0} cpIdx={1} nt={2}\n", t, num2, num3);
		Vector3 a = array[num2 - 1];
		Vector3 a2 = array[num2];
		Vector3 vector = array[num2 + 1];
		Vector3 vector2 = array[num2 + 2];
		float d = num3 * num3;
		float d2 = num3 * num3 * num3;
		Vector3 origin = 0.5f * (2f * a2 + (-a + vector) * num3 + (2f * a - 5f * a2 + 4f * vector - vector2) * d + (-a + 3f * a2 - 3f * vector + vector2) * d2);
		return new Ray(origin, (0.5f * (-a + vector + num3 * (4f * a - 10f * a2 + 8f * vector - 2f * vector2) + d * (-3f * a + 9f * a2 - 9f * vector + 3f * vector2))).normalized);
	}
}

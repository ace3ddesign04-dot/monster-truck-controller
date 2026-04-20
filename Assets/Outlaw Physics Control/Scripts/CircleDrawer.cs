using System;
using UnityEngine;

public class CircleDrawer : MonoBehaviour
{
	public int pointsCount = 20;

	public float radius = 3f;

	public float width = 0.5f;

	public Material mat;

	private LineRenderer lineRenderer;

	private Terrain terrain;

	private void Awake()
	{
		terrain = Terrain.activeTerrain;
		base.gameObject.name = "CircleDrawer";
		lineRenderer = base.gameObject.AddComponent<LineRenderer>();
		lineRenderer.loop = true;
		lineRenderer.alignment = LineAlignment.TransformZ;
		UpdateCircle();
	}

	private void Update()
	{
		UpdateCircle();
	}

	private void UpdateCircle()
	{
		lineRenderer.positionCount = pointsCount;
		lineRenderer.widthMultiplier = width;
		lineRenderer.material = mat;
		base.transform.eulerAngles = new Vector3(90f, 0f, 0f);
		float num = 0f;
		for (int i = 0; i < pointsCount; i++)
		{
			num += 1f / (float)(pointsCount - 1) * (float)Math.PI * 2f;
			float x = radius * Mathf.Cos(num);
			float z = radius * Mathf.Sin(num);
			Vector3 position = base.transform.position + new Vector3(x, 0f, z);
			lineRenderer.SetPosition(i, position);
		}
	}

	private void OnDisable()
	{
		lineRenderer.enabled = false;
	}

	private void OnEnable()
	{
		lineRenderer.enabled = true;
	}
}

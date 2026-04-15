using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerRoute : MonoBehaviour
{
	public static float completionMoney = 750f;

	public static float bronzeMoney = 1000f;

	public static float silverMoney = 1500f;

	public static float goldMoney = 2000f;

	public static float completionXP = 10f;

	public static float bronzeXP = 20f;

	public static float silverXP = 30f;

	public static float goldXP = 40f;

	public static float completionGolds = 5f;

	public static float bronzeGolds = 10f;

	public static float silverGolds = 15f;

	public static float goldGolds = 20f;

	public string routeName;

	public List<Transform> checkpoints = new List<Transform>();

	public LineRenderer lineRenderer;

	public string routeID;

	private GameObject routeIndicator;

	private GameObject startCheckpoint;

	public float routeRecord;

	public string routeRecordKeeper;

	public void InitializeLineRenderer(Material lineRendererMaterial)
	{
		lineRenderer = new GameObject("Line renderer").AddComponent<LineRenderer>();
		lineRenderer.transform.parent = base.transform;
		lineRenderer.material = lineRendererMaterial;
		lineRenderer.widthMultiplier = 1f;
		lineRenderer.numCapVertices = 3;
		lineRenderer.numCornerVertices = 3;
		lineRenderer.transform.eulerAngles = new Vector3(90f, 0f, 0f);
		lineRenderer.alignment = LineAlignment.TransformZ;
		lineRenderer.positionCount = 0;
	}

	public void UpdateLineRenderer()
	{
		lineRenderer.positionCount = checkpoints.Count;
		for (int i = 0; i < checkpoints.Count; i++)
		{
			lineRenderer.SetPosition(i, checkpoints[i].transform.position);
		}
	}

	public void AddCheckpoint(Vector3 position)
	{
		GameObject gameObject = new GameObject("Checkpoint");
		gameObject.transform.parent = base.transform;
		gameObject.transform.position = position;
		checkpoints.Add(gameObject.transform);
		UpdateLineRenderer();
		AlignCheckpoints();
	}

	public void UpdateCheckpointPrefabs()
	{
		LevelEditorResources editorResources = LevelEditorTools.editorResources;
		ToggleCheckpoints(on: true);
		for (int i = 0; i < checkpoints.Count; i++)
		{
			for (int j = 0; j < checkpoints[i].transform.childCount; j++)
			{
				UnityEngine.Object.Destroy(checkpoints[i].transform.GetChild(j).gameObject);
			}
			GameObject original = editorResources.routeStartPrefab;
			if (i > 0)
			{
				original = editorResources.routeCheckpointPrefab;
			}
			if (i == checkpoints.Count - 1 && i > 0)
			{
				original = editorResources.routeFinishPrefab;
			}
			GameObject gameObject = UnityEngine.Object.Instantiate(original, checkpoints[i].transform.position, checkpoints[i].transform.rotation, checkpoints[i].transform);
			gameObject.GetComponent<PlayerRouteCheckpoint>().checkpointID = i;
			if (i == 0)
			{
				startCheckpoint = gameObject;
			}
		}
		if (checkpoints.Count > 0)
		{
			UnityEngine.Object.DestroyImmediate(routeIndicator);
			routeIndicator = UnityEngine.Object.Instantiate(editorResources.routeIndicatorPrefab, checkpoints[0].transform.position, checkpoints[0].transform.rotation, checkpoints[0].transform);
			routeIndicator.SetActive(value: false);
		}
		UpdateLineRenderer();
		AlignCheckpoints();
	}

	[ContextMenu("Bake")]
	public void BakeRoute()
	{
		lineRenderer.enabled = false;
		ToggleCheckpoints(on: false);
		foreach (Transform checkpoint in checkpoints)
		{
			PlayerRouteCheckpoint componentInChildren = checkpoint.GetComponentInChildren<PlayerRouteCheckpoint>(includeInactive: true);
			if (componentInChildren != null)
			{
				componentInChildren.ToggleTapTarget(on: false);
			}
		}
	}

	public void UnBakeRoute()
	{
		lineRenderer.enabled = true;
		ToggleCheckpoints(on: true);
		foreach (Transform checkpoint in checkpoints)
		{
			PlayerRouteCheckpoint componentInChildren = checkpoint.GetComponentInChildren<PlayerRouteCheckpoint>(includeInactive: true);
			if (componentInChildren != null)
			{
				componentInChildren.ToggleTapTarget(on: true);
			}
		}
	}

	public void ToggleCheckpoints(bool on)
	{
		if (routeIndicator != null)
		{
			routeIndicator.SetActive(!on);
		}
		if (startCheckpoint != null)
		{
			startCheckpoint.SetActive(on);
		}
		for (int i = 1; i < checkpoints.Count; i++)
		{
			checkpoints[i].gameObject.SetActive(on);
		}
	}

	public void AlignCheckpoints()
	{
		for (int i = 0; i < checkpoints.Count; i++)
		{
			Ray ray = new Ray(checkpoints[i].position + Vector3.up * 50f, Vector3.down);
			RaycastHit[] array = (from h in Physics.RaycastAll(ray)
				orderby h.distance
				select h).ToArray();
			for (int j = 0; j < array.Length; j++)
			{
				if (!(array[j].collider.GetComponentInParent<PlayerRoute>() != null) && !array[j].collider.name.Contains("MudIndicator"))
				{
					checkpoints[i].position = array[j].point;
					break;
				}
			}
			if (i < checkpoints.Count - 1)
			{
				Vector3 position = checkpoints[i + 1].transform.position;
				Vector3 position2 = checkpoints[i].transform.position;
				position.y = position2.y;
				Vector3 normalized = (position - checkpoints[i].transform.position).normalized;
				if (i > 0)
				{
					Vector3 position3 = checkpoints[i - 1].transform.position;
					Vector3 position4 = checkpoints[i].transform.position;
					position3.y = position4.y;
					Vector3 normalized2 = (checkpoints[i].transform.position - position3).normalized;
					Vector3 forward = (normalized + normalized2) / 2f;
					checkpoints[i].transform.rotation = Quaternion.LookRotation(forward);
				}
				else
				{
					checkpoints[i].transform.rotation = Quaternion.LookRotation(normalized);
				}
			}
			Renderer[] componentsInChildren = checkpoints[i].GetComponentsInChildren<Renderer>();
			foreach (Renderer renderer in componentsInChildren)
			{
				renderer.gameObject.SetActive(value: false);
				ray = new Ray(renderer.transform.position + Vector3.up * 50f, Vector3.down);
				array = (from h in Physics.RaycastAll(ray)
					orderby h.distance
					select h).ToArray();
				for (int l = 0; l < array.Length; l++)
				{
					if (!(array[l].collider.GetComponentInParent<PlayerRoute>() != null))
					{
						renderer.transform.position = array[l].point;
						break;
					}
				}
				renderer.gameObject.SetActive(value: true);
			}
		}
	}

	public float RouteLength()
	{
		float num = 0f;
		if (checkpoints != null)
		{
			for (int i = 0; i < checkpoints.Count - 1; i++)
			{
				if (!(checkpoints[i] == null) && !(checkpoints[i + 1] == null))
				{
					num += Vector3.Distance(checkpoints[i].position, checkpoints[i + 1].position);
				}
			}
		}
		return num;
	}

	public string Serialize()
	{
		string text = routeName + "|" + routeID + "|";
		for (int i = 0; i < checkpoints.Count; i++)
		{
			object[] array = new object[5];
			Vector3 position = checkpoints[i].position;
			array[0] = Mathf.Round(position.x * 10f) / 10f;
			array[1] = ";";
			Vector3 position2 = checkpoints[i].position;
			array[2] = Mathf.Round(position2.y * 10f) / 10f;
			array[3] = ";";
			Vector3 position3 = checkpoints[i].position;
			array[4] = Mathf.Round(position3.z * 10f) / 10f;
			string str = string.Concat(array);
			text = text + str + "|";
		}
		return text;
	}

	public void Deserialize(string str)
	{
		checkpoints = new List<Transform>();
		string[] array = str.Split('|');
		routeName = array[0];
		routeID = array[1];
		for (int i = 2; i < array.Length - 1; i++)
		{
			string text = array[i];
			string[] array2 = text.Split(';');
			float x = float.Parse(array2[0]);
			float y = float.Parse(array2[1]);
			float z = float.Parse(array2[2]);
			Vector3 position = new Vector3(x, y, z);
			AddCheckpoint(position);
		}
	}
}
